using System;
using System.Reflection;
using System.Threading.Tasks;
using Electrum.Common.Types;
using Electrum.EventBus.Abstractions;
using Electrum.EventBus.Extensions;
using Electrum.EventBus.Handlers;
using Electrum.EventBus.Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Enrichers.MessageContext;

namespace Electrum.EventBusRabbitMQ
{
    public class BusSubscriber : IBusSubscriber
    {
        private IBusClient _busClient;
        private RabbitMqOptions _options;
        private string _defaultNamespace;
        private IServiceProvider _serviceProvider;
        private int _retries;
        private int _retryInterval;
        private ILogger<BusSubscriber> _logger;

        public BusSubscriber(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetRequiredService<ILogger<BusSubscriber>>();
            _busClient = serviceProvider.GetRequiredService<IBusClient>();
            _options = serviceProvider.GetRequiredService<RabbitMqOptions>();
            _defaultNamespace = _options.Namespace;
            _retries = _options.Retries;
            _retryInterval = _options.RetryInterval;
        }

        public IBusSubscriber SubscribeCommand<TCommand>(string @namespace = null, string queueName = null,
            Func<TCommand, ElectrumException, IRejectedEvent> onError = null) where TCommand : ICommand
        {
            _busClient.SubscribeAsync<TCommand, CorrelationContext>(async (command, context) =>
                {
                    var commandHandler = _serviceProvider.GetService<ICommandHandler<TCommand>>();
                    await TryHandleAsync(command, context, () => commandHandler.HandleAsync(command, context), onError);
                },
                ctx => ctx.UseSubscribeConfiguration(cfg =>
                    cfg.FromDeclaredQueue(q => q.WithName(GetQueueName<TCommand>(@namespace, queueName)))));

            return this;
        }

        public IBusSubscriber SubscribeEvent<TEvent>(string @namespace = null, string queueName = null,
            Func<TEvent, ElectrumException, IRejectedEvent> onError = null) where TEvent : IEvent
        {
            _busClient.SubscribeAsync<TEvent, CorrelationContext>(async (command, context) =>
                {
                    var eventHandler = _serviceProvider.GetService<IEventHandler<TEvent>>();
                    await TryHandleAsync(command, context, () => eventHandler.HandleAsync(command, context), onError);
                },
                ctx => ctx.UseSubscribeConfiguration(cfg =>
                    cfg.FromDeclaredQueue(q => q.WithName(GetQueueName<TEvent>(@namespace, queueName)))));

            return this;
        }

        private async Task<Acknowledgement> TryHandleAsync<TMessage>(TMessage message,
            ICorrelationContext correlationContext,
            Func<Task> handle, Func<TMessage, ElectrumException, IRejectedEvent> onError = null)
        {
            var currentRetry = 0;
            var policy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(_retries, i => TimeSpan.FromSeconds(_retryInterval));

            var messageName = message.GetType().Name;
            return await policy.ExecuteAsync<Acknowledgement>(async () =>
            {
                try
                {
                    var retryMessage = currentRetry == 0
                        ? string.Empty
                        : $"Retry: {currentRetry}'.";

                    var preLogMessage = $"Handling a message: '{messageName}' " +
                                        $"with correlation id: '{correlationContext.Id}'. {retryMessage}";
                        
                    _logger.LogInformation(preLogMessage);
                    
                    await handle();
                    
                    var postLogMessage = $"Handled a message: '{messageName}' " +
                                         $"with correlation id: '{correlationContext.Id}'. {retryMessage}";
                    _logger.LogInformation(postLogMessage);
                    return new Ack();
                }
                catch (Exception exception)
                {
                    currentRetry++;
                    _logger.LogError(exception, exception.Message);

                    if (exception is ElectrumException electrumException && onError != null)
                    {
                        var rejectedEvent = onError(message, electrumException);
                        await _busClient.PublishAsync(rejectedEvent, ctx => ctx.UseMessageContext(correlationContext));
                        _logger.LogInformation($"Published a rejected event: '{rejectedEvent.GetType().Name}' " +
                                               $"for the message: '{messageName}' with correlation id: '{correlationContext.Id}'.");
                    }
                    throw new Exception($"Unable to handle a message: '{messageName}' " +
                                        $"with correlation id: '{correlationContext.Id}', " +
                                        $"retry {currentRetry - 1}/{_retries}...");
                }
            });
        }
        
        private string GetQueueName<T>(string @namespace = null, string name = null)
        {
            @namespace = string.IsNullOrWhiteSpace(@namespace)
                ? (string.IsNullOrWhiteSpace(_defaultNamespace) ? string.Empty : _defaultNamespace)
                : @namespace;

            var separatedNamespace = string.IsNullOrWhiteSpace(@namespace) ? string.Empty : $"{@namespace}.";

            return (string.IsNullOrWhiteSpace(name)
                ? $"{Assembly.GetEntryAssembly().GetName().Name}/{separatedNamespace}{typeof(T).Name.Underscore()}"
                : $"{name}/{separatedNamespace}{typeof(T).Name.Underscore()}").ToLowerInvariant();
        }
    }
}
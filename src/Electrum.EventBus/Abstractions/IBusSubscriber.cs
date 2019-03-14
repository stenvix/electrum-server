using System;
using Electrum.Common.Types;
using Electrum.EventBus.Messages;

namespace Electrum.EventBus.Abstractions
{
    public interface IBusSubscriber
    {
        IBusSubscriber SubscribeCommand<TCommand>(string @namespace = null, string queueName = null,
            Func<TCommand, ElectrumException, IRejectedEvent> onError = null)
            where TCommand : ICommand;

        IBusSubscriber SubscribeEvent<TEvent>(string @namespace = null, string queueName = null,
            Func<TEvent, ElectrumException, IRejectedEvent> onError = null)
            where TEvent : IEvent;
    }
}
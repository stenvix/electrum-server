using System.Threading.Tasks;
using Electrum.EventBus.Abstractions;
using Electrum.EventBus.Messages;

namespace Electrum.EventBus.Handlers
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event, ICorrelationContext context);
    }
}
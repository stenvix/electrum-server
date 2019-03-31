using Electrum.EventBus.Messages;
using System.Threading.Tasks;

namespace Electrum.EventBus.Abstractions
{
    public interface IBusPublisher
    {
        Task SendAsync<TCommand>(TCommand command, ICorrelationContext context)
            where TCommand : ICommand;
    }
}
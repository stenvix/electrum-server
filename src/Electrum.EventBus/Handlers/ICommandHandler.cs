using System.Threading.Tasks;
using Electrum.EventBus.Abstractions;
using Electrum.EventBus.Messages;

namespace Electrum.EventBus.Handlers
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command, ICorrelationContext context);
    }
}
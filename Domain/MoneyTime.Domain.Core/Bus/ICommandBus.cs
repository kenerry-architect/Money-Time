using MoneyTime.Domain.Core.Messaging;
using System.Threading.Tasks;

namespace MoneyTime.Domain.Core.Bus
{
    public interface ICommandBus
    {
        Task SendCommand<TCommand>(TCommand command) where TCommand : Command;
    }
}

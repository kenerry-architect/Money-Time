using MoneyTime.Domain.Core.Events;
using System.Threading.Tasks;

namespace MoneyTime.Domain.Core.Bus
{
    public interface IEventBus
    {
        Task RaiseEvent<TEvent>(TEvent @event) where TEvent : Event;
    }
}

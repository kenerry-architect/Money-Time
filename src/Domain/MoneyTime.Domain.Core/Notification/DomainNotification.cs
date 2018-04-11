using MediatR;

namespace MoneyTime.Domain.Core.Notification
{
    public class DomainNotification : INotification
    {
        public DomainNotification(string target, string error)
        {
            Target = target;
            Message = error;
        }

        public string Target { get; }
        public string Message { get; }
    }
}

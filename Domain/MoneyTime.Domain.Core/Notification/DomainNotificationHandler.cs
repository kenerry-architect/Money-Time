using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace MoneyTime.Domain.Core.Notification
{
    public class DomainNotificationHandler : INotificationHandler<DomainNotification>, IDisposable
    {
        private readonly List<DomainNotification> _notifications;

        public DomainNotificationHandler()
        {
            _notifications = new List<DomainNotification>();
        }

        public Task Handle(DomainNotification notification, CancellationToken cancellationToken)
        {
            _notifications.Add(notification);
            return Task.CompletedTask;
        }

        public List<DomainNotification> GetNotifications()
        {
            return _notifications;
        }

        public bool HasNotifications()
        {
            return GetNotifications().Any();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

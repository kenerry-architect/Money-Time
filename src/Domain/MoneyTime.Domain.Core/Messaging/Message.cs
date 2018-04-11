using System;
using MediatR;

namespace MoneyTime.Domain.Core.Messaging
{
    public class Message : INotification
    {
        public Message()
        {
            MessageType = GetType().Name;
        }

        public Guid AggregateId { get; protected set; }
        public string MessageType { get; protected set; }
    }
}

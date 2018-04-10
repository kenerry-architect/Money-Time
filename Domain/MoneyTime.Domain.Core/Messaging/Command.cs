namespace MoneyTime.Domain.Core.Messaging
{
    public abstract class Command : Message
    {
        public abstract bool IsValid();
    }
}

namespace Infrastructure.Messaging.Models;

public class Message
{
    private readonly Guid _messageId;
    private readonly string _messageType;

    protected Message() : this(Guid.NewGuid())
    {
    }

    protected Message(Guid messageId)
    {
        _messageId = messageId;
        _messageType = GetType().Name;
    }

    protected Message(string messageType) : this(Guid.NewGuid())
    {
        _messageType = messageType;
    }

    protected Message(Guid messageId, string messageType)
    {
        _messageId = messageId;
        _messageType = messageType;
    }

    protected Guid GetMessageId()
    {
        return _messageId;
    }
}
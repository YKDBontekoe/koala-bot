namespace Koala.MessagePublisherService.Models;

public class BaseMessage
{
    public string Message { get; set; }
    public BaseChannel Channel { get; set; }
    public BaseUser User { get; set; }
}
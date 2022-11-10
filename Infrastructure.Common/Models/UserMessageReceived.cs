namespace Infrastructure.Common.Models;

public class UserMessageReceived
{
    public BaseUser User { get; set; }
    public BaseChannel Channel { get; set; }
    public string Message { get; set; }
    public DateTime Time { get; set; }

    public override string ToString()
    {
        return $"[{Time}] {User.ToString()} in {Channel.ToString()}: {Message}";
    }
}
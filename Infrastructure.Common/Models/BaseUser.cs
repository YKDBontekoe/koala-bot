namespace Infrastructure.Common.Models;

public class BaseUser
{
    public ulong Id { get; set; }
    public string Username { get; set; }

    public override string ToString()
    {
        return Username;
    }
}
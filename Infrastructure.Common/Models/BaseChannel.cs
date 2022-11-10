namespace Infrastructure.Common.Models;

public class BaseChannel
{
    public ulong Id { get; set; }
    public string Name { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
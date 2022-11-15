using Koala.MessageConsumerService.Models;

namespace Koala.MessageConsumerService.Repositories.Interfaces;

public interface IMessageRepository
{
    Task AddMessageAsync(BaseMessage message);
}
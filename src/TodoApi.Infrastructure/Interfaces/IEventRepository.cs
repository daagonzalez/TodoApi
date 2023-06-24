using TodoApi.Domain.Interfaces;

namespace TodoApi.Infrastructure.Interfaces;

public interface IEventRepository
{
    void Publish(IEvent @event);
}
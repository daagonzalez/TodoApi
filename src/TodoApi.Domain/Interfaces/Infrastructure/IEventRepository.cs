namespace TodoApi.Domain.Interfaces.Infrastructure;

public interface IEventRepository
{
    void Publish(IEvent @event);
}
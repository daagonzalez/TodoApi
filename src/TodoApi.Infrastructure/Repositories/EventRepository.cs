using TodoApi.Domain.Interfaces;
using TodoApi.Domain.Interfaces.Infrastructure;

namespace TodoApi.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    public void Publish(IEvent @event)
    {
        Console.WriteLine("===============");
        Console.WriteLine("EVENT PUBLISHED");
        Console.WriteLine("Id: {0}", @event.Id);
        Console.WriteLine("Type: {0}", @event.Type);
        Console.WriteLine("Data: {0}", @event.GetData());
        Console.WriteLine("===============");
    }
}

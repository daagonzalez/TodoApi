using TodoApi.Domain.Interfaces;

namespace TodoApi.Domain.Models;

public abstract class AggregateRoot
{
    private List<IEvent> _events = new();
    public IReadOnlyCollection<IEvent> Events => _events.AsReadOnly();

    public void AddEvent(IEvent @event) { _events.Add(@event); }
}
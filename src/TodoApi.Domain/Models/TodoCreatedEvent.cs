using TodoApi.Domain.Interfaces;

namespace TodoApi.Domain.Models;

public record TodoCreatedEvent(Guid TodoId, string Description) : IEvent
{
    public Guid Id { get; } = Guid.NewGuid();

    public string Type => "Todo.Created.v1";

    public object? GetData()
    {
        return new { TodoId, Description };
    }
}
namespace TodoApi.Domain.Interfaces;

public interface IEvent
{
    Guid Id { get; }
    string Type { get; }

    object? GetData();
}
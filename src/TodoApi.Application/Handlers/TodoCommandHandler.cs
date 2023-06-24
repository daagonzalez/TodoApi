using TodoApi.Application.Interfaces;
using TodoApi.Domain.Models;
using TodoApi.Infrastructure.Interfaces;

namespace TodoApi.Application.Handlers;

public class TodoCommandHandler : ITodoCommandHandler
{
    private readonly ITodoRepository _todoRepository;
    private readonly IEventRepository _eventRepository;

    public TodoCommandHandler(ITodoRepository todoRepository,
                               IEventRepository eventRepository)
    {
        _todoRepository = todoRepository;
        _eventRepository = eventRepository;
    }
    public Todo Create(string description)
    {
        var todoId = Guid.NewGuid();
        var newTodo = Todo.Create(todoId, description);

        _todoRepository.Store(newTodo);

        foreach (var @event in newTodo.Events)
        {
            _eventRepository.Publish(@event);
        }

        return newTodo;
    }
}
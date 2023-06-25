using TodoApi.Application.Interfaces;
using TodoApi.Domain.Interfaces.Infrastructure;
using TodoApi.Domain.Models;

namespace TodoApi.Application.Handlers;
public class TodoQueryHandler : ITodoQueryHandler
{
    private readonly ITodoRepository _todoRepository;

    public TodoQueryHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }
    public Todo Fetch(Guid id)
    {
        return _todoRepository.Read(id);
    }

    public List<Todo> FetchAll()
    {
        var result = _todoRepository.Read();
        return result;
    }
}
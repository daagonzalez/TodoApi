using TodoApi.Application.Interfaces;
using TodoApi.Domain.Models;
using TodoApi.Infrastructure.Interfaces;

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
        return _todoRepository.Read();
    }
}
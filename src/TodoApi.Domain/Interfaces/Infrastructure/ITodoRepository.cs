using TodoApi.Domain.Models;

namespace TodoApi.Domain.Interfaces.Infrastructure;

public interface ITodoRepository
{
    void Store(Todo todo);
    Todo Read(Guid id);
    List<Todo> Read();
}
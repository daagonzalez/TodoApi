using TodoApi.Domain.Models;

namespace TodoApi.Infrastructure.Interfaces;

public interface ITodoRepository
{
    void Store(Todo todo);
    Todo Read(Guid id);
    List<Todo> Read();
}
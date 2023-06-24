using TodoApi.Domain.Models;

namespace TodoApi.Application.Interfaces;
public interface ITodoQueryHandler
{
    Todo Fetch(Guid id);
    List<Todo> FetchAll();
}
using TodoApi.Domain.Models;

namespace TodoApi.Application.Interfaces;
public interface ITodoCommandHandler
{
    Todo Create(string description);
}
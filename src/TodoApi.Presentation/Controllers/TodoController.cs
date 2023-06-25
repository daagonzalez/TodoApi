using Microsoft.AspNetCore.Mvc;
using TodoApi.Application.Interfaces;
using TodoApi.Presentation.Models;

namespace TodoApi.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;
    private readonly ITodoCommandHandler _todoCommandHandler;
    private readonly ITodoQueryHandler _todoQueryHandler;

    public TodoController(ILogger<TodoController> logger, 
    ITodoCommandHandler todoCommandHandler, 
    ITodoQueryHandler todoQueryHandler)
    {
        _logger = logger;
        _todoCommandHandler = todoCommandHandler;
        _todoQueryHandler = todoQueryHandler;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var response = _todoQueryHandler.FetchAll();
        var todos = response.Select(todo => 
        new TodoResponse 
        { 
            TodoId = todo.Id, 
            Description = todo.Description, 
            Completed = todo.Completed 
        });
        return Ok(todos.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        try
        {
            var response = _todoQueryHandler.Fetch(id);
            return Ok(new TodoResponse
            { 
                TodoId = response.Id, 
                Description = response.Description, 
                Completed = response.Completed 
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR: {0}", ex.Message);
            return NotFound();
        }
    }

    [HttpPost]
    public IActionResult Create([FromBody]TodoRequest request)
    {
        var createdTodo = _todoCommandHandler.Create(request.Description);
        return CreatedAtAction(nameof(GetById), 
        new { id = createdTodo.Id }, new TodoResponse
        {
            TodoId = createdTodo.Id, 
            Description = createdTodo.Description, 
            Completed = createdTodo.Completed 
        });
    }    
}

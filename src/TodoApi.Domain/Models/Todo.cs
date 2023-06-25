namespace TodoApi.Domain.Models;

public class Todo : AggregateRoot
{
    public Guid Id { get; private set; }
    public string Description { get; private set; }
    public bool Completed { get; private set; }

    private Todo(Guid id, string description)
    {
        Id = id;
        Description = description;
        Completed = false;
    }

    public void Complete()
    {
        // Add validation
        Completed = true;

        // business event goes here
    }

    public static Todo Create(Guid id, string description)
    {
        // Validation
        var todo = new Todo(id, description);

        // Additional business logic
        todo.AddEvent(new TodoCreatedEvent(id, description));

        return todo;
    }

    public static Todo CreateForTesting(Guid id, string description, bool completed)
    {
        var todo = new Todo(id, description)
        {
            Completed = completed
        };

        return todo;
    }
}
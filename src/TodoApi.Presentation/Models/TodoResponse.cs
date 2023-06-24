namespace TodoApi.Presentation.Models;

public class TodoResponse
{
    public Guid TodoId { get; set; }
    public string Description { get; set; }
    public bool Completed { get; set; }
}
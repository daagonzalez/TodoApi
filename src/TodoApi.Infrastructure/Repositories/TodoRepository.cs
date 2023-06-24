using TodoApi.Domain.Models;
using TodoApi.Infrastructure.Interfaces;

namespace TodoApi.Infrastructure.Repositories;
public class TodoRepository : ITodoRepository
{
    public Todo Read(Guid id)
    {
        var docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        using var sr = new StreamReader(Path.Combine(docPath, "TodosDatabase.txt"), true);
        var lineRead = "";
        while (!sr.EndOfStream)
        {
            lineRead = sr.ReadLine();
            if (lineRead is not null)
            {
                var dbRecord = lineRead.Split(";");
                if (dbRecord[0] == id.ToString())
                {
                    var todo = Todo.Create(Guid.Parse(dbRecord[0]), dbRecord[1]);
                    if (bool.Parse(dbRecord[2]) == true)
                    {
                        todo.Complete();
                    }
                    
                    return todo;
                }
            }
        }

        throw new Exception($"Unable to find a record with Id {id}");
    }

    public List<Todo> Read()
    {
        var docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        using var sr = new StreamReader(Path.Combine(docPath, "TodosDatabase.txt"), true);
        var lineRead = "";
        var todos = new List<Todo>();
        while (!sr.EndOfStream)
        {
            lineRead = sr.ReadLine();
            if (lineRead is not null)
            {
                var dbRecord = lineRead.Split(";");
                var todo = Todo.Create(Guid.Parse(dbRecord[0]), dbRecord[1]);
                if (bool.Parse(dbRecord[2]) == true)
                {
                    todo.Complete();
                }

                todos.Add(todo);
            }
        }

        return todos;
    }

    public void Store(Todo todo)
    {
        var docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        using var outputFile = new StreamWriter(Path.Combine(docPath, "TodosDatabase.txt"), true);
        outputFile.WriteLine("{0};{1};{2}", todo.Id, todo.Description, todo.Completed);
    }
}

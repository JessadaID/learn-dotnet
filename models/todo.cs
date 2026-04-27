namespace learn_dotnet.models;

public class Todo
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; } = false;
}
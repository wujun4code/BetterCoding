namespace UsageDemo.Entities
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
        public TodoItemStatus Status { get; set; }
    }

    public enum TodoItemStatus : int
    {
        Draft,
        New,
        InProgress,
        Done,
        Deleted,
        Closed,
    }
}

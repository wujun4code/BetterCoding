namespace UsageDemo.Entities
{
    public class JiraTicket
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public JiraTicketStatus Status { get; set; }
    }

    public enum JiraTicketStatus : int
    {
        Draft,
        New,
        InProgress,
        Done,
        Deleted,
        Closed,
    }
}

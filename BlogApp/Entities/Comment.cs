namespace BlogApp.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int PostId { get; set; }
        public int AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

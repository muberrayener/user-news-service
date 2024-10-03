namespace NewsService.NewsService.Core.Entities
{
    public class NewsArticle
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string Content { get; set; }
        public string Date { get; set; }

    }
}

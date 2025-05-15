namespace NewsService.NewsService.Core.Entities
{
    public class NewsArticleEnt : BaseEnt
    {
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string Content { get; set; }

    }
}

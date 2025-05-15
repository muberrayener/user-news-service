namespace NewsService.NewsService.Application.DTOs
{
    public class NewsArticleDto:BaseDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }

    }
}

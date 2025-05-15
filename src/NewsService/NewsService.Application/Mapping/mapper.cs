using AutoMapper;
using NewsService.NewsService.Application.DTOs;
using NewsService.NewsService.Core.Entities;

namespace NewsService.NewsService.Application.Mapping
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<NewsArticleDto, NewsArticleEnt>();
            CreateMap<NewsArticleEnt, NewsArticleDto>();
        }
    }
}

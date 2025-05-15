using AutoMapper;

namespace NewsService.NewsService.Application.Mapping
{
    public class NewsMapper<TProfile> where TProfile : Profile, new()
    {
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() => new MapperConfiguration(delegate (IMapperConfigurationExpression cfg)
        {
            cfg.AllowNullCollections = true;
            cfg.AddProfile(new TProfile());
        }).CreateMapper());

        public static IMapper Mapper => lazy.Value;
    }
}

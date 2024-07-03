using Business.Interfaces;
using Business.Logic.GenreData;
using Business.Logic.MovieData;
using Business.Logic.RatingData;
using Business.Logic.StreamingData;
using Business.Logic.UserData;
using Microsoft.Extensions.DependencyInjection;

namespace Business.DependencyInjection
{
    public class CreateBusinessDependencyInjection
    {
        public static void RegisterDependencyInjection(IServiceCollection services)
        {
            services.AddScoped<IBlUser, BlUser>();
            services.AddScoped<IBlGenre, BlGenre>();
            services.AddScoped<IBlMovie, BlMovie>();
            services.AddScoped<IBlRating, BlRating>();
            services.AddScoped<IBlStreaming, BlStreaming>();
        }
    }
}

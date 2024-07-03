using DAO.Interfaces;
using DAO.Logic.GenreDAO;
using DAO.Logic.MovieDAO;
using DAO.Logic.RatingDAO;
using DAO.Logic.Streaming;
using DAO.Logic.UserDAO;
using Microsoft.Extensions.DependencyInjection;

namespace DAO.DependencyInjection
{
    public class CreateDaoDependencyInjection
    {
        public static void RegisterDependencyInjection(IServiceCollection services)
        {
            services.AddScoped<IUserDAO, UserDAO>();
            services.AddScoped<IGenreDAO, GenreDAO>();
            services.AddScoped<IMovieDAO, MovieDAO>();
            services.AddScoped<IRatingDAO, RatingDAO>();
            services.AddScoped<IStreamingDAO, StreamingDAO>();
        }
    }
}

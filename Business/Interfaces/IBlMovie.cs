using DTO.General.Base.Output;
using DTO.Logic.Movie.Database;
using DTO.Logic.Movie.Input;

namespace Business.Interfaces
{
    public interface IBlMovie
    {
        public Movie GetMovie(string id);
        public BaseListOutput<Movie> List(MovieListInput input);
        public BaseApiOutput CreateMovie(MovieInput movie);
        public BaseApiOutput UpdateMovie(string id, MovieInput movie);
        public BaseApiOutput DeleteMovie(string id);
    }
}

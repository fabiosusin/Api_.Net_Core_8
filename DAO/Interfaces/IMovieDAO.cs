using DAO.Base;
using DTO.General.Base.Output;
using DTO.Logic.Movie.Database;
using DTO.Logic.Movie.Input;

namespace DAO.Interfaces
{
    public interface IMovieDAO : IBaseDAO<Movie>
    {
        public BaseListOutput<Movie> List(MovieListInput input);
    }
}

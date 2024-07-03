using DAO.Base;
using DTO.General.Base.Output;
using DTO.Logic.Genre.Database;
using DTO.Logic.Genre.Input;

namespace DAO.Interfaces
{
    public interface IGenreDAO : IBaseDAO<Genre>
    {
        public Genre FindByName(string name);
        public BaseListOutput<Genre> List(GenreListInput input);
    }
}

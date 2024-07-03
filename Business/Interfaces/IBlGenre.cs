using DTO.General.Base.Output;
using DTO.Logic.Genre.Database;
using DTO.Logic.Genre.Input;

namespace Business.Interfaces
{
    public interface IBlGenre
    {
        public Genre GetGenre(string id);
        public BaseListOutput<Genre> List(GenreListInput input);
        public BaseApiOutput CreateGenre(string name);
        public BaseApiOutput UpdateGenre(string id, string name);
        public BaseApiOutput DeleteGenre(string id);
    }
}

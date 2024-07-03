using Business.Interfaces;
using DAO.Interfaces;
using DTO.General.Base.Output;
using DTO.Logic.Genre.Database;
using DTO.Logic.Genre.Input;
using System.Linq;

namespace Business.Logic.GenreData
{
    public class BlGenre : IBlGenre
    {
        private readonly IGenreDAO _genreDAO;
        private readonly IMovieDAO _movieDAO;
        public BlGenre(
            IMovieDAO movieDAO,
            IGenreDAO genreDAO)
        {
            _genreDAO = genreDAO;
            _movieDAO = movieDAO;
        }

        public BaseApiOutput CreateGenre(string name)
        {
            var existingName = _genreDAO.FindByName(name);
            if (existingName != null)
                return new("Já existe um Gênero com este nome!");

            return _genreDAO.Insert(new(name));
        }

        public BaseApiOutput DeleteGenre(string id)
        {
            var genre = _genreDAO.FindById(id);
            if (genre == null)
                return new("Gênero não encontrado!");

            var movie = _movieDAO.FindOne(x => x.GenreId == id);
            if (movie != null)
                return new("Existem filmes vinculados a este Gênero!");

            return _genreDAO.RemoveById(id);
        }

        public Genre GetGenre(string id) => _genreDAO.FindById(id);

        public BaseListOutput<Genre> List(GenreListInput input)
        {
            var result = _genreDAO.List(input);
            if (!(result?.Items?.Any() ?? false))
                return new("Nenhum Gênero encontrado!");

            return result;
        }

        public BaseApiOutput UpdateGenre(string id, string name)
        {
            var genre = _genreDAO.FindById(id);
            if (genre == null)
                return new("Gênero não encontrado!");

            genre.Name = name;
            return _genreDAO.Update(genre);
        }
    }
}

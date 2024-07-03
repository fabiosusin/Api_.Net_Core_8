using Business.Interfaces;
using DAO.Interfaces;
using DTO.General.Base.Output;
using DTO.Logic.Movie.Database;
using DTO.Logic.Movie.Input;

namespace Business.Logic.MovieData
{
    public class BlMovie : IBlMovie
    {
        private readonly IMovieDAO _movieDAO;
        private readonly IGenreDAO _genreDAO;
        private readonly IRatingDAO _ratingDAO;
        private readonly IStreamingDAO _streamingDAO;
        public BlMovie(
            IMovieDAO movieDAO,
            IGenreDAO genreDAO,
            IRatingDAO ratingDAO,
            IStreamingDAO streamingDAO)
        {
            _movieDAO = movieDAO;
            _genreDAO = genreDAO;
            _ratingDAO = ratingDAO;
            _streamingDAO = streamingDAO;
        }

        public BaseApiOutput CreateMovie(MovieInput input)
        {
            var validationResult = UpsertMovieValidation(input);
            if (!validationResult.Success)
                return validationResult;

            var existingName = _movieDAO.FindOne(x => x.Title == input.Title);
            if (existingName != null)
                return new("Já existe um filme com este nome!");

            return _movieDAO.Insert(new(input));
        }

        public BaseApiOutput DeleteMovie(string id)
        {
            var movie = _movieDAO.FindById(id);
            if (movie == null)
                return new("Filme não encontrado!");

            var rating = _ratingDAO.FindOne(x => x.MovieId == id);
            if (rating != null)
                return new("Existem avaliações vinculados a este filme!");

            return _movieDAO.RemoveById(id);
        }

        public Movie GetMovie(string id) => _movieDAO.FindById(id);

        public BaseListOutput<Movie> List(MovieListInput input)
        {
            var result = _movieDAO.List(input);
            if (!(result?.Items?.Any() ?? false))
                return new("Nenhum Streaming encontrado!");

            return result;
        }

        public BaseApiOutput UpdateMovie(string id, MovieInput input)
        {
            var movie = _movieDAO.FindById(id);
            if (movie == null)
                return new("Streaming não encontrado!");

            var validationResult = UpsertMovieValidation(input);
            if (!validationResult.Success)
                return validationResult;

            movie = new(input)
            {
                Id = id
            };

            return _movieDAO.Update(movie);
        }

        public BaseApiOutput UpsertMovieValidation(MovieInput input)
        {
            if (string.IsNullOrEmpty(input?.Title))
                return new("Informe um nome para o filme!");

            if (string.IsNullOrEmpty(input.GenreId))
                return new("Informe um gênero para o filme!");

            var existingGenre = _genreDAO.FindById(input.GenreId);
            if (existingGenre == null)
                return new("Gênero não encontrado!");

            if (!(input.StreamingsId?.Any() ?? false))
                return new("Informe ao menos um streaming para o filme!");

            foreach (var item in input.StreamingsId)
            {
                var existingStreaming = _streamingDAO.FindById(item);
                if (existingStreaming == null)
                    return new("Streaming não encontrado!");
            }

            return new(true);
        }
    }
}

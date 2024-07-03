using Business.Interfaces;
using DAO.Interfaces;
using DTO.General.Base.Output;
using DTO.Logic.Movie.Database;
using DTO.Logic.Movie.Input;

namespace Business.Logic.MovieData
{
    /// <summary>
    /// Classe de lógica de negócios para operações relacionadas a filmes.
    /// </summary>
    public class BlMovie : IBlMovie
    {
        private readonly IMovieDAO _movieDAO;
        private readonly IGenreDAO _genreDAO;
        private readonly IRatingDAO _ratingDAO;
        private readonly IStreamingDAO _streamingDAO;

        /// <summary>
        /// Construtor da classe BlMovie.
        /// </summary>
        /// <param name="movieDAO">Data Access Object para operações relacionadas a filmes.</param>
        /// <param name="genreDAO">Data Access Object para operações relacionadas a gêneros.</param>
        /// <param name="ratingDAO">Data Access Object para operações relacionadas a avaliações.</param>
        /// <param name="streamingDAO">Data Access Object para operações relacionadas a streamings.</param>
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

        /// <summary>
        /// Cria um novo filme com base nos dados fornecidos.
        /// </summary>
        /// <param name="input">Dados de entrada para criar o filme.</param>
        /// <returns>Resultado da operação de criação do filme.</returns>
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

        /// <summary>
        /// Deleta um filme com base no ID fornecido.
        /// </summary>
        /// <param name="id">ID do filme a ser deletado.</param>
        /// <returns>Resultado da operação de deleção do filme.</returns>
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

        /// <summary>
        /// Obtém os dados de um filme com base no ID fornecido.
        /// </summary>
        /// <param name="id">ID do filme a ser obtido.</param>
        /// <returns>Dados do filme correspondente ao ID.</returns>
        public Movie GetMovie(string id) => _movieDAO.FindById(id);

        /// <summary>
        /// Lista filmes com base nos critérios fornecidos.
        /// </summary>
        /// <param name="input">Dados de entrada para listar os filmes.</param>
        /// <returns>Lista paginada de filmes.</returns>
        public BaseListOutput<Movie> List(MovieListInput input)
        {
            var result = _movieDAO.List(input);
            if (!(result?.Items?.Any() ?? false))
                return new("Nenhum filme encontrado!");

            return result;
        }

        /// <summary>
        /// Atualiza um filme com base no ID e nos novos dados fornecidos.
        /// </summary>
        /// <param name="id">ID do filme a ser atualizado.</param>
        /// <param name="input">Novos dados do filme.</param>
        /// <returns>Resultado da operação de atualização do filme.</returns>
        public BaseApiOutput UpdateMovie(string id, MovieInput input)
        {
            var movie = _movieDAO.FindById(id);
            if (movie == null)
                return new("Filme não encontrado!");

            var validationResult = UpsertMovieValidation(input);
            if (!validationResult.Success)
                return validationResult;

            movie = new(input)
            {
                Id = id
            };

            return _movieDAO.Update(movie);
        }

        /// <summary>
        /// Valida os dados fornecidos para criação ou atualização de um filme.
        /// </summary>
        /// <param name="input">Dados do filme a serem validados.</param>
        /// <returns>Resultado da validação.</returns>
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

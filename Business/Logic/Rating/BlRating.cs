using Business.Interfaces;
using DAO.Interfaces;
using DTO.General.Base.Output;
using DTO.Logic.Rating.Database;
using DTO.Logic.Rating.Input;

namespace Business.Logic.RatingData
{
    public class BlRating : IBlRating
    {
        private readonly IUserDAO _userDAO;
        private readonly IMovieDAO _movieDAO;
        private readonly IRatingDAO _ratingDAO;
        public BlRating(
            IUserDAO userDAO,
            IMovieDAO movieDAO,
            IRatingDAO ratingDAO)
        {
            _userDAO = userDAO;
            _movieDAO = movieDAO;
            _ratingDAO = ratingDAO;
        }

        public BaseApiOutput CreateRating(RatingInput input)
        {
            var validationResult = UpsertRatingValidation(input);
            if (!validationResult.Success)
                return validationResult;

            return _ratingDAO.Insert(new(input));
        }

        public BaseApiOutput DeleteRating(string id)
        {
            var rating = _ratingDAO.FindById(id);
            if (rating == null)
                return new("Avaliação não encontrado!");

            return _ratingDAO.RemoveById(id);
        }

        public Rating GetRating(string id) => _ratingDAO.FindById(id);

        public BaseListOutput<Rating> List(RatingListInput input)
        {
            var result = _ratingDAO.List(input);
            if (!(result?.Items?.Any() ?? false))
                return new("Nenhum Streaming encontrado!");

            return result;
        }

        public BaseApiOutput UpdateRating(string id, RatingInput input)
        {
            var Rating = _ratingDAO.FindById(id);
            if (Rating == null)
                return new("Streaming não encontrado!");

            var validationResult = UpsertRatingValidation(input);
            if (!validationResult.Success)
                return validationResult;

            Rating = new(input)
            {
                Id = id
            };

            return _ratingDAO.Update(Rating);
        }

        public BaseApiOutput UpsertRatingValidation(RatingInput input)
        {
            if (string.IsNullOrEmpty(input?.Comment))
                return new("Informe um comentário para o filme!");

            if (string.IsNullOrEmpty(input.MovieId))
                return new("Informe um filme!");

            if (string.IsNullOrEmpty(input.UserId))
                return new("Informe um usuário!");

            if (input.RatingValue > 5 || input.RatingValue < 0)
                return new("Informe uma nota para o filme entre 0 e 5!");

            var existingMovie = _movieDAO.FindById(input.MovieId);
            if (existingMovie == null)
                return new("Filme não encontrado!");

            var existingUser = _userDAO.FindById(input.UserId);
            if (existingUser == null)
                return new("Usuário não encontrado!");

            return new(true);
        }
    }
}

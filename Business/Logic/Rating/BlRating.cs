using Business.Interfaces;
using DAO.Interfaces;
using DTO.General.Base.Output;
using DTO.Logic.Rating.Database;

using DTO.Logic.Rating.Input;

/// <summary>
/// Business logic implementation for managing ratings of movies.
/// </summary>
public class BlRating : IBlRating
{
    private readonly IUserDAO _userDAO;
    private readonly IMovieDAO _movieDAO;
    private readonly IRatingDAO _ratingDAO;

    /// <summary>
    /// Initializes a new instance of the <see cref="BlRating"/> class.
    /// </summary>
    /// <param name="userDAO">Data Access Object for user operations.</param>
    /// <param name="movieDAO">Data Access Object for movie operations.</param>
    /// <param name="ratingDAO">Data Access Object for rating operations.</param>
    public BlRating(
        IUserDAO userDAO,
        IMovieDAO movieDAO,
        IRatingDAO ratingDAO)
    {
        _userDAO = userDAO;
        _movieDAO = movieDAO;
        _ratingDAO = ratingDAO;
    }

    /// <summary>
    /// Creates a new rating for a movie based on the provided input.
    /// </summary>
    /// <param name="input">Input containing the rating details.</param>
    /// <returns>Operation result indicating success or failure.</returns>
    public BaseApiOutput CreateRating(RatingInput input)
    {
        var validationResult = UpsertRatingValidation(input);
        if (!validationResult.Success)
            return validationResult;

        return _ratingDAO.Insert(new Rating(input));
    }

    /// <summary>
    /// Deletes a rating by its ID.
    /// </summary>
    /// <param name="id">ID of the rating to delete.</param>
    /// <returns>Operation result indicating success or failure.</returns>
    public BaseApiOutput DeleteRating(string id)
    {
        var rating = _ratingDAO.FindById(id);
        if (rating == null)
            return new BaseApiOutput("Avaliação não encontrada!");

        return _ratingDAO.RemoveById(id);
    }

    /// <summary>
    /// Retrieves a rating by its ID.
    /// </summary>
    /// <param name="id">ID of the rating to retrieve.</param>
    /// <returns>The rating object if found, otherwise null.</returns>
    public Rating GetRating(string id) => _ratingDAO.FindById(id);

    /// <summary>
    /// Lists ratings based on the provided input criteria.
    /// </summary>
    /// <param name="input">Input criteria for listing ratings.</param>
    /// <returns>List of ratings matching the criteria.</returns>
    public BaseListOutput<Rating> List(RatingListInput input)
    {
        var result = _ratingDAO.List(input);
        if (!(result?.Items?.Any() ?? false))
            return new BaseListOutput<Rating>("Nenhum streaming encontrado!");

        return result;
    }

    /// <summary>
    /// Updates an existing rating based on the provided ID and input.
    /// </summary>
    /// <param name="id">ID of the rating to update.</param>
    /// <param name="input">Input containing the updated rating details.</param>
    /// <returns>Operation result indicating success or failure.</returns>
    public BaseApiOutput UpdateRating(string id, RatingInput input)
    {
        var Rating = _ratingDAO.FindById(id);
        if (Rating == null)
            return new BaseApiOutput("Streaming não encontrado!");

        var validationResult = UpsertRatingValidation(input);
        if (!validationResult.Success)
            return validationResult;

        Rating = new Rating(input)
        {
            Id = id
        };

        return _ratingDAO.Update(Rating);
    }

    /// <summary>
    /// Performs validation on the provided rating input.
    /// </summary>
    /// <param name="input">Input containing the rating details to validate.</param>
    /// <returns>Operation result indicating validation success or failure.</returns>
    public BaseApiOutput UpsertRatingValidation(RatingInput input)
    {
        if (string.IsNullOrEmpty(input?.Comment))
            return new BaseApiOutput("Informe um comentário para o filme!");

        if (string.IsNullOrEmpty(input.MovieId))
            return new BaseApiOutput("Informe um filme!");

        if (string.IsNullOrEmpty(input.UserId))
            return new BaseApiOutput("Informe um usuário!");

        if (input.RatingValue > 5 || input.RatingValue < 0)
            return new BaseApiOutput("Informe uma nota para o filme entre 0 e 5!");

        var existingMovie = _movieDAO.FindById(input.MovieId);
        if (existingMovie == null)
            return new BaseApiOutput("Filme não encontrado!");

        var existingUser = _userDAO.FindById(input.UserId);
        if (existingUser == null)
            return new BaseApiOutput("Usuário não encontrado!");

        return new BaseApiOutput(true);
    }
}

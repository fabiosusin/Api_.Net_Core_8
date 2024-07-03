using Business.Interfaces;
using Business.Logic.RatingData;
using DAO.Interfaces;
using DTO.General.Base.Output;
using DTO.General.DAO.Output;
using DTO.Logic.Rating.Database;
using DTO.Logic.Rating.Input;
using Moq;

namespace Business.Tests.Logic.RatingData
{
    public class BlRatingTests
    {
        private readonly Mock<IUserDAO> _userDAOMock;
        private readonly Mock<IMovieDAO> _movieDAOMock;
        private readonly Mock<IRatingDAO> _ratingDAOMock;
        private readonly BlRating _blRating;

        public BlRatingTests()
        {
            _userDAOMock = new Mock<IUserDAO>();
            _movieDAOMock = new Mock<IMovieDAO>();
            _ratingDAOMock = new Mock<IRatingDAO>();
            _blRating = new BlRating(_userDAOMock.Object, _movieDAOMock.Object, _ratingDAOMock.Object);
        }

        [Fact]
        public void CreateRating_ShouldReturnError_WhenValidationFails()
        {
            // Arrange
            var input = new RatingInput();
            var validationResult = new DAOActionResultOutput("Informe um comentário para o filme!");
            _ratingDAOMock.Setup(x => x.Insert(It.IsAny<Rating>())).Returns(validationResult);

            // Act
            var result = _blRating.CreateRating(input);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(validationResult.Message, result.Message);
        }

        [Fact]
        public void CreateRating_ShouldReturnSuccess_WhenRatingIsCreated()
        {
            // Arrange
            var input = new RatingInput
            {
                Comment = "ótimo filme!",
                MovieId = "movieId",
                UserId = "userId",
                RatingValue = 4
            };
            _userDAOMock.Setup(x => x.FindById(input.UserId)).Returns(new DTO.Logic.User.Database.User());
            _movieDAOMock.Setup(x => x.FindById(input.MovieId)).Returns(new DTO.Logic.Movie.Database.Movie());
            _ratingDAOMock.Setup(x => x.Insert(It.IsAny<Rating>())).Returns(new DAOActionResultOutput(true));

            // Act
            var result = _blRating.CreateRating(input);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public void DeleteRating_ShouldReturnError_WhenRatingNotFound()
        {
            // Arrange
            _ratingDAOMock.Setup(x => x.FindById(It.IsAny<string>())).Returns((Rating)null);

            // Act
            var result = _blRating.DeleteRating("ratingId");

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Avaliação não encontrado!", result.Message);
        }

        [Fact]
        public void DeleteRating_ShouldReturnSuccess_WhenRatingIsDeleted()
        {
            // Arrange
            var rating = new Rating { Id = "ratingId" };
            _ratingDAOMock.Setup(x => x.FindById(It.IsAny<string>())).Returns(rating);
            _ratingDAOMock.Setup(x => x.RemoveById(It.IsAny<string>())).Returns(new DAOActionResultOutput(true));

            // Act
            var result = _blRating.DeleteRating("ratingId");

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public void GetRating_ShouldReturnRating_WhenRatingFound()
        {
            // Arrange
            var rating = new Rating { Id = "ratingId" };
            _ratingDAOMock.Setup(x => x.FindById(It.IsAny<string>())).Returns(rating);

            // Act
            var result = _blRating.GetRating("ratingId");

            // Assert
            Assert.Equal(rating, result);
        }

        [Fact]
        public void List_ShouldReturnError_WhenNoRatingsFound()
        {
            // Arrange
            _ratingDAOMock.Setup(x => x.List(It.IsAny<RatingListInput>())).Returns((BaseListOutput<Rating>)null);

            // Act
            var result = _blRating.List(new RatingListInput());

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nenhum Streaming encontrado!", result.Message);
        }

        [Fact]
        public void List_ShouldReturnRatings_WhenRatingsFound()
        {
            // Arrange
            var ratings = new List<Rating> { new Rating { Id = "ratingId" } };
            var listResult = new BaseListOutput<Rating>(ratings, 1);
            _ratingDAOMock.Setup(x => x.List(It.IsAny<RatingListInput>())).Returns(listResult);

            // Act
            var result = _blRating.List(new RatingListInput());

            // Assert
            Assert.True(result.Success);
            Assert.Equal(ratings, result.Items);
        }

        [Fact]
        public void UpdateRating_ShouldReturnError_WhenRatingNotFound()
        {
            // Arrange
            _ratingDAOMock.Setup(x => x.FindById(It.IsAny<string>())).Returns((Rating)null);

            // Act
            var result = _blRating.UpdateRating("ratingId", new RatingInput());

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Streaming não encontrado!", result.Message);
        }

        [Fact]
        public void UpdateRating_ShouldReturnSuccess_WhenRatingIsUpdated()
        {
            // Arrange
            var input = new RatingInput
            {
                Comment = "Updated comentário",
                MovieId = "movieId",
                UserId = "userId",
                RatingValue = 4
            };
            var rating = new Rating { Id = "ratingId" };
            _ratingDAOMock.Setup(x => x.FindById(It.IsAny<string>())).Returns(rating);
            _userDAOMock.Setup(x => x.FindById(input.UserId)).Returns(new DTO.Logic.User.Database.User());
            _movieDAOMock.Setup(x => x.FindById(input.MovieId)).Returns(new DTO.Logic.Movie.Database.Movie());
            _ratingDAOMock.Setup(x => x.Update(It.IsAny<Rating>())).Returns(new DAOActionResultOutput(true));

            // Act
            var result = _blRating.UpdateRating("ratingId", input);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public void UpsertRatingValidation_ShouldReturnError_WhenCommentIsNull()
        {
            // Arrange
            var input = new RatingInput { Comment = null };

            var result = _blRating.UpsertRatingValidation(input);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Informe um comentário para o filme!", result.Message);
        }

        [Fact]
        public void UpsertRatingValidation_ShouldReturnError_WhenMovieNotFound()
        {
            // Arrange
            var input = new RatingInput { Comment = "Bom", MovieId = "movieId", UserId = "userId", RatingValue = 4 };
            _movieDAOMock.Setup(x => x.FindById(input.MovieId)).Returns((DTO.Logic.Movie.Database.Movie)null);

            // Act
            var result = _blRating.UpsertRatingValidation(input);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Filme não encontrado!", result.Message);
        }

        [Fact]
        public void UpsertRatingValidation_ShouldReturnError_WhenUserNotFound()
        {
            // Arrange
            var input = new RatingInput { Comment = "Bom", MovieId = "movieId", UserId = "userId", RatingValue = 4 };
            _movieDAOMock.Setup(x => x.FindById(input.MovieId)).Returns(new DTO.Logic.Movie.Database.Movie());
            _userDAOMock.Setup(x => x.FindById(input.UserId)).Returns((DTO.Logic.User.Database.User)null);

            // Act
            var result = _blRating.UpsertRatingValidation(input);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Usuário não encontrado!", result.Message);
        }

        [Fact]
        public void UpsertRatingValidation_ShouldReturnSuccess_WhenValidationPasses()
        {
            // Arrange
            var input = new RatingInput { Comment = "Bom", MovieId = "movieId", UserId = "userId", RatingValue = 4 };
            _movieDAOMock.Setup(x => x.FindById(input.MovieId)).Returns(new DTO.Logic.Movie.Database.Movie());
            _userDAOMock.Setup(x => x.FindById(input.UserId)).Returns(new DTO.Logic.User.Database.User());

            // Act
            var result = _blRating.UpsertRatingValidation(input);

            // Assert
            Assert.True(result.Success);
        }
    }
}

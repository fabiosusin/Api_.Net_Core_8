using Moq;
using DAO.Interfaces;
using DTO.General.Base.Output;
using DTO.Logic.Movie.Database;
using DTO.Logic.Movie.Input;
using Business.Logic.MovieData;
using DTO.Logic.Genre.Database;
using DTO.Logic.Streaming.Database;
using DTO.General.DAO.Output;

namespace UnitTests
{
    public class BlMovieTests
    {
        private readonly Mock<IMovieDAO> _movieDAOMock;
        private readonly Mock<IGenreDAO> _genreDAOMock;
        private readonly Mock<IRatingDAO> _ratingDAOMock;
        private readonly Mock<IStreamingDAO> _streamingDAOMock;
        private readonly BlMovie _blMovie;

        public BlMovieTests()
        {
            _movieDAOMock = new Mock<IMovieDAO>();
            _genreDAOMock = new Mock<IGenreDAO>();
            _ratingDAOMock = new Mock<IRatingDAO>();
            _streamingDAOMock = new Mock<IStreamingDAO>();
            _blMovie = new BlMovie(
                _movieDAOMock.Object,
                _genreDAOMock.Object,
                _ratingDAOMock.Object,
                _streamingDAOMock.Object
            );
        }

        [Fact]
        public void CreateMovie_ShouldReturnError_WhenTitleIsEmpty()
        {
            // Arrange
            var input = new MovieInput { Title = "", GenreId = "1", StreamingsId = new List<string> { "1" } };

            // Act
            var result = _blMovie.CreateMovie(input);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Informe um nome para o filme!", result.Message);
        }

        [Fact]
        public void CreateMovie_ShouldReturnError_WhenGenreNotFound()
        {
            // Arrange
            var input = new MovieInput { Title = "Filme Teste", GenreId = "1", StreamingsId = new List<string> { "1" } };
            _genreDAOMock.Setup(g => g.FindById(It.IsAny<string>())).Returns((Genre)null);

            // Act
            var result = _blMovie.CreateMovie(input);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Gênero não encontrado!", result.Message);
        }

        [Fact]
        public void CreateMovie_ShouldReturnError_WhenStreamingNotFound()
        {
            // Arrange
            var input = new MovieInput { Title = "Filme Teste", GenreId = "1", StreamingsId = new List<string> { "1" } };
            _genreDAOMock.Setup(g => g.FindById(It.IsAny<string>())).Returns(new Genre());
            _streamingDAOMock.Setup(s => s.FindById(It.IsAny<string>())).Returns((Streaming)null);

            // Act
            var result = _blMovie.CreateMovie(input);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Streaming não encontrado!", result.Message);
        }

        [Fact]
        public void CreateMovie_ShouldReturnSuccess_WhenMovieIsValid()
        {
            // Arrange
            var input = new MovieInput { Title = "Filme Teste", GenreId = "1", StreamingsId = new List<string> { "1" } };
            _genreDAOMock.Setup(g => g.FindById(It.IsAny<string>())).Returns(new Genre());
            _streamingDAOMock.Setup(s => s.FindById(It.IsAny<string>())).Returns(new Streaming());
            _movieDAOMock.Setup(m => m.FindOne()).Returns((Movie)null);
            _movieDAOMock.Setup(m => m.Insert(It.IsAny<Movie>())).Returns(new DAOActionResultOutput(true));

            // Act
            var result = _blMovie.CreateMovie(input);

            // Assert
            Assert.True(result.Success);
        }
    }
}

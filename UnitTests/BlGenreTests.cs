using Moq;
using DAO.Interfaces;
using DTO.General.Base.Output;
using DTO.Logic.Genre.Database;
using DTO.Logic.Genre.Input;
using global::Business.Logic.GenreData;
using DTO.General.DAO.Output;
using DTO.Logic.Movie.Database;

namespace UnitTests
{

    namespace Business.Logic.GenreData.Tests
    {
        public class BlGenreTests
        {
            private readonly Mock<IGenreDAO> _genreDAOMock;
            private readonly Mock<IMovieDAO> _movieDAOMock;
            private readonly BlGenre _blGenre;

            public BlGenreTests()
            {
                _genreDAOMock = new Mock<IGenreDAO>();
                _movieDAOMock = new Mock<IMovieDAO>();
                _blGenre = new BlGenre(_movieDAOMock.Object, _genreDAOMock.Object);
            }

            [Fact]
            public void CreateGenre_ShouldReturnErrorMessage_WhenGenreAlreadyExists()
            {
                // Arrange
                string genreName = "Ação";
                _genreDAOMock.Setup(x => x.FindByName(genreName)).Returns(new Genre());

                // Act
                var result = _blGenre.CreateGenre(genreName);

                // Assert
                Assert.Equal("Já existe um Gênero com este nome!", result.Message);
            }

            [Fact]
            public void CreateGenre_ShouldReturnSuccessMessage_WhenGenreIsCreated()
            {
                // Arrange
                string genreName = "Ação";
                _genreDAOMock.Setup(x => x.FindByName(genreName)).Returns((Genre)null);
                _genreDAOMock.Setup(x => x.Insert(It.IsAny<Genre>())).Returns(new DAOActionResultOutput(true));

                // Act
                var result = _blGenre.CreateGenre(genreName);

                // Assert
                Assert.Null(result.Message);
            }

            [Fact]
            public void DeleteGenre_ShouldReturnErrorMessage_WhenGenreNotFound()
            {
                // Arrange
                string genreId = "1";
                _genreDAOMock.Setup(x => x.FindById(genreId)).Returns((Genre)null);

                // Act
                var result = _blGenre.DeleteGenre(genreId);

                // Assert
                Assert.Equal("Gênero não encontrado!", result.Message);
            }

            [Fact]
            public void DeleteGenre_ShouldReturnErrorMessage_WhenMoviesLinkedToGenre()
            {
                // Arrange
                string genreId = "1";
                _genreDAOMock.Setup(x => x.FindById(genreId)).Returns(new Genre());
                _movieDAOMock.Setup(x => x.FindOne(x=> x.GenreId == genreId)).Returns(new Movie());

                // Act
                var result = _blGenre.DeleteGenre(genreId);

                // Assert
                Assert.Equal("Existem filmes vinculados a este Gênero!", result.Message);
            }

            [Fact]
            public void DeleteGenre_ShouldReturnSuccessMessage_WhenGenreIsDeleted()
            {
                // Arrange
                string genreId = "1";
                _genreDAOMock.Setup(x => x.FindById(genreId)).Returns(new Genre());
                _movieDAOMock.Setup(x => x.FindOne()).Returns((Movie)null);
                _genreDAOMock.Setup(x => x.RemoveById(genreId)).Returns(new DAOActionResultOutput(true));

                // Act
                var result = _blGenre.DeleteGenre(genreId);

                // Assert
                Assert.Null(result.Message);
            }

            [Fact]
            public void GetGenre_ShouldReturnGenre_WhenGenreExists()
            {
                // Arrange
                string genreId = "1";
                var expectedGenre = new Genre { Id = genreId, Name = "Ação" };
                _genreDAOMock.Setup(x => x.FindById(genreId)).Returns(expectedGenre);

                // Act
                var result = _blGenre.GetGenre(genreId);

                // Assert
                Assert.Equal(expectedGenre, result);
            }

            [Fact]
            public void List_ShouldReturnErrorMessage_WhenNoGenresFound()
            {
                // Arrange
                var input = new GenreListInput();
                _genreDAOMock.Setup(x => x.List(input)).Returns((BaseListOutput<Genre>)null);

                // Act
                var result = _blGenre.List(input);

                // Assert
                Assert.Equal("Nenhum Gênero encontrado!", result.Message);
            }

            [Fact]
            public void List_ShouldReturnGenres_WhenGenresFound()
            {
                // Arrange
                var input = new GenreListInput();
                var genres = new List<Genre> { new Genre { Id = "1", Name = "Ação" } };
                var expectedOutput = new BaseListOutput<Genre>(genres, 1);
                _genreDAOMock.Setup(x => x.List(input)).Returns(expectedOutput);

                // Act
                var result = _blGenre.List(input);

                // Assert
                Assert.Equal(expectedOutput, result);
            }

            [Fact]
            public void UpdateGenre_ShouldReturnErrorMessage_WhenGenreNotFound()
            {
                // Arrange
                string genreId = "1";
                string genreName = "Ação";
                _genreDAOMock.Setup(x => x.FindById(genreId)).Returns((Genre)null);

                // Act
                var result = _blGenre.UpdateGenre(genreId, genreName);

                // Assert
                Assert.Equal("Gênero não encontrado!", result.Message);
            }

            [Fact]
            public void UpdateGenre_ShouldReturnSuccessMessage_WhenGenreIsUpdated()
            {
                // Arrange
                string genreId = "1";
                string genreName = "Ação";
                var genre = new Genre { Id = genreId, Name = "Drama" };
                _genreDAOMock.Setup(x => x.FindById(genreId)).Returns(genre);
                _genreDAOMock.Setup(x => x.Update(genre)).Returns(new DAOActionResultOutput(true));

                // Act
                var result = _blGenre.UpdateGenre(genreId, genreName);

                // Assert
                Assert.Null(result.Message);
            }
        }
    }

}

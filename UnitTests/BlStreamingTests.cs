using Moq;
using DAO.Interfaces;
using DTO.General.Base.Output;
using DTO.Logic.Streaming.Database;
using DTO.Logic.Streaming.Input;
using Business.Logic.StreamingData;
using DTO.Logic.Movie.Database;
using DTO.General.DAO.Output;

namespace UnitTests
{

    public class BlStreamingTests
    {
        private readonly Mock<IStreamingDAO> _streamingDAOMock;
        private readonly Mock<IMovieDAO> _movieDAOMock;
        private readonly BlStreaming _blStreaming;

        public BlStreamingTests()
        {
            _streamingDAOMock = new Mock<IStreamingDAO>();
            _movieDAOMock = new Mock<IMovieDAO>();
            _blStreaming = new BlStreaming(
                _movieDAOMock.Object,
                _streamingDAOMock.Object
            );
        }

        [Fact]
        public void CreateStreaming_ShouldReturnError_WhenNameExists()
        {
            // Arrange
            var name = "Teste Streaming";
            _streamingDAOMock.Setup(s => s.FindByName(name)).Returns(new Streaming());

            // Act
            var result = _blStreaming.CreateStreaming(name);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Já existe um Streaming com este nome!", result.Message);
        }

        [Fact]
        public void CreateStreaming_ShouldReturnSuccess_WhenNameIsUnique()
        {
            // Arrange
            var name = "Teste Streaming";
            _streamingDAOMock.Setup(s => s.FindByName(name)).Returns((Streaming)null);
            _streamingDAOMock.Setup(s => s.Insert(It.IsAny<Streaming>())).Returns(new DAOActionResultOutput(true));

            // Act
            var result = _blStreaming.CreateStreaming(name);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public void DeleteStreaming_ShouldReturnError_WhenStreamingNotFound()
        {
            // Arrange
            var id = "1";
            _streamingDAOMock.Setup(s => s.FindById(id)).Returns((Streaming)null);

            // Act
            var result = _blStreaming.DeleteStreaming(id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Streaming não encontrado!", result.Message);
        }

        [Fact]
        public void DeleteStreaming_ShouldReturnError_WhenMoviesLinked()
        {
            // Arrange
            var id = "1";
            _streamingDAOMock.Setup(s => s.FindById(id)).Returns(new Streaming());
            _movieDAOMock.Setup(m => m.FindOne(x=> x.StreamingsId.Contains(id))).Returns(new Movie());

            // Act
            var result = _blStreaming.DeleteStreaming(id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Existem filmes vinculados a este Streaming!", result.Message);
        }

        [Fact]
        public void DeleteStreaming_ShouldReturnSuccess_WhenNoMoviesLinked()
        {
            // Arrange
            var id = "1";
            _streamingDAOMock.Setup(s => s.FindById(id)).Returns(new Streaming());
            _movieDAOMock.Setup(m => m.FindOne()).Returns((Movie)null);
            _streamingDAOMock.Setup(s => s.RemoveById(id)).Returns(new DAOActionResultOutput(true));

            // Act
            var result = _blStreaming.DeleteStreaming(id);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public void GetStreaming_ShouldReturnStreaming_WhenFound()
        {
            // Arrange
            var id = "1";
            var streaming = new Streaming { Id = id };
            _streamingDAOMock.Setup(s => s.FindById(id)).Returns(streaming);

            // Act
            var result = _blStreaming.GetStreaming(id);

            // Assert
            Assert.Equal(streaming, result);
        }

        [Fact]
        public void List_ShouldReturnError_WhenNoStreamingFound()
        {
            // Arrange
            var input = new StreamingListInput();
            _streamingDAOMock.Setup(s => s.List(input)).Returns(new BaseListOutput<Streaming>([], 1));

            // Act
            var result = _blStreaming.List(input);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nenhum Streaming encontrado!", result.Message);
        }

        [Fact]
        public void List_ShouldReturnSuccess_WhenStreamingsFound()
        {
            // Arrange
            var input = new StreamingListInput();
            var streamings = new List<Streaming> { new Streaming() };
            var output = new BaseListOutput<Streaming>(streamings, 1);
            _streamingDAOMock.Setup(s => s.List(input)).Returns(output);

            // Act
            var result = _blStreaming.List(input);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(streamings, result.Items);
        }

        [Fact]
        public void UpdateStreaming_ShouldReturnError_WhenStreamingNotFound()
        {
            // Arrange
            var id = "1";
            var name = "Atualizar Streaming";
            _streamingDAOMock.Setup(s => s.FindById(id)).Returns((Streaming)null);

            // Act
            var result = _blStreaming.UpdateStreaming(id, name);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Streaming não encontrado!", result.Message);
        }

        [Fact]
        public void UpdateStreaming_ShouldReturnSuccess_WhenStreamingUpdated()
        {
            // Arrange
            var id = "1";
            var name = "Atualizar Streaming";
            var streaming = new Streaming { Id = id, Name = name };
            _streamingDAOMock.Setup(s => s.FindById(id)).Returns(streaming);
            _streamingDAOMock.Setup(s => s.Update(It.IsAny<Streaming>())).Returns(new DAOActionResultOutput(true));

            // Act
            var result = _blStreaming.UpdateStreaming(id, name);

            // Assert
            Assert.True(result.Success);
        }
    }

}

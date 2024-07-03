using Business.Interfaces;
using Business.Logic.UserData;
using DAO.Interfaces;
using DTO.General.Base.Output;
using DTO.General.DAO.Output;
using DTO.Logic.Rating.Database;
using DTO.Logic.User.Database;
using DTO.Logic.User.Input;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Business.Tests.Logic.UserData
{
    public class BlUserTests
    {
        private readonly Mock<IUserDAO> _userDAOMock;
        private readonly Mock<IBlRating> _blRatingMock;
        private readonly Mock<IRatingDAO> _ratingDAOMock;
        private readonly BlUser _blUser;

        public BlUserTests()
        {
            _userDAOMock = new Mock<IUserDAO>();
            _blRatingMock = new Mock<IBlRating>();
            _ratingDAOMock = new Mock<IRatingDAO>();
            _blUser = new BlUser(_userDAOMock.Object, _blRatingMock.Object, _ratingDAOMock.Object);
        }

        [Fact]
        public void Login_ShouldReturnError_WhenInputIsNull()
        {
            // Act
            var result = _blUser.Login(null);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Dados não informados!", result.Message);
        }

        [Fact]
        public void Login_ShouldReturnError_WhenUserNotFound()
        {
            // Arrange
            var input = new UserInput { Email = "teste@teste.com", Password = "password" };
            _userDAOMock.Setup(x => x.FindOne(x => x.Password == input.Password)).Returns((User)null);

            // Act
            var result = _blUser.Login(input);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Usuário não encontrado!", result.Message);
        }

        [Fact]
        public void Login_ShouldReturnSuccess_WhenUserFound()
        {
            // Arrange
            var input = new UserInput { Email = "teste@teste.com", Password = "password" };
            var user = new User { Email = "teste@teste.com", Password = "password" };
            _userDAOMock.Setup(x => x.FindOne(x => x.Password == input.Password && x.Email == input.Email)).Returns(user);

            // Act
            var result = _blUser.Login(input);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public void CreateUser_ShouldReturnError_WhenEmailIsNull()
        {
            // Arrange
            var user = new UserInput();

            // Act
            var result = _blUser.CreateUser(user);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Email não informado!", result.Message);
        }

        [Fact]
        public void CreateUser_ShouldReturnError_WhenNameIsNull()
        {
            // Arrange
            var user = new UserInput { Email = "teste@teste.com" };

            // Act
            var result = _blUser.CreateUser(user);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nome não informado!", result.Message);
        }

        [Fact]
        public void CreateUser_ShouldReturnError_WhenPasswordIsNull()
        {
            // Arrange
            var user = new UserInput { Email = "teste@teste.com", Name = "Teste" };

            // Act
            var result = _blUser.CreateUser(user);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Senha não informada!", result.Message);
        }

        [Fact]
        public void CreateUser_ShouldReturnError_WhenEmailAlreadyExists()
        {
            // Arrange
            var input = new UserInput { Email = "teste@teste.com", Name = "Teste", Password = "password" };
            var existingUser = new User { Email = "teste@teste.com" };
            _userDAOMock.Setup(x => x.FindOne(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>())).Returns(existingUser);

            // Act
            var result = _blUser.CreateUser(input);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Email se encontra vinculado a outro usuário!", result.Message);
        }

        [Fact]
        public void CreateUser_ShouldReturnSuccess_WhenUserIsCreated()
        {
            // Arrange
            var input = new UserInput { Email = "teste@teste.com", Name = "Teste", Password = "password" };
            _userDAOMock.Setup(x => x.FindOne(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>())).Returns((User)null);
            _userDAOMock.Setup(x => x.Insert(It.IsAny<User>())).Returns(new DAOActionResultOutput(true));

            // Act
            var result = _blUser.CreateUser(input);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public void DeleteUser_ShouldReturnError_WhenUserNotFound()
        {
            // Arrange
            _userDAOMock.Setup(x => x.FindById(It.IsAny<string>())).Returns((User)null);

            // Act
            var result = _blUser.DeleteUser("userId");

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Usuário não encontrado!", result.Message);
        }

        [Fact]
        public void DeleteUser_ShouldDeleteRatings_WhenRatingsExist()
        {
            // Arrange
            var user = new User { Id = "userId" };
            var ratings = new List<Rating> { new Rating { MovieId = "movieId" } };
            _userDAOMock.Setup(x => x.FindById(It.IsAny<string>())).Returns(user);
            _ratingDAOMock.Setup(x => x.Find(x => x.UserId == user.Id)).Returns(ratings);
            _userDAOMock.Setup(x => x.RemoveById("userId")).Returns(new DAOActionResultOutput(true));
            _blRatingMock.Setup(x => x.DeleteRating("userId")).Returns(new BaseApiOutput(true));

            // Act
            var result = _blUser.DeleteUser("userId");

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public void GetUser_ShouldReturnUser_WhenUserFound()
        {
            // Arrange
            var user = new User { Id = "userId" };
            _userDAOMock.Setup(x => x.FindById(It.IsAny<string>())).Returns(user);

            // Act
            var result = _blUser.GetUser("userId");

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public void UpdateUser_ShouldReturnError_WhenEmailIsNull()
        {
            // Arrange
            var user = new UserInput();

            // Act
            var result = _blUser.UpdateUser("userId", user);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Email não informado!", result.Message);
        }

        [Fact]
        public void UpdateUser_ShouldReturnError_WhenNameIsNull()
        {
            // Arrange
            var user = new UserInput { Email = "teste@teste.com" };

            // Act
            var result = _blUser.UpdateUser("userId", user);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Nome não informado!", result.Message);
        }

        [Fact]
        public void UpdateUser_ShouldReturnError_WhenEmailAlreadyExists()
        {
            // Arrange
            var input = new UserInput { Email = "teste@teste.com", Name = "Teste" };
            var existingUser = new User { Id = "anotherUserId", Email = "teste@teste.com" };
            _userDAOMock.Setup(x => x.FindOne(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>())).Returns(existingUser);

            // Act
            var result = _blUser.UpdateUser("userId", input);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Email se encontra vinculado a outro usuário!", result.Message);
        }

        [Fact]
        public void UpdateUser_ShouldReturnSuccess_WhenUserIsUpdated()
        {
            // Arrange
            var input = new UserInput { Email = "teste@teste.com", Name = "Teste" };
            _userDAOMock.Setup(x => x.FindOne(It.IsAny<System.Linq.Expressions.Expression<System.Func<User, bool>>>())).Returns((User)null);
            _userDAOMock.Setup(x => x.Update(It.IsAny<User>())).Returns(new DAOActionResultOutput(true));

            // Act
            var result = _blUser.UpdateUser("userId", input);

            // Assert
            Assert.True(result.Success);
        }
    }
}

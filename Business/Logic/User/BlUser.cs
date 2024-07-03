using Business.Interfaces;
using DAO.Interfaces;
using DTO.General.Base.Output;
using DTO.Logic.User.Database;
using DTO.Logic.User.Input;

namespace Business.Logic.UserData
{
    public class BlUser : IBlUser
    {
        private readonly IUserDAO _userDAO;
        private readonly IBlRating _blRating;
        private readonly IRatingDAO _ratingDAO;
        public BlUser(
            IUserDAO userDAO,
            IBlRating blRating,
            IRatingDAO ratingDAO)
        {
            _userDAO = userDAO;
            _blRating = blRating;
            _ratingDAO = ratingDAO;
        }

        public BaseApiOutput Login(UserInput input)
        {
            if (input == null)
                return new("Dados não informados!");

            var existingUser = _userDAO.FindOne(x => x.Password == input.Password && x.Email == input.Email);
            return existingUser == null ? new("Usuário não encontrado!") : new(true);
        }

        public BaseApiOutput CreateUser(UserInput user)
        {
            if (string.IsNullOrEmpty(user?.Email))
                return new("Email não informado!");

            if (string.IsNullOrEmpty(user.Name))
                return new("Nome não informado!");

            if (string.IsNullOrEmpty(user.Password))
                return new("Senha não informada!");

            var existingUser = _userDAO.FindOne(x => x.Email == user.Email);
            if (existingUser != null)
                return new("Email se encontra vinculado a outro usuário!");

            return _userDAO.Insert(new(user));
        }

        public BaseApiOutput DeleteUser(string id)
        {
            var existingUser = _userDAO.FindById(id);
            if (existingUser == null)
                return new("Usuário não encontrado!");

            var ratings = _ratingDAO.Find(x => x.UserId == id);
            if (ratings?.Any() ?? false)
            {
                foreach (var rating in ratings)
                    _blRating.DeleteRating(id);
            }

            return _userDAO.RemoveById(id);
        }

        public User GetUser(string id) => _userDAO.FindById(id);

        public BaseApiOutput UpdateUser(string id, UserInput user)
        {
            if (string.IsNullOrEmpty(user?.Email))
                return new("Email não informado!");

            if (string.IsNullOrEmpty(user.Name))
                return new("Nome não informado!");

            var existingUser = _userDAO.FindOne(x => x.Id != id && x.Email == user.Email);
            if (existingUser != null)
                return new("Email se encontra vinculado a outro usuário!");

            return _userDAO.Update(new(user)
            {
                Id = id
            });
        }
    }
}

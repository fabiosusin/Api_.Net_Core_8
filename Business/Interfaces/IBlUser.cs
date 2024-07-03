using DTO.General.Base.Output;
using DTO.Logic.User.Database;
using DTO.Logic.User.Input;

namespace Business.Interfaces
{
    public interface IBlUser
    {
        public User GetUser(string id);
        public BaseApiOutput Login(UserInput input);
        public BaseApiOutput CreateUser(UserInput User);
        public BaseApiOutput UpdateUser(string id, UserInput User);
        public BaseApiOutput DeleteUser(string id);
    }
}

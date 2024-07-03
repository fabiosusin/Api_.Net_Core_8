using DTO.General.Base.Database;
using DTO.Logic.User.Input;

namespace DTO.Logic.User.Database
{
    public class User : BaseData
    {
        public User() { }
        public User(UserInput user)
        {
            if (user == null)
                return;

            Name = user.Name;
            Email = user.Email;
            Password = user.Password;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

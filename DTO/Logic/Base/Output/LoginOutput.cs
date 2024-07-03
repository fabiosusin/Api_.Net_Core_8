using DTO.General.Base.Output;
using DTO.Logic.User.Input;

namespace DTO.General.Login.Output
{
    public class LoginOutput : BaseApiOutput
    {
        public LoginOutput(string msg) : base(msg) { }

        public LoginOutput(UserInput input) : base(true)
        {
            if (input == null)
                return;

            Email = input.Email;
        }

        public string Email { get; set; }
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
    }
}

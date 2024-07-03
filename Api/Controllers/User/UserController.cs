using Business.Interfaces;
using DTO.API.Auth;
using DTO.General.Base.Output;
using DTO.General.Login.Output;
using DTO.Logic.Movie.Input;
using DTO.Logic.User.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using XApi.Controllers;
using static MongoDB.Driver.WriteConcern;

namespace Api.Controllers.User
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Usuário"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    public class UserController : BaseController<UserController>
    {
        private readonly IBlUser IBlUser;
        public UserController(ILogger<UserController> logger, IBlUser iBlUser) : base(logger) => IBlUser = iBlUser;

        [HttpGet, Route("{id}")]
        public IActionResult Get(string id) => Ok(IBlUser.GetUser(id));

        [HttpPost, Route("create"), AllowAnonymous]
        public IActionResult Create(UserInput input) => Ok(IBlUser.CreateUser(input));

        [HttpPut, Route("update/{id}")]
        public IActionResult Update(string id, UserInput input) => Ok(IBlUser.UpdateUser(id, input));

        [HttpDelete, Route("{id}")]
        public IActionResult Delete(string id) => Ok(IBlUser.DeleteUser(id));

        [HttpPost, Route("login"), AllowAnonymous]
        public IActionResult TechnicianAppLogin(UserInput input, [FromServices] SigningConfigurations signingConfigurations, [FromServices] TokenConfigurations tokenConfigurations)
        {
            var loginResult = IBlUser.Login(input);
            if (!(loginResult?.Success ?? false))
                return Ok(loginResult);

            var result = new LoginOutput(input);
            if (signingConfigurations == null || tokenConfigurations == null)
                throw new Exception("Não foi possível carregar as configurações de autenticação");

            RenewToken(signingConfigurations, tokenConfigurations, result);
            if (string.IsNullOrEmpty(result.AccessToken))
                throw new Exception("Não foi possível gerar o token!");

            return Ok(result);
        }

        private static void RenewToken(SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations, LoginOutput account)
        {
            if (!(account?.Success ?? false))
                return;

            var claims = new List<Claim> {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new(JwtRegisteredClaimNames.UniqueName, account.Email),
                new(ClaimsTypes.AppUserId, account.Email),
                new(AuthTypeData.UserId, account.Email)
            };

            var creation = DateTime.Now;
            var expiration = creation + TimeSpan.FromSeconds(tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenConfigurations.Issuer,
                Audience = tokenConfigurations.Audience,
                SigningCredentials = signingConfigurations.SigningCredentials,
                Subject = new ClaimsIdentity(new GenericIdentity(account.Email, "Login"), claims),
                NotBefore = creation,
                Expires = expiration
            });

            account.AccessToken = handler.WriteToken(securityToken);
            account.AccessTokenExpiration = expiration;
        }

    }
}

using Business.Interfaces;
using DTO.API.Auth;
using DTO.General.Base.Output;
using DTO.General.Login.Output;
using DTO.Logic.User.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using XApi.Controllers;

namespace Api.Controllers.User
{
    /// <summary>
    /// Controlador responsável por gerenciar operações relacionadas aos usuários.
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "Usuário"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    public class UserController : BaseController<UserController>
    {
        private readonly IBlUser _IBlUser;

        /// <summary>
        /// Construtor do controlador UserController.
        /// </summary>
        /// <param name="logger">Instância do logger para registro de logs.</param>
        /// <param name="iBlUser">Instância da interface de lógica de negócios de usuário.</param>
        public UserController(ILogger<UserController> logger, IBlUser iBlUser) : base(logger)
        {
            _IBlUser = iBlUser;
        }

        /// <summary>
        /// Obtém um usuário pelo seu ID.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        /// <returns>ActionResult contendo o usuário encontrado.</returns>
        [HttpGet, Route("{id}")]
        public IActionResult Get(string id) => Ok(_IBlUser.GetUser(id));

        /// <summary>
        /// Cria um novo usuário no sistema.
        /// </summary>
        /// <param name="input">Dados de entrada para criação do usuário.</param>
        /// <returns>ActionResult indicando o resultado da operação de criação.</returns>
        [HttpPost, Route("create"), AllowAnonymous]
        public IActionResult Create(UserInput input) => Ok(_IBlUser.CreateUser(input));

        /// <summary>
        /// Atualiza os dados de um usuário existente.
        /// </summary>
        /// <param name="id">ID do usuário a ser atualizado.</param>
        /// <param name="input">Dados de entrada para atualização do usuário.</param>
        /// <returns>ActionResult indicando o resultado da operação de atualização.</returns>
        [HttpPut, Route("update/{id}")]
        public IActionResult Update(string id, UserInput input) => Ok(_IBlUser.UpdateUser(id, input));

        /// <summary>
        /// Deleta um usuário do sistema.
        /// </summary>
        /// <param name="id">ID do usuário a ser deletado.</param>
        /// <returns>ActionResult indicando o resultado da operação de exclusão.</returns>
        [HttpDelete, Route("{id}")]
        public IActionResult Delete(string id) => Ok(_IBlUser.DeleteUser(id));

        /// <summary>
        /// Realiza o login de um usuário no sistema.
        /// </summary>
        /// <param name="input">Dados de entrada para o login.</param>
        /// <param name="signingConfigurations">Configurações para assinatura do token.</param>
        /// <param name="tokenConfigurations">Configurações do token de autenticação.</param>
        /// <returns>ActionResult contendo o resultado do login, incluindo o token de acesso.</returns>
        [HttpPost, Route("login"), AllowAnonymous]
        public IActionResult TechnicianAppLogin(UserInput input, [FromServices] SigningConfigurations signingConfigurations, [FromServices] TokenConfigurations tokenConfigurations)
        {
            var loginResult = _IBlUser.Login(input);
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

        /// <summary>
        /// Renova o token de acesso para o usuário autenticado.
        /// </summary>
        /// <param name="signingConfigurations">Configurações para assinatura do token.</param>
        /// <param name="tokenConfigurations">Configurações do token de autenticação.</param>
        /// <param name="account">Dados da conta de usuário para gerar o novo token.</param>
        private static void RenewToken(SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations, LoginOutput account)
        {
            if (!(account?.Success ?? false))
                return;

            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, account.Email),
                new Claim(ClaimTypes.NameIdentifier, account.Email),
                new Claim(AuthTypeData.UserId, account.Email)
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

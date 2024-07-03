using Business.Interfaces;
using DTO.API.Auth;
using DTO.Logic.Genre.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XApi.Controllers;

namespace Server.Controllers.Genre
{
    /// <summary>
    /// Controlador para operações relacionadas a gêneros de filmes.
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "Genre"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    public class GenreController : BaseController<GenreController>
    {
        private readonly IBlGenre IBlGenre;

        /// <summary>
        /// Construtor da classe GenreController.
        /// </summary>
        /// <param name="logger">Instância do logger para logging de eventos.</param>
        /// <param name="iBlGenre">Instância da interface de lógica de negócios para gêneros.</param>
        public GenreController(ILogger<GenreController> logger, IBlGenre iBlGenre) : base(logger) => IBlGenre = iBlGenre;

        /// <summary>
        /// Obtém um gênero de filme com base no ID fornecido.
        /// </summary>
        /// <param name="id">ID do gênero a ser obtido.</param>
        /// <returns>ActionResult contendo o gênero de filme.</returns>
        [HttpGet, Route("{id}")]
        public IActionResult Get(string id) => Ok(IBlGenre.GetGenre(id));

        /// <summary>
        /// Lista os gêneros de filmes com base nos critérios fornecidos.
        /// </summary>
        /// <param name="input">Dados de entrada para listar os gêneros de filmes.</param>
        /// <returns>ActionResult contendo a lista de gêneros de filmes.</returns>
        [HttpPost, Route("list")]
        public IActionResult List(GenreListInput input) => Ok(IBlGenre.List(input));

        /// <summary>
        /// Cria um novo gênero de filme com o nome fornecido.
        /// </summary>
        /// <param name="name">Nome do novo gênero de filme a ser criado.</param>
        /// <returns>ActionResult indicando o resultado da operação de criação.</returns>
        [HttpPost, Route("create")]
        public IActionResult Create(string name) => Ok(IBlGenre.CreateGenre(name));

        /// <summary>
        /// Atualiza um gênero de filme com base no ID e no novo nome fornecido.
        /// </summary>
        /// <param name="id">ID do gênero a ser atualizado.</param>
        /// <param name="name">Novo nome para o gênero de filme.</param>
        /// <returns>ActionResult indicando o resultado da operação de atualização.</returns>
        [HttpPut, Route("update/{id}")]
        public IActionResult Update(string id, string name) => Ok(IBlGenre.UpdateGenre(id, name));

        /// <summary>
        /// Deleta um gênero de filme com base no ID fornecido.
        /// </summary>
        /// <param name="id">ID do gênero de filme a ser deletado.</param>
        /// <returns>ActionResult indicando o resultado da operação de deleção.</returns>
        [HttpDelete, Route("{id}")]
        public IActionResult Delete(string id) => Ok(IBlGenre.DeleteGenre(id));
    }
}

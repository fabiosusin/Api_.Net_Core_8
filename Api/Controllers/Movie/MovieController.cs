using Business.Interfaces;
using DTO.API.Auth;
using DTO.Logic.Movie.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XApi.Controllers;

namespace Server.Controllers.Movie
{
    /// <summary>
    /// Controller responsável por endpoints relacionados a filmes.
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "Movie"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    public class MovieController : BaseController<MovieController>
    {
        private readonly IBlMovie IBlMovie;

        /// <summary>
        /// Construtor do MovieController que recebe uma instância do logger e da interface IBlMovie.
        /// </summary>
        /// <param name="logger">Instância do ILogger para logging.</param>
        /// <param name="iBlMovie">Instância da interface IBlMovie para operações relacionadas a filmes.</param>
        public MovieController(ILogger<MovieController> logger, IBlMovie iBlMovie) : base(logger) => IBlMovie = iBlMovie;

        /// <summary>
        /// Obtém um filme pelo seu ID.
        /// </summary>
        /// <param name="id">ID do filme a ser obtido.</param>
        /// <returns>ActionResult contendo o filme obtido, se encontrado.</returns>
        [HttpGet, Route("{id}")]
        public IActionResult Get(string id) => Ok(IBlMovie.GetMovie(id));

        /// <summary>
        /// Lista filmes de acordo com os critérios especificados.
        /// </summary>
        /// <param name="input">Critérios de listagem de filmes.</param>
        /// <returns>ActionResult contendo a lista de filmes conforme os critérios.</returns>
        [HttpPost, Route("list")]
        public IActionResult List(MovieListInput input) => Ok(IBlMovie.List(input));

        /// <summary>
        /// Cria um novo filme com base nos dados fornecidos.
        /// </summary>
        /// <param name="input">Dados de entrada para criar o filme.</param>
        /// <returns>ActionResult indicando o resultado da criação do filme.</returns>
        [HttpPost, Route("create")]
        public IActionResult Create(MovieInput input) => Ok(IBlMovie.CreateMovie(input));

        /// <summary>
        /// Atualiza um filme existente com base no ID fornecido e nos novos dados.
        /// </summary>
        /// <param name="id">ID do filme a ser atualizado.</param>
        /// <param name="input">Novos dados para atualizar o filme.</param>
        /// <returns>ActionResult indicando o resultado da atualização do filme.</returns>
        [HttpPut, Route("update/{id}")]
        public IActionResult Update(string id, MovieInput input) => Ok(IBlMovie.UpdateMovie(id, input));

        /// <summary>
        /// Deleta um filme pelo seu ID.
        /// </summary>
        /// <param name="id">ID do filme a ser deletado.</param>
        /// <returns>ActionResult indicando o resultado da exclusão do filme.</returns>
        [HttpDelete, Route("{id}")]
        public IActionResult Delete(string id) => Ok(IBlMovie.DeleteMovie(id));
    }
}

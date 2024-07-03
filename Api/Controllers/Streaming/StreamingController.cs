using Business.Interfaces;
using DTO.API.Auth;
using DTO.Logic.Streaming.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XApi.Controllers;

namespace Server.Controllers.Streaming
{
    /// <summary>
    /// Controlador responsável por gerenciar operações relacionadas a Streamings.
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "Streaming")]
    [Authorize(Policies.Bearer)]
    [Authorize(Policies.AppUser)]
    public class StreamingController : BaseController<StreamingController>
    {
        private readonly IBlStreaming _blStreaming;

        /// <summary>
        /// Construtor do controlador StreamingController.
        /// </summary>
        /// <param name="logger">Instância do logger.</param>
        /// <param name="blStreaming">Instância da interface de negócio de Streamings.</param>
        public StreamingController(ILogger<StreamingController> logger, IBlStreaming blStreaming) : base(logger)
        {
            _blStreaming = blStreaming;
        }

        /// <summary>
        /// Obtém um Streaming pelo ID.
        /// </summary>
        /// <param name="id">ID do Streaming a ser obtido.</param>
        /// <returns>ActionResult contendo o Streaming encontrado.</returns>
        [HttpGet, Route("{id}")]
        public IActionResult Get(string id) => Ok(_blStreaming.GetStreaming(id));

        /// <summary>
        /// Lista os Streamings de acordo com os critérios especificados.
        /// </summary>
        /// <param name="input">Objeto de entrada contendo os critérios de listagem.</param>
        /// <returns>ActionResult contendo a lista de Streamings encontrados.</returns>
        [HttpPost, Route("list")]
        public IActionResult List(StreamingListInput input) => Ok(_blStreaming.List(input));

        /// <summary>
        /// Cria um novo Streaming com o nome especificado.
        /// </summary>
        /// <param name="name">Nome do novo Streaming a ser criado.</param>
        /// <returns>ActionResult indicando o resultado da criação.</returns>
        [HttpPost, Route("create")]
        public IActionResult Create(string name) => Ok(_blStreaming.CreateStreaming(name));

        /// <summary>
        /// Atualiza o nome de um Streaming existente pelo ID.
        /// </summary>
        /// <param name="id">ID do Streaming a ser atualizado.</param>
        /// <param name="name">Novo nome para o Streaming.</param>
        /// <returns>ActionResult indicando o resultado da atualização.</returns>
        [HttpPut, Route("update/{id}")]
        public IActionResult Update(string id, string name) => Ok(_blStreaming.UpdateStreaming(id, name));

        /// <summary>
        /// Remove um Streaming pelo ID.
        /// </summary>
        /// <param name="id">ID do Streaming a ser removido.</param>
        /// <returns>ActionResult indicando o resultado da remoção.</returns>
        [HttpDelete, Route("{id}")]
        public IActionResult Delete(string id) => Ok(_blStreaming.DeleteStreaming(id));
    }
}

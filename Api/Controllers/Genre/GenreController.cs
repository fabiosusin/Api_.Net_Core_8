using Business.Interfaces;
using DTO.API.Auth;
using DTO.Logic.Genre.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XApi.Controllers;

namespace Server.Controllers.Genre
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Genre"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    public class GenreController : BaseController<GenreController>
    {
        private readonly IBlGenre IBlGenre;
        public GenreController(ILogger<GenreController> logger, IBlGenre iBlGenre) : base(logger) => IBlGenre = iBlGenre;

        [HttpGet, Route("{id}")]
        public IActionResult Get(string id) => Ok(IBlGenre.GetGenre(id));

        [HttpPost, Route("list")]
        public IActionResult List(GenreListInput input) => Ok(IBlGenre.List(input));

        [HttpPost, Route("create")]
        public IActionResult Create(string name) => Ok(IBlGenre.CreateGenre(name));

        [HttpPut, Route("update/{id}")]
        public IActionResult Update(string id, string name) => Ok(IBlGenre.UpdateGenre(id, name));

        [HttpDelete, Route("{id}")]
        public IActionResult Delete(string id) => Ok(IBlGenre.DeleteGenre(id));
    }
}

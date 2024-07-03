using Business.Interfaces;
using DTO.API.Auth;
using DTO.Logic.Movie.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XApi.Controllers;

namespace Server.Controllers.Movie
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Movie"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    public class MovieController : BaseController<MovieController>
    {
        private readonly IBlMovie IBlMovie;
        public MovieController(ILogger<MovieController> logger, IBlMovie iBlMovie) : base(logger) => IBlMovie = iBlMovie;

        [HttpGet, Route("{id}")]
        public IActionResult Get(string id) => Ok(IBlMovie.GetMovie(id));

        [HttpPost, Route("list")]
        public IActionResult List(MovieListInput input) => Ok(IBlMovie.List(input));

        [HttpPost, Route("create")]
        public IActionResult Create(MovieInput input) => Ok(IBlMovie.CreateMovie(input));

        [HttpPut, Route("update/{id}")]
        public IActionResult Update(string id, MovieInput input) => Ok(IBlMovie.UpdateMovie(id, input));

        [HttpDelete, Route("{id}")]
        public IActionResult Delete(string id) => Ok(IBlMovie.DeleteMovie(id));
    }
}

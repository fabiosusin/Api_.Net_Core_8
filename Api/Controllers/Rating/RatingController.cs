using Business.Interfaces;
using DTO.API.Auth;
using DTO.Logic.Rating.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XApi.Controllers;

namespace Server.Controllers.Rating
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Rating"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    public class RatingController : BaseController<RatingController>
    {
        private readonly IBlRating IBlRating;
        public RatingController(ILogger<RatingController> logger, IBlRating iBlRating) : base(logger) => IBlRating = iBlRating;

        [HttpGet, Route("{id}")]
        public IActionResult Get(string id) => Ok(IBlRating.GetRating(id));

        [HttpPost, Route("list")]
        public IActionResult List(RatingListInput input) => Ok(IBlRating.List(input));

        [HttpPost, Route("create")]
        public IActionResult Create(RatingInput input) => Ok(IBlRating.CreateRating(input));

        [HttpPut, Route("update/{id}")]
        public IActionResult Update(string id, RatingInput input) => Ok(IBlRating.UpdateRating(id, input));

        [HttpDelete, Route("{id}")]
        public IActionResult Delete(string id) => Ok(IBlRating.DeleteRating(id));
    }
}

using Business.Interfaces;
using DTO.API.Auth;
using DTO.Logic.Rating.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XApi.Controllers;

namespace Server.Controllers.Rating
{
    /// <summary>
    /// Controller for managing ratings of movies.
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(GroupName = "Rating"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    public class RatingController : BaseController<RatingController>
    {
        private readonly IBlRating IBlRating;

        /// <summary>
        /// Initializes a new instance of the <see cref="RatingController"/> class.
        /// </summary>
        /// <param name="logger">Logger instance for logging.</param>
        /// <param name="iBlRating">Business logic interface for rating operations.</param>
        public RatingController(ILogger<RatingController> logger, IBlRating iBlRating) : base(logger)
        {
            IBlRating = iBlRating;
        }

        /// <summary>
        /// Retrieves a rating by its ID.
        /// </summary>
        /// <param name="id">ID of the rating to retrieve.</param>
        /// <returns>ActionResult containing the rating if found, otherwise NotFound.</returns>
        [HttpGet, Route("{id}")]
        public IActionResult Get(string id) => Ok(IBlRating.GetRating(id));

        /// <summary>
        /// Lists ratings based on the provided criteria.
        /// </summary>
        /// <param name="input">Input criteria for listing ratings.</param>
        /// <returns>ActionResult containing the list of ratings.</returns>
        [HttpPost, Route("list")]
        public IActionResult List(RatingListInput input) => Ok(IBlRating.List(input));

        /// <summary>
        /// Creates a new rating based on the provided input.
        /// </summary>
        /// <param name="input">Input containing the rating details.</param>
        /// <returns>ActionResult indicating the result of the creation operation.</returns>
        [HttpPost, Route("create")]
        public IActionResult Create(RatingInput input) => Ok(IBlRating.CreateRating(input));

        /// <summary>
        /// Updates an existing rating based on the provided ID and input.
        /// </summary>
        /// <param name="id">ID of the rating to update.</param>
        /// <param name="input">Input containing the updated rating details.</param>
        /// <returns>ActionResult indicating the result of the update operation.</returns>
        [HttpPut, Route("update/{id}")]
        public IActionResult Update(string id, RatingInput input) => Ok(IBlRating.UpdateRating(id, input));

        /// <summary>
        /// Deletes a rating by its ID.
        /// </summary>
        /// <param name="id">ID of the rating to delete.</param>
        /// <returns>ActionResult indicating the result of the deletion operation.</returns>
        [HttpDelete, Route("{id}")]
        public IActionResult Delete(string id) => Ok(IBlRating.DeleteRating(id));
    }
}

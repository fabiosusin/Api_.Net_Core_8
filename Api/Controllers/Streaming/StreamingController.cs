using Business.Interfaces;
using DTO.API.Auth;
using DTO.Logic.Streaming.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XApi.Controllers;

namespace Server.Controllers.Streaming
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Streaming"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    public class StreamingController : BaseController<StreamingController>
    {
        private readonly IBlStreaming IBlStreaming;
        public StreamingController(ILogger<StreamingController> logger, IBlStreaming iBlStreaming) : base(logger) => IBlStreaming = iBlStreaming;

        [HttpGet, Route("{id}")]
        public IActionResult Get(string id) => Ok(IBlStreaming.GetStreaming(id));

        [HttpPost, Route("list")]
        public IActionResult List(StreamingListInput input) => Ok(IBlStreaming.List(input));

        [HttpPost, Route("create")]
        public IActionResult Create(string name) => Ok(IBlStreaming.CreateStreaming(name));

        [HttpPut, Route("update/{id}")]
        public IActionResult Update(string id, string name) => Ok(IBlStreaming.UpdateStreaming(id, name));

        [HttpDelete, Route("{id}")]
        public IActionResult Delete(string id) => Ok(IBlStreaming.DeleteStreaming(id));
    }
}

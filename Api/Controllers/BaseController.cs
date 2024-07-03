using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace XApi.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class BaseController<T> : ControllerBase
    {
        private readonly ILogger<T> _logger;
        protected static string ApiVersion => Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        public BaseController(ILogger<T> logger)
        {
            _logger = logger;
        }
    }
}

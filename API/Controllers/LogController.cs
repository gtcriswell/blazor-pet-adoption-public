using DTO.Client;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : Controller
    {
        private readonly ILogger<UserController> _logger;

        public LogController(ILogger<UserController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("error")]
        public IActionResult LogError([FromBody] ClientErrorDto error)
        {

            _logger.LogError(error.Message, error.StackTrace);
            return Ok();
        }
    }
}

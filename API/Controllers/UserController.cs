using API.Business;
using Data.Models;
using DTO.API.Animals;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;
        private readonly ILogger<AdoptController> _logger;

        public UserController(IUserBusiness userBusiness, ILogger<AdoptController> logger)
        {
            _userBusiness = userBusiness ?? throw new ArgumentNullException(nameof(userBusiness));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("AddVisitor")]
        public async Task<IActionResult> AddAddVistorAsync([FromQuery] string email = "", CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("email is required.");
            }


            try
            {
                Visitor visitor = new()
                {
                    CreatedDate = DateTime.UtcNow,
                    Email = email
                };

                var results = await _userBusiness.AddVistorAsync(visitor);

                return results == null ? Ok(new Visitor()) : Ok(results);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("AddAddVistorAsync cancelled by client.");
                return BadRequest("Request cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in AddAddVistorAsync.");
                return Problem(detail: "An error occurred while adding vistor.", statusCode: 500);
            }
        }

        [HttpPost("AddTracker")]
        public async Task<IActionResult> AddTracker(Tracker tracker, CancellationToken cancellationToken = default)
        {


            try
            {

                var results = await _userBusiness.AddTrackerAsync(tracker);

                return results == null ? Ok(new Visitor()) : Ok(results);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("AddTrackerAsync cancelled by client.");
                return BadRequest("Request cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in AddTrackerAsync.");
                return Problem(detail: "An error occurred while adding tracker.", statusCode: 500);
            }
        }

    }
}

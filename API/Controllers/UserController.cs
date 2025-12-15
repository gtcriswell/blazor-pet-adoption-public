using API.Business;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserBusiness userBusiness, ILogger<UserController> logger)
        {
            _userBusiness = userBusiness ?? throw new ArgumentNullException(nameof(userBusiness));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("AddVisitor")]
        public async Task<IActionResult> AddVisitorAsync([FromBody] Visitor visitor, CancellationToken cancellationToken = default)
        {
            if (visitor == null || string.IsNullOrWhiteSpace(visitor.Email))
            {
                return BadRequest("Email is required.");
            }

            try
            {
                // Optionally set CreatedDate if needed
                visitor.CreatedDate = DateTime.UtcNow;

                Visitor? result = await _userBusiness.AddVistorAsync(visitor);

                // Return the created visitor or an empty visitor if null
                return Ok(result ?? new Visitor());
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("AddVisitorAsync cancelled by client.");
                return BadRequest("Request cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in AddVisitorAsync.");
                return Problem(detail: "An error occurred while adding visitor.", statusCode: 500);
            }
        }


        [HttpPost("AddTracker")]
        public async Task<IActionResult> AddTracker(Tracker tracker, CancellationToken cancellationToken = default)
        {
            try
            {
                Tracker results = await _userBusiness.AddTrackerAsync(tracker);

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

using API.Business;
using DTO.API.Animals;
using DTO.API.Orgs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Shared;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AdoptController : ControllerBase
    {
        private readonly IAdoptBusiness _adoptBusiness;
        private readonly ILogger<AdoptController> _logger;
        private readonly IMemoryCache _cache;

        public AdoptController(IAdoptBusiness adoptBusiness, ILogger<AdoptController> logger, IMemoryCache cache)
        {
            _adoptBusiness = adoptBusiness ?? throw new ArgumentNullException(nameof(adoptBusiness));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cache = cache;
        }

        [HttpGet("SearchPets")]
        public async Task<IActionResult> SearchPetsAsync(
            [FromQuery] string species = "dogs",
            [FromQuery] int miles = 25,
            [FromQuery] string zipCode = "",
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(zipCode))
            {
                return BadRequest("zipCode is required.");
            }

            if (miles <= 0)
            {
                return BadRequest("miles must be greater than zero.");
            }

            try
            {
                string key = CacheHelper.GetKey(CacheHelper.animals, species, miles, zipCode);
                List<AnimalsStandard>? animalsFromCache = _cache.Get<List<AnimalsStandard>>(key);

                if (animalsFromCache != null && animalsFromCache.Count > 0)
                {
                    return Ok(animalsFromCache);
                }

                List<AnimalsStandard>? results = await _adoptBusiness.GetAnimalsAsync(species, miles, zipCode, cancellationToken);
                _ = _cache.Set<List<AnimalsStandard>>(key, results.OrderBy(x => x.MilesAway).ToList(), TimeSpan.FromMinutes(30));

                return results == null || !results.Any() ? Ok(new List<AnimalsStandard>()) : Ok(results);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("SearchPetsAsync cancelled by client.");
                return BadRequest("Request cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in SearchPetsAsync.");
                return Problem(detail: "An error occurred while searching for pets.", statusCode: 500);
            }
        }

        [HttpGet("SearchOrgs")]
        public async Task<IActionResult> SearchOrgsAsync(
            [FromQuery] int miles = 25,
            [FromQuery] string zipCode = "28080",
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(zipCode))
            {
                return BadRequest("zipCode is required.");
            }

            if (miles <= 0)
            {
                return BadRequest("miles must be greater than zero.");
            }

            try
            {
                string key = CacheHelper.GetKey(CacheHelper.orgs, miles, zipCode);
                List<OrgStandard>? orgsFromCache = _cache.Get<List<OrgStandard>>(key);

                if (orgsFromCache != null && orgsFromCache.Count > 0)
                {
                    return Ok(orgsFromCache);
                }

                List<OrgStandard>? results = await _adoptBusiness.GetOrgsAsync(miles, zipCode, cancellationToken);

                // Fix for CS8604: Ensure results is not null before calling OrderBy
                List<OrgStandard> orderedResults = (results ?? []).OrderBy(x => x.distance).ToList();
                _ = _cache.Set<List<OrgStandard>>(key, orderedResults, TimeSpan.FromMinutes(30));

                // Fix for IDE0305: Use collection initializer for empty list
                return results == null || !results.Any() ? Ok(new List<DTO.API.Orgs.Attributes>()) : Ok(results);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("SearchPetsAsync cancelled by client.");
                return BadRequest("Request cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in SearchPetsAsync.");
                return Problem(detail: "An error occurred while searching for pets.", statusCode: 500);
            }
        }
    }
}

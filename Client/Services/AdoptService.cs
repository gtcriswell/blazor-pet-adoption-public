using DTO.API;
using DTO.API.Animals;
using DTO.API.Orgs;
using Shared;
using System.Net.Http.Json;

namespace Client.Services
{
    #region interface

    public interface IAdoptService
    {
        Task<List<AnimalsStandard>> GetAnimals(SearchParams animalSearchDto, CancellationToken cancellationToken = default);
        Task<List<OrgStandard>> GetOrgs(SearchParams animalSearchDto, CancellationToken cancellationToken = default);

    }

    #endregion

    public class AdoptService : IAdoptService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly System.Text.Json.JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };

        public AdoptService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<AnimalsStandard>> GetAnimals(SearchParams animalSearchDto, CancellationToken cancellationToken = default)
        {
            HttpClient client = _httpClientFactory.CreateClient("API");

            string url = Utilities.BuildUrl("api/adopt/searchpets", new Dictionary<object, object>
            {
                { "species", animalSearchDto.data.filterRadius.species.ToTrimmedString() },
                { "miles", animalSearchDto.data.filterRadius.miles },
                { "zipCode", animalSearchDto.data.filterRadius.postalcode },
            });
            try
            {
                HttpResponseMessage response = await client.GetAsync(url, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    List<AnimalsStandard>? animals = await response.Content.ReadFromJsonAsync<List<AnimalsStandard>>(_jsonOptions, cancellationToken).ConfigureAwait(false);
                    return animals ?? [];
                }

                return [];
            }
            catch (OperationCanceledException)
            {
                // Propagate cancellation
                throw;
            }
            catch (Exception)
            {
                // Log exception if logging is available; return empty list to preserve existing behavior
                return [];
            }
        }

        public async Task<List<OrgStandard>> GetOrgs(SearchParams animalSearchDto, CancellationToken cancellationToken = default)
        {
            HttpClient client = _httpClientFactory.CreateClient("API");

            string url = Utilities.BuildUrl("api/adopt/searchorgs", new Dictionary<object, object>
            {
                { "miles", animalSearchDto.data.filterRadius.miles },
                { "zipCode", animalSearchDto.data.filterRadius.postalcode },
            });
            try
            {
                HttpResponseMessage response = await client.GetAsync(url, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    List<OrgStandard>? orgSearch = await response.Content.ReadFromJsonAsync<List<OrgStandard>>(_jsonOptions, cancellationToken).ConfigureAwait(false);
                    return orgSearch ?? [];
                }

                return [];
            }
            catch (OperationCanceledException)
            {
                // Propagate cancellation
                throw;
            }
            catch (Exception)
            {
                // Log exception if logging is available; return empty list to preserve existing behavior
                return [];
            }
        }
    }
}

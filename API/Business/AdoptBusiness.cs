using DTO.API;
using DTO.API.Animals;
using DTO.API.Orgs;
using Mapster;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Business
{
    #region interface

    public interface IAdoptBusiness
    {
        Task<List<AnimalsStandard>?> GetAnimalsAsync(string species = "dogs", int miles = 25, string zipCode = "", CancellationToken cancellationToken = default);
        Task<List<OrgStandard>?> GetOrgsAsync(int miles = 25, string zipCode = "", CancellationToken cancellationToken = default);
    }

    #endregion

    public class AdoptBusiness : IAdoptBusiness
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _apiSettings;
        private readonly ILogger<AdoptBusiness> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public AdoptBusiness(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> options, ILogger<AdoptBusiness> logger)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _apiSettings = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }

        public async Task<List<AnimalsStandard>?> GetAnimalsAsync(string species = "dogs", int miles = 25, string zipCode = "", CancellationToken cancellationToken = default)
        {
            // Build request URL from settings (settings may be a format string)
            string url = _apiSettings.SearchApi.Replace("{species}", species);

            List<AnimalsStandard> animalsStandards = [];

            SearchParams searchParams = new()
            {
                data = new()
                {
                    filterRadius = new()
                    {
                        miles = miles,
                        postalcode = zipCode
                    }
                }
            };

            HttpClient client = _httpClientFactory.CreateClient("AdoptClient");
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            client.DefaultRequestHeaders.Add("Authorization", _apiSettings.ApiKey);

            using HttpRequestMessage request = new(HttpMethod.Post, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(searchParams, _jsonOptions), Encoding.UTF8, "application/vnd.api+json")
            };

            try
            {
                using HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Adopt API returned non-success: {StatusCode} {Reason}", response.StatusCode, response.ReasonPhrase);
                    return [];
                }

                await using Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                DTO.API.Animals.Root? animals = await JsonSerializer.DeserializeAsync<DTO.API.Animals.Root?>(stream, _jsonOptions, cancellationToken);

                if (animals != null && animals.data != null && animals.data.Any())
                {
                    foreach (DTO.API.Animals.Datum animal in animals.data)
                    {
                        string? locationId = animal.relationships.orgs.data.FirstOrDefault()?.id;
                        string? breedId = animal.relationships.breeds.data.FirstOrDefault()?.id;
                        string? pictureId = animal.relationships.pictures.data.FirstOrDefault()?.id;

                        Included? include = null;
                        Included? breed = null;
                        Included? picture = null;

                        if (locationId != null)
                        {
                            include = animals.included.FirstOrDefault(i => i.type == "orgs" && i.id == locationId);
                            breed = animals.included.FirstOrDefault(i => i.type == "breeds" && i.id == breedId);
                            picture = animals.included.FirstOrDefault(i => i.type == "pictures" && i.id == pictureId);
                        }

                        string?[] parts = new[]
                        {
                            include?.attributes.street,
                            include?.attributes.city,
                            include?.attributes.state
                        };

                        string address = string.Join(", ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
                        AnimalsStandard animalStandard = new()
                        {
                            Id = animal.id,
                            LocationName = include?.attributes.name ?? "Unknown",
                            Address = address,
                            WebsiteUrl = include?.attributes.adoptionUrl ?? "",
                            FacebookUrl = include?.attributes.facebookUrl ?? "",
                            Breed = breed?.attributes.name ?? "Unknown",
                            PhoneNumber = include?.attributes.phone ?? "",
                            MilesAway = animal.attributes.distance,
                            PhotoUrlLarge = picture?.attributes.large.url ?? "",
                            PhotoUrlSmall = picture?.attributes.small.url ?? "",
                        };

                        animalsStandards.Add(animalStandard);
                    }

                }

                return animalsStandards;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Adopt API request was cancelled.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Adopt API.");
                return null;
            }
        }

        public async Task<List<OrgStandard>?> GetOrgsAsync(int miles = 25, string zipCode = "", CancellationToken cancellationToken = default)
        {
            // Build request URL from settings (settings may be a format string)
            string url = _apiSettings.OrgApi;

            List<OrgStandard> orgSearches = [];

            SearchParams searchParams = new()
            {
                data = new()
                {
                    filterRadius = new()
                    {
                        miles = miles,
                        postalcode = zipCode
                    }
                }
            };

            HttpClient client = _httpClientFactory.CreateClient("AdoptClient");
            client.BaseAddress = new Uri(_apiSettings.BaseUrl);
            client.DefaultRequestHeaders.Add("Authorization", _apiSettings.ApiKey);

            using HttpRequestMessage request = new(HttpMethod.Get, url)
            {
                Content = new StringContent(JsonSerializer.Serialize(searchParams, _jsonOptions), Encoding.UTF8, "application/vnd.api+json")
            };

            try
            {
                using HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Adopt API returned non-success: {StatusCode} {Reason}", response.StatusCode, response.ReasonPhrase);
                    return [];
                }

                await using Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                DTO.API.Orgs.Root? orgSearchResult = await JsonSerializer.DeserializeAsync<DTO.API.Orgs.Root?>(stream, _jsonOptions, cancellationToken);
                if (orgSearchResult != null && orgSearchResult.data.Any())
                {
                    foreach (DTO.API.Orgs.Datum data in orgSearchResult.data)
                    {
                        orgSearches.Add(data.attributes.Adapt<OrgStandard>());

                    }
                }

                return orgSearches;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Adopt API request was cancelled.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Adopt API.");
                return null;
            }
        }


    }
}

using DTO.User;
using Shared;
using System.Net.Http.Json;

namespace Client.Services
{
    #region interface

    public interface IUserService
    {
        Task<VisitorDto> AddUser(string email, CancellationToken cancellationToken = default);
    }

    #endregion

    public class UserService : IUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly System.Text.Json.JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };

        public UserService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<VisitorDto> AddUser(string email, CancellationToken cancellationToken = default)
        {
            HttpClient client = _httpClientFactory.CreateClient("API");

            string url = Utilities.BuildUrl("api/user/addvisitor", new Dictionary<object, object>
            {
                { "email", email},
            });

            try
            {
                HttpResponseMessage response = await client.GetAsync(url, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    VisitorDto? visitorDto = await response.Content.ReadFromJsonAsync<VisitorDto>(_jsonOptions, cancellationToken).ConfigureAwait(false);
                    return visitorDto ?? new();
                }

                return new();
            }
            catch (OperationCanceledException)
            {
                // Propagate cancellation
                throw;
            }
            catch (Exception)
            {
                // Log exception if logging is available; return empty list to preserve existing behavior
                return new();
            }
        }
    }
}

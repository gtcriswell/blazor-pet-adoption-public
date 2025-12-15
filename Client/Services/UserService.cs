using DTO.User;
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
            string url = client.BaseAddress + "api/User/AddVisitor";

            var payload = new VisitorDto()
            {
                Email = email,
                CreatedDate = DateTime.UtcNow
            };

            try
            {
                // Pass the object, not a serialized string
                HttpResponseMessage response = await client.PostAsJsonAsync(url, payload, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    VisitorDto? visitorDto = await response.Content.ReadFromJsonAsync<VisitorDto>(_jsonOptions, cancellationToken).ConfigureAwait(false);

                    return visitorDto ?? new VisitorDto();
                }

                return new VisitorDto();
            }
            catch (OperationCanceledException)
            {
                // Propagate cancellation
                throw;
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                return new VisitorDto();
            }
        }

    }
}

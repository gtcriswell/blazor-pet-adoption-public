using DTO.Client;
using DTO.User;
using System.Net.Http.Json;

namespace Client.Services
{
    #region interface

    public interface ILogService
    {
        Task LogError(ClientErrorDto clientError, CancellationToken cancellationToken = default);
    }

    #endregion

    public class LogService : ILogService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly System.Text.Json.JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };

        public LogService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task LogError(ClientErrorDto clientError, CancellationToken cancellationToken = default)
        {
            HttpClient client = _httpClientFactory.CreateClient("API");
            string url = client.BaseAddress + "api/log/error";

            try
            {
                // Pass the object, not a serialized string
                HttpResponseMessage response = await client.PostAsJsonAsync(url, clientError, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    await response.Content.ReadFromJsonAsync<VisitorDto>(_jsonOptions, cancellationToken).ConfigureAwait(false);
 
                }
                 
            }
            catch (OperationCanceledException)
            {
                // Propagate cancellation
                throw;
            }
            catch (Exception ex)
            {
                // Optionally log the exception 
            }
        }

    }
}

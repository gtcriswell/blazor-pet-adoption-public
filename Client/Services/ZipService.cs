using Microsoft.JSInterop;

namespace Client.Services
{


    public class ZipService
    {
        private readonly IJSRuntime _js;

        public ZipService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<string> GetZipAsync()
        {
            try
            {
                return await _js.InvokeAsync<string>("zipLookup.getZipCode");
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}


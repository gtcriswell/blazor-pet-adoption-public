using DTO.Client;
using Microsoft.AspNetCore.Components;

namespace Client.Base
{

    public class AdoptBase : ComponentBase, IDisposable
    {
        private CancellationTokenSource? cancellationTokenSource;

        public AdoptBase() { }

        [Inject]
        public NavigationManager Nav { get; set; } = default!;

        [Inject]
        public Client.Services.IAdoptService adoptService { get; set; } = default!;

        [Inject]
        public ClientSettings clientSettings { get; set; } = default!;

        public virtual void Dispose()
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }
        }

    }

}
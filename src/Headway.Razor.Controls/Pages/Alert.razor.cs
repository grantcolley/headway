using Microsoft.AspNetCore.Components;

namespace Headway.Razor.Controls.Pages
{
    public partial class Alert : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string AlertType { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Message { get; set; }

        [Parameter]
        public string RedirectText { get; set; }

        [Parameter]
        public string RedirectPage { get; set; }

        public string Class { get; set; }

        protected override void OnInitialized()
        {
            Class = $"alert alert-{AlertType} mt-4";

            base.OnInitialized();
        }

        private void Redirect()
        {
            NavigationManager.NavigateTo($@"{RedirectPage}");
        }
    }
}

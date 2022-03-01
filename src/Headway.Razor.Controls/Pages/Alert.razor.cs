using Headway.Core.Constants;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Headway.Razor.Controls.Pages
{
    public partial class Alert : ComponentBase
    {
        [Parameter]
        public string AlertType { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Message { get; set; }

        protected Severity severity;

        protected override void OnInitialized()
        {
            severity = AlertType switch
            {
                Alerts.NORMAL => Severity.Normal,
                Alerts.INFO => Severity.Info,
                Alerts.SUCCESS => Severity.Success,
                Alerts.WARNING => Severity.Warning,
                Alerts.ERROR => Severity.Error,
                _ => Severity.Normal,
            };

            base.OnInitialized();
        }
    }
}

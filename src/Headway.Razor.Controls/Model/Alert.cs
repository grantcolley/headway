using Headway.Core.Constants;

namespace Headway.Razor.Controls.Model
{
    public class Alert
    {
        public string AlertType { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public string Page
        {
            get
            {
                return $"/{Alerts.ALERT_PAGE}/{AlertType ?? string.Empty}/{Title ?? string.Empty}/{Message ?? string.Empty}";
            }
        }
    }
}

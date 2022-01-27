using Headway.Core.Constants;

namespace Headway.Razor.Controls.Model
{
    public class Alert
    {
        public string AlertType { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string RedirectText { get; set; }
        public string RedirectPage { get; set; }
        public string Page
        {
            get
            {
                var path = $"/{Alerts.ALERT}/{AlertType ?? string.Empty}/{Title ?? string.Empty}/{Message ?? string.Empty}";

                if(!string.IsNullOrEmpty(RedirectText)
                    && !string.IsNullOrEmpty(RedirectPage))
                {
                    path += $"/{ RedirectText ?? string.Empty}/{ RedirectPage ?? string.Empty}";
                }

                return path;
            }
        }
    }
}

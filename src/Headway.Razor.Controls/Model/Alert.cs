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
                return $"/alert/{AlertType ?? string.Empty}/{Title ?? string.Empty}/{Message ?? string.Empty}/{RedirectText ?? string.Empty}/{RedirectPage ?? string.Empty}";
            }
        }
    }
}

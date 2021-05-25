namespace Headway.RazorShared.Model
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
                return $"/alert/{AlertType ?? string.Empty}/{Title ?? string.Empty}/{Message ?? string.Empty}";
            }
        }
    }
}

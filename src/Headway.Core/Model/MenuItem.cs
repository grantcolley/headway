namespace Headway.Core.Model
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string ImageClass { get; set; }
        public string Path { get; set; }
        public string Permission { get; set; }
        public Category Category { get; set; }
    }
}
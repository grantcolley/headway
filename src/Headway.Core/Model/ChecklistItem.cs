using System.Collections.Generic;

namespace Headway.Core.Model
{
    public class ChecklistItem
    {
        public ChecklistItem()
        {
            SubItems = new List<string>();
        }

        public bool IsChecked { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> SubItems { get; set; }
    }
}
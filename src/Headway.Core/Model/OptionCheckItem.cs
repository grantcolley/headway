namespace Headway.Core.Model
{
    public class OptionCheckItem
    {
        public bool IsChecked { get; set; }
        public string Id { get; set; }
        public string Display { get; set; }

        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }

            var optionItem = obj as OptionCheckItem;
            return optionItem?.Id == this.Id;
        }

        public override int GetHashCode()
        {
            if(string.IsNullOrWhiteSpace(Id))
            {
                return "empty".GetHashCode();
            }

            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Display;
        }
    }
}

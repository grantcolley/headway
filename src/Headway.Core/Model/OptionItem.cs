namespace Headway.Core.Model
{
    public class OptionItem
    {
        public string Id { get; set; }
        public string Display { get; set; }

        public override bool Equals(object obj)
        {
            var optionItem = obj as OptionItem;
            return optionItem.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Display;
        }
    }
}

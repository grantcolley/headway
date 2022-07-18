namespace Headway.Razor.Controls.Model
{
    public class OptionItemEnum<T>
    {
        public T Id { get; set; }
        public string Display { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var optionItem = obj as OptionItemEnum<T>;
            return optionItem.Display == this.Display;
        }

        public override int GetHashCode()
        {
            return Display.GetHashCode();
        }

        public override string ToString()
        {
            return Display;
        }
    }
}

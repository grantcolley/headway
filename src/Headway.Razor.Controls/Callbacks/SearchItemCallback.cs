using System;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Callbacks
{
    public class SearchItemCallback
    {
        public Func<Task> OnKeyDown { get; set; }
    }
}
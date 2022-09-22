using System;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Callbacks
{
    public class SearchItemCallback
    {
        public Func<Task> OnKeyDown { get; set; }
    }
}
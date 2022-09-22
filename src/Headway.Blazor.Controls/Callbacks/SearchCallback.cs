using System;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Callbacks
{
    public class SearchCallback
    {
        public Func<Task> Click { get; set; }
    }
}
using System;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Callbacks
{
    public class SearchCallback
    {
        public Func<Task> Click { get; set; }
    }
}
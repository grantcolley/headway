using System;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Callbacks
{
    public class ClickCallback
    {
        public Func<Task> Click { get; set; }
    }
}

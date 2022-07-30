using System;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Model
{
    public class SearchCallback
    {
        public Func<Task> Seach { get; set; }
    }
}

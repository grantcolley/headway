using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;

namespace Headway.Razor.Controls.Base
{
    public abstract class DynamicContainerBase<T> : DynamicModelContainerBase<T> where T : class, new()
    {
        [Parameter]
        public DynamicContainer Container { get; set; }
    }
}

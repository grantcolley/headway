using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Helpers;
using Headway.Blazor.Controls.Base;
using System;
using System.Threading.Tasks;
using Headway.Core.Extensions;

namespace Headway.Blazor.Controls.Containers
{
    [DynamicContainer]
    public abstract class DivBase : DynamicContainerBase
    {
        protected bool LayoutHorizontal { get; private set; }

        protected override Task OnInitializedAsync()
        {
            var layoutHorizontal = Container.DynamicArgs.FirstDynamicArgValueToStringOrDefault(Args.LAYOUT_HORIZONTAL);

            if(!string.IsNullOrWhiteSpace(layoutHorizontal))
            {
                LayoutHorizontal = Convert.ToBoolean(layoutHorizontal);
            }

            return base.OnInitializedAsync();
        }
    }
}

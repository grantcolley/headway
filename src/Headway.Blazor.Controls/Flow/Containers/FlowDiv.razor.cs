using Headway.Blazor.Controls.Base;
using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Extensions;
using System;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Flow.Containers
{
    [DynamicContainer]
    public abstract class FlowDivBase : DynamicContainerBase
    {
        protected bool LayoutHorizontal { get; private set; }

        protected override Task OnInitializedAsync()
        {
            var layoutHorizontal = Container.DynamicArgs.FirstDynamicArgValueToStringOrDefault(Args.LAYOUT_HORIZONTAL);

            if (!string.IsNullOrWhiteSpace(layoutHorizontal))
            {
                LayoutHorizontal = Convert.ToBoolean(layoutHorizontal);
            }

            Container.ApplyStateReadOnlyFlag();

            return base.OnInitializedAsync();
        }
    }
}

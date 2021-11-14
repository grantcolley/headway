using Headway.Core.Attributes;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public abstract class TabPageBase : DynamicComponentBase
    {
        [CascadingParameter]
        protected TabControl Parent { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string TabButtonText { get; set; }

        protected override void OnInitialized()
        {
            if (Parent == null)
            {
                throw new ArgumentNullException(nameof(Parent), "TabPage must have a parent TabControl");
            }

            base.OnInitialized();

            Parent.AddPage(this);
        }
    }
}

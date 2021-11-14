using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public abstract class TabControlBase : DynamicComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public TabPageBase ActivePage { get; set; }

        protected List<TabPageBase> Pages = new List<TabPageBase>();

        internal void AddPage(TabPageBase tabPage)
        {
            Pages.Add(tabPage);

            if (Pages.Count == 1)
            {
                ActivePage = tabPage;
            }

            StateHasChanged();
        }

        protected string GetTabButtonClass(TabPageBase page)
        {
            return page == ActivePage ? Css.BTN_PRIMARY : Css.BTN_SECONDARY;
        }

        protected void SetActivePage(TabPageBase page)
        {
            ActivePage = page;
        }
    }
}

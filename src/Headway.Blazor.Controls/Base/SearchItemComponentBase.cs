using Headway.Core.Args;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Extensions;
using Headway.Blazor.Controls.Callbacks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;

namespace Headway.Blazor.Controls.Base
{
    public class SearchItemComponentBase : HeadwayComponentBase
    {
        [CascadingParameter]
        public SearchItemCallback SearchItemCallBack { get; set; }

        [Parameter]
        public DynamicSearchItem SearchItem { get; set; }

        [Parameter]
        public List<Arg> ComponentArgs { get; set; }

        public string PropertyValue
        {
            get { return SearchItem.Value; }
            set { SearchItem.Value = value; }
        }

        protected async void OnKeyDown(KeyboardEventArgs e)
        {
            if(e.Key.Equals(KeyCodes.ENTER))
            {
                await SearchItemCallBack?.OnKeyDown.Invoke();
            }
        }

        protected void LinkFieldCheck()
        {
            if (SearchItem.IsLinkedSearchItem)
            {
                SetLinkedValue();
            }
        }

        protected void SetLinkedValue()
        {
            if (SearchItem.IsLinkedSearchItem)
            {
                var value = SearchItem.LinkValue;

                var linkValueArg = ComponentArgs.FirstArgOrDefault(Args.LINK_VALUE);

                if (linkValueArg == null)
                {
                    linkValueArg = new Arg { Name = Args.LINK_VALUE };
                    ComponentArgs.Add(linkValueArg);
                }

                linkValueArg.Value = value;
            }
        }
    }
}

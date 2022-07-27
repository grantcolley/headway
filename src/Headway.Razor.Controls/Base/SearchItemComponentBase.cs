using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Extensions;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Headway.Razor.Controls.Base
{
    public class SearchItemComponentBase : HeadwayComponentBase
    {
        [Parameter]
        public DynamicSearchItem SearchItem { get; set; }

        [Parameter]
        public List<Arg> ComponentArgs { get; set; }

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

                var linkValueArg = ComponentArgs.ArgOrDefault(Args.LINK_VALUE);

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

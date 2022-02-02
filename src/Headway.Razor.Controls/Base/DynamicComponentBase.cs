using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Razor.Controls.Base
{
    public abstract class DynamicComponentBase : HeadwayComponentBase
    {
        [Parameter]
        public DynamicField Field { get; set; }

        [Parameter]
        public List<DynamicArg> ComponentArgs { get; set; }

        protected void LinkFieldCheck()
        {
            if (Field.IsLinkedField)
            {
                SetLinkedValue();
            }
        }

        protected void SetLinkedValue()
        {
            if (Field.IsLinkedField)
            {
                var value = Field.LinkValue;

                var linkValueArg = ComponentArgs.FirstOrDefault(a => a.Name.Equals(Args.LINK_VALUE));

                if(linkValueArg == null)
                {
                    linkValueArg = new DynamicArg { Name = Args.LINK_VALUE };
                    ComponentArgs.Add(linkValueArg);
                }

                linkValueArg.Value = value;
            }
        }
    }
}

using Headway.Core.Args;
using Headway.Core.Constants;
using Headway.Core.Dynamic;
using Headway.Core.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Headway.Blazor.Controls.Base
{
    public abstract class DynamicComponentBase : HeadwayComponentBase
    {
        [CascadingParameter]
        public EditContext CurrentEditContext { get; set; }

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

                var linkValueArg = ComponentArgs.FirstDynamicArgOrDefault(Args.LINK_VALUE);

                if(linkValueArg == null)
                {
                    linkValueArg = new DynamicArg { Name = Args.LINK_VALUE };
                    ComponentArgs.Add(linkValueArg);
                }

                linkValueArg.Value = value;
            }
        }

        protected void PropagateLinkedFields(List<DynamicField> dynamicFields)
        {
            var propagateFieldsArg = ComponentArgs.FirstDynamicArgOrDefault(Args.PROPAGATE_FIELDS);

            if (propagateFieldsArg == null
                || propagateFieldsArg.Value == null)
            {
                return;
            }

            var sourceFieldArgNames = propagateFieldsArg.Value.ToString().Split(',');
            var sourceFieldArgs = (from a in ComponentArgs
                                   join n in sourceFieldArgNames on a.Name equals n
                                   select a).ToList();

            dynamicFields.PropagateDynamicArgs(sourceFieldArgs);
        }
    }
}

using Headway.Core.Dynamic;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Headway.Razor.Controls.Base
{
    public abstract class DynamicComponentBase : HeadwayComponentBase
    {
        [Parameter]
        public DynamicField Field { get; set; }

        [Parameter]
        public List<DynamicArg> ComponentArgs { get; set; }
    }
}

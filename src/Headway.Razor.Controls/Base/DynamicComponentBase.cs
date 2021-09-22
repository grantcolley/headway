using Headway.Core.Dynamic;
using Headway.Core.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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

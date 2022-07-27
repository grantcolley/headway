using Headway.Core.Dynamic;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Headway.Razor.Controls.SearchComponents
{
    public class StandardSearchComponentBase : HeadwayComponentBase
    {
        [Parameter]
        public List<DynamicSearchItem> SearchItems { get; set; }
    }
}

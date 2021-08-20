using Headway.Core.Attributes;
using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Razor.Controls.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public class DropdownBase : HeadwayComponentBase
    {
        [Inject]
        public IOptionsService OptionsService { get; set; }

        [Parameter]
        public DynamicField Field { get; set; }

        public Expression<Func<string>> FieldExpression
        {
            get
            {
                return Expression.Lambda<Func<string>>(Field.MemberExpression);
            }
        }

        public string PropertyValue
        {
            get
            {
                return Field.PropertyInfo.GetValue(Field.Model)?.ToString();
            }
        }

        protected IEnumerable<OptionItem> OptionItems;

        public void OnValueChanged(string value)
        {
            Field.PropertyInfo.SetValue(Field.Model, value);
        }

        protected override async Task OnParametersSetAsync()
        {
            var result = await OptionsService.GetOptionItemsAsync(Field.ContainerArgs).ConfigureAwait(false);

            OptionItems = GetResponse(result);

            await base.OnParametersSetAsync();
        }
    }
}

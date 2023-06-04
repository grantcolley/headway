using Headway.Blazor.Controls.Base;
using Headway.Core.Attributes;
using Headway.Core.Interface;
using Headway.Core.Notifications;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Components
{
    [DynamicComponent]
    public abstract class DropdownTextBase : DynamicComponentBase
    {
        [Inject]
        public IStateNotification StateNotification { get; set; }

        [Inject]
        public IOptionsApiRequest OptionsApiRequest { get; set; }

        protected IEnumerable<string> Items = new List<string>();

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
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Field.PropertyInfo.SetValue(Field.Model, null);
                }
                else
                {
                    Field.PropertyInfo.SetValue(Field.Model, value);
                }
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            LinkFieldCheck();

            var result = await OptionsApiRequest.GetOptionTextItemsAsync(ComponentArgs).ConfigureAwait(false);

            Items = GetResponse(result);

            await base.OnParametersSetAsync().ConfigureAwait(false);
        }

        protected async virtual void OnValueChanged(IEnumerable<string> values)
        {
            if (Field.HasLinkDependents)
            {
                StateNotification.NotifyStateHasChanged(Field.ContainerUniqueId);
            }

            await InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }
    }
}

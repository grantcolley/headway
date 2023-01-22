using Headway.Blazor.Controls.Base;
using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Core.Notifications;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Components.Mobile
{
    [DynamicComponent]
    public abstract class DropdownBase : DynamicComponentBase
    {
        [Inject]
        public IStateNotification StateNotification { get; set; }

        [Inject]
        public IOptionsApiRequest OptionsApiRequest { get; set; }

        protected IEnumerable<OptionItem> optionItems;

        private bool isNumericId = false;

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
                else if (isNumericId)
                {
                    Field.PropertyInfo.SetValue(Field.Model, int.Parse(value));
                }
                else
                {
                    Field.PropertyInfo.SetValue(Field.Model, value);
                }
            }
        }

        protected override void OnInitialized()
        {
            var isNumericIdArg = ComponentArgHelper.GetArg(ComponentArgs, Args.IS_NUMERIC_ID);

            if (isNumericIdArg != null)
            {
                isNumericId = bool.Parse(isNumericIdArg.Value);
            }

            base.OnInitialized();
        }

        protected override async Task OnParametersSetAsync()
        {
            LinkFieldCheck();

            var result = await OptionsApiRequest.GetOptionItemsAsync(ComponentArgs).ConfigureAwait(false);

            optionItems = GetResponse(result);

            OptionItem selectedItem = null;

            if (isNumericId)
            {
                var id = (int)Field.PropertyInfo.GetValue(Field.Model);
                selectedItem = optionItems.FirstOrDefault(o => o.Id != null && o.Id.Equals(id.ToString()));
            }
            else
            {
                var id = Field.PropertyInfo.GetValue(Field.Model)?.ToString();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    selectedItem = optionItems.FirstOrDefault(o => o.Id != null && o.Id.Equals(id));
                }
            }

            if(selectedItem == null)
            {
                selectedItem = optionItems.First();
            }

            PropertyValue = selectedItem.Id;

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

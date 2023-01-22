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

namespace Headway.Blazor.Controls.Base
{
    [DynamicComponent]
    public abstract class OptionBase : DynamicComponentBase
    {
        [Inject]
        public IStateNotification StateNotification { get; set; }

        [Inject]
        public IOptionsApiRequest OptionsApiRequest { get; set; }

        protected List<OptionItem> options;

        private bool dynamicOptions;

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
                Field.PropertyInfo.SetValue(Field.Model, value);

                if (Field.HasLinkDependents)
                {
                    StateNotification.NotifyStateHasChanged(Field.ContainerUniqueId);
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            var arg = ComponentArgHelper.GetArg(ComponentArgs, Options.OPTIONS_CODE);

            if (arg == null)
            {
                options = ComponentArgs
                    .Select(a => new OptionItem { Id = a.Name, Display = a.Value.ToString() })
                    .ToList();
            }
            else
            {
                dynamicOptions = true;
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            if (dynamicOptions)
            {
                LinkFieldCheck();

                var result = await OptionsApiRequest.GetOptionItemsAsync(ComponentArgs).ConfigureAwait(false);

                var optionItems = GetResponse(result);

                options = new List<OptionItem>(optionItems);
            }

            await OnInitializedAsync().ConfigureAwait(false);
        }
    }
}

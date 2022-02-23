using Headway.Core.Attributes;
using Headway.Core.Model;
using Headway.Core.Notifications;
using Headway.Razor.Controls.Base;
using Headway.RequestApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Components
{
    [DynamicComponent]
    public abstract class DropdownBase : DynamicComponentBase
    {
        [Inject]
        public IStateNotification StateNotification { get; set; }

        [Inject]
        public IMediator Mediator { get; set; }

        protected IEnumerable<OptionItem> optionItems;

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

        protected override async Task OnParametersSetAsync()
        {
            LinkFieldCheck();

            var result = await Mediator.Send(new OptionItemsRequest(ComponentArgs)).ConfigureAwait(false);

            optionItems = GetResponse(result.OptionItems);

            await base.OnParametersSetAsync().ConfigureAwait(false);
        }

        public virtual void OnValueChanged(string value)
        {
            Field.PropertyInfo.SetValue(Field.Model, value);

            if(Field.HasLinkDependents)
            {
                StateNotification.NotifyStateHasChanged(Field.ContainerUniqueId);
            }
        }
    }
}

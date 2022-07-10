using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Helpers;
using Headway.Core.Model;
using Headway.Core.Notifications;
using Headway.Razor.Controls.Base;
using Headway.RequestApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
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

        private OptionItem selectedItem;

        private bool isNumericId = false;

        public OptionItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if(selectedItem != value)
                {
                    selectedItem = value;

                    if (selectedItem == null
                        || string.IsNullOrWhiteSpace(selectedItem.Id))
                    {
                        Field.PropertyInfo.SetValue(Field.Model, null);
                    }
                    else if (isNumericId)
                    {
                        Field.PropertyInfo.SetValue(Field.Model, int.Parse(SelectedItem.Id));
                    }
                    else
                    {
                        Field.PropertyInfo.SetValue(Field.Model, SelectedItem.Id);
                    }
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

            var result = await Mediator.Send(new OptionItemsRequest(ComponentArgs)).ConfigureAwait(false);

            optionItems = GetResponse(result.OptionItems);

            if (isNumericId)
            {
                var id = (int)Field.PropertyInfo.GetValue(Field.Model);
                SelectedItem = optionItems.FirstOrDefault(o => o.Id != null && o.Id.Equals(id.ToString()));
            }
            else
            {
                var id = Field.PropertyInfo.GetValue(Field.Model)?.ToString();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    SelectedItem = optionItems.FirstOrDefault(o => o.Id != null && o.Id.Equals(id));
                }
            }

            if(selectedItem == null)
            {
                selectedItem = optionItems.FirstOrDefault();
            }

            await base.OnParametersSetAsync().ConfigureAwait(false);
        }

        public virtual void OnValueChanged(IEnumerable<OptionItem> values)
        {
            if (Field.HasLinkDependents)
            {
                StateNotification.NotifyStateHasChanged(Field.ContainerUniqueId);
            }
        }
    }
}

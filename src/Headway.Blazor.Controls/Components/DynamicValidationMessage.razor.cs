using Headway.Core.Dynamic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Headway.Blazor.Controls.Components
{
    public class DynamicValidationMessageBase<TValue> : ComponentBase, IDisposable
    {
        private FieldIdentifier fieldIdentifier;

        [Parameter]
        public Expression<Func<TValue>> For { get; set; }

        [CascadingParameter]
        private EditContext EditContext { get; set; }

        [Parameter]
        public DynamicField Field { get; set; }

        protected IEnumerable<string> ValidationMessages
        {
            get 
            {
                var validationMessages = EditContext.GetValidationMessages(fieldIdentifier);
                Field.ValidationMessagesCount = validationMessages.Count();
                return validationMessages;
            }
        }

        protected override void OnInitialized()
        {
            fieldIdentifier = FieldIdentifier.Create(For);
            EditContext.OnValidationStateChanged += HandleValidationStateChanged;
        }

        public void Dispose()
        {
            EditContext.OnValidationStateChanged -= HandleValidationStateChanged;

            GC.SuppressFinalize(this);
        }

        private async void HandleValidationStateChanged(object o, ValidationStateChangedEventArgs args)
        {
            await InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }
    }
}

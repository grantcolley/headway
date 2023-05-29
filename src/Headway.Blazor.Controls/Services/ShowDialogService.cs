﻿using Headway.Blazor.Controls.Dialogs;
using MudBlazor;
using System.Threading.Tasks;

namespace Headway.Blazor.Controls.Services
{
    public class ShowDialogService : IShowDialogService
    {
        private readonly IDialogService dialogService;

        public ShowDialogService(IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        public Task<DialogResult> ShowAsync(
            string title, string message, string buttonText, 
            bool closeButton, Color color, bool scrollable)
        {
            var parameters = new DialogParameters
            {
                { "ContentText", message },
                { "ButtonText", buttonText },
                { "Color", color }
            };

            var options = new DialogOptions() 
            {
                CloseButton = closeButton,
                MaxWidth = MaxWidth.ExtraSmall                 
            };

            if (scrollable)
            {
                return dialogService.Show<ScrollableDialog>(title, parameters, options).Result;
            }
            else
            {
                return dialogService.Show<ScrollableDialog>(title, parameters, options).Result;
            }
        }
    }
}

using Headway.Razor.Controls.Dialogs;
using MudBlazor;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Services
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
            var parameters = new DialogParameters();
            parameters.Add("ContentText", message);
            parameters.Add("ButtonText", buttonText);
            parameters.Add("Color", color);

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

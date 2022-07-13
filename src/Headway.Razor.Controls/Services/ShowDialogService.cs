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

        public async Task<DialogResult> ValidationErrorsAsync(string message)
        {
            var parameters = new DialogParameters();
            parameters.Add("ContentText", message);
            parameters.Add("ButtonText", "Ok");
            parameters.Add("Color", Color.Error);

            var options = new DialogOptions() { MaxWidth = MaxWidth.ExtraSmall };

            var result = dialogService.Show<ScrollableDialog>("Validation Errors", parameters, options);

            return await result.Result.ConfigureAwait(false);
        }

        public async Task<DialogResult> DeleteAsync(string message)
        {
            var parameters = new DialogParameters();
            parameters.Add("ContentText", message);
            parameters.Add("ButtonText", "Delete");
            parameters.Add("Color", Color.Error);

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

            var result = dialogService.Show<StandardDialog>("Delete", parameters, options);

            return await result.Result.ConfigureAwait(false);
        }
    }
}

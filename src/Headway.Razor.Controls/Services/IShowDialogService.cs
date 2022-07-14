using MudBlazor;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Services
{
    public interface IShowDialogService
    {
        Task<DialogResult> ShowAsync(string title, string message, string buttonText, bool closeButton, Color color, bool scrollable);
    }
}

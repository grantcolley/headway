using MudBlazor;
using System.Threading.Tasks;

namespace Headway.Razor.Controls.Services
{
    public interface IShowDialogService
    {
        Task<DialogResult> DeleteAsync(string message);
    }
}

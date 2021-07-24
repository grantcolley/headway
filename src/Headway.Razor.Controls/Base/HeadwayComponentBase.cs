using Headway.Core.Interface;
using Headway.Razor.Controls.Model;
using Microsoft.AspNetCore.Components;

namespace Headway.Razor.Controls.Base
{
    public abstract class HeadwayComponentBase : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected T GetResponse<T>(IServiceResult<T> result)
        {
            if (!result.IsSuccess)
            {
                RaiseAlert(result.Message);
                return default;
            }
            else
            {
                return result.Result;
            }
        }

        protected void RaiseAlert(string message)
        {
            var alert = new Alert
            {
                AlertType = "danger",
                Title = "Error",
                Message = message
            };

            NavigationManager.NavigateTo(alert.Page);
        }
    }
}

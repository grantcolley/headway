using Headway.Core.Constants;
using Headway.Core.Interface;
using Headway.Razor.Controls.Model;
using Microsoft.AspNetCore.Components;

namespace Headway.Razor.Controls.Base
{
    public abstract class HeadwayComponentBase : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected T GetResponse<T>(IResponse<T> response)
        {
            if (!response.IsSuccess)
            {
                RaiseAlert(response.Message);
                return default;
            }
            else
            {
                return response.Result;
            }
        }

        protected void RaiseAlert(string message)
        {
            var alert = new Alert
            {
                AlertType = Alerts.ERROR,
                Title = "Error",
                Message = message
            };

            NavigationManager.NavigateTo(alert.Page);
        }
    }
}

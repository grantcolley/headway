using Headway.Core.Constants;
using Headway.Core.Interface;
using Headway.Razor.Controls.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics;

namespace Headway.Razor.Controls.Base
{
    public abstract class HeadwayComponentBase : ComponentBase, IDisposable
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public void Dispose()
        {
            Debug.WriteLine($"Dispose {GetType().Name}");

            GC.SuppressFinalize(this);
        }

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
                AlertType = Alerts.DANGER,
                Title = "Error",
                Message = message
            };

            NavigationManager.NavigateTo(alert.Page);
        }
    }
}

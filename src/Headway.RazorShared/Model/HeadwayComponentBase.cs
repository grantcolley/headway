using Headway.Core.Interface;
using Microsoft.AspNetCore.Components;

namespace Headway.RazorShared.Model
{
    public class HeadwayComponentBase : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected T GetResponse<T>(IServiceResult<T> result)
        {
            if (!result.IsSuccess)
            {
                var alert = new Alert
                {
                    AlertType = "danger",
                    Title = "Error",
                    Message = result.Message
                };

                NavigationManager.NavigateTo(alert.Page);
                return default;
            }
            else
            {
                return result.Result;
            }
        }
    }
}

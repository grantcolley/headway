//using Headway.Core.Attributes;
//using Headway.Core.Model;
//using Headway.Razor.Components.Base;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace Headway.Razor.Configuration.Components
//{
//    [DynamicConfiguration]
//    public partial class ListConfigsBase : DynamicTypeComponentBase
//    {

//        protected IEnumerable<Config> listConfigs;

//        protected override async Task OnInitializedAsync()
//        {
//            var result = await ConfigurationService.GetConfigsAsync().ConfigureAwait(false);

//            await base.OnInitializedAsync().ConfigureAwait(false);
//        }

//        protected void Add()
//        {
//            //NavigationManager.NavigateTo($"{dynamicList.ListConfig.NavigateTo}/{TypeName}");
//        }

//        protected void Update(object id)
//        {
//            //NavigationManager.NavigateTo($"{dynamicList.ListConfig.NavigateTo}/{TypeName}/{id}");
//        }
//    }
//}

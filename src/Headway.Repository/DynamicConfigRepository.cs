using Headway.Core.Dynamic;
using Headway.Core.Interface;
using Headway.Repository.Data;
using System;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class DynamicConfigRepository : RepositoryBase, IDynamicConfigRepository
    {
        public DynamicConfigRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }

        public Task<DynamicModelConfig> GetDynamicModelConfigAsync(string model)
        {
            if(model.Equals("Permission"))
            {
                var dynamicModelConfig = new DynamicModelConfig
                {
                    ConfigName = "Permission",
                    ConfigPath = "Permissions",
                    RedirectPage = "/Permissions",
                    RedirectText = "Return to permissions."
                };

                dynamicModelConfig.FieldConfigs.AddRange(new[]
                {
                    new DynamicFieldConfig
                    { 
                        PropertyName = "PermissionId",
                        Order  = 1,
                        DynamicComponentTypeName = "Headway.RazorShared.Components.LabelData, Headway.RazorShared"
                    },
                    new DynamicFieldConfig
                    {
                        PropertyName = "Name",
                        Order  = 2,
                        DynamicComponentTypeName = "Headway.RazorShared.Components.LabelText, Headway.RazorShared",
                        IsTitleField = true
                    },
                    new DynamicFieldConfig
                    {
                        PropertyName = "Description",
                        Order  = 3,
                        DynamicComponentTypeName = "Headway.RazorShared.Components.LabelText, Headway.RazorShared"
                    }
                });

                return Task.FromResult(dynamicModelConfig);
            }

            throw new NotImplementedException(model);
        }
    }
}

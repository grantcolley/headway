using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Data;
using System;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class ConfigRepository : RepositoryBase, IConfigRepository
    {
        public ConfigRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }

        public Task<ModelConfig> GetModelConfigAsync(string model)
        {
            if(model.Equals("Permission"))
            {
                var dynamicModelConfig = new ModelConfig
                {
                    ConfigName = "Permission",
                    ConfigPath = "Permissions",
                    RedirectPage = "/Permissions",
                    RedirectText = "Return to permissions."
                };

                dynamicModelConfig.FieldConfigs.AddRange(new[]
                {
                    new FieldConfig
                    { 
                        PropertyName = "PermissionId",
                        Order  = 1,
                        DynamicComponentTypeName = "Headway.RazorShared.Components.LabelData, Headway.RazorShared",
                        IsIdField = true
                    },
                    new FieldConfig
                    {
                        PropertyName = "Name",
                        Order  = 2,
                        DynamicComponentTypeName = "Headway.RazorShared.Components.LabelText, Headway.RazorShared",
                        IsTitleField = true
                    },
                    new FieldConfig
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

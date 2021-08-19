using Headway.Core.Attributes;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Core.Options;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class OptionsRepository : RepositoryBase, IOptionsRepository
    {
        private readonly Dictionary<string, Func<object, Task<IEnumerable<OptionItem>>>> optionItems = new();

        public OptionsRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
            optionItems["ControllerOptionItems"] = new Func<object, Task<IEnumerable<OptionItem>>>(GetControllerOptionItemsAsync);
            optionItems["ConfigOptionItems"] = new Func<object, Task<IEnumerable<OptionItem>>>(ConfigOptionItems);
        }

        public async Task<IEnumerable<OptionItem>> GetOptionItemsAsync(string optionsCode)
        {
            if (optionItems.ContainsKey(optionsCode))
            {
                return await optionItems[optionsCode].Invoke(null);
            }

            throw new NotImplementedException();
        }

        private Task<IEnumerable<OptionItem>> GetControllerOptionItemsAsync(object arg = null)
        {
            var controllers = TypeAttributeHelper.GetEntryAssemblyTypeNamesByAttribute(typeof(DynamicApiControllerAttribute));

            var optionItems = from c in controllers
                              select new OptionItem
                              {
                                  Id = c.Name.Replace("Controller", ""),
                                  Display = c.DisplayName.Replace("Controller", "")
                              };

            return Task.FromResult(optionItems);
        }

        private async Task<IEnumerable<OptionItem>> ConfigOptionItems(object arg = null)
        {
            var configs = await applicationDbContext.Configs
                .OrderBy(c => c.Name)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            return configs.Select(c => new OptionItem { Id = c.Name, Display = c.Title }).ToList();
        }
    }
}

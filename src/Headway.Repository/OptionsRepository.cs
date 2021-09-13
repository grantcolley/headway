using Headway.Core.Attributes;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Core.Options;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class OptionsRepository : RepositoryBase, IOptionsRepository
    {
        private readonly Dictionary<string, Func<List<Arg>, Task<IEnumerable<OptionItem>>>> optionItems = new();

        public OptionsRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
            optionItems["ControllerOptionItems"] = new Func<List<Arg>, Task<IEnumerable<OptionItem>>>(GetControllerOptionItemsAsync);
            optionItems["ConfigOptionItems"] = new Func<List<Arg>, Task<IEnumerable<OptionItem>>>(ConfigOptionItems);
        }

        public async Task<IEnumerable<OptionItem>> GetOptionItemsAsync(string optionsCode, List<Arg> args)
        {
            if (optionItems.ContainsKey(optionsCode))
            {
                return await optionItems[optionsCode].Invoke(args);
            }

            throw new NotImplementedException(optionsCode);
        }

        private Task<IEnumerable<OptionItem>> GetControllerOptionItemsAsync(List<Arg> args)
        {
            var controllers = TypeAttributeHelper.GetEntryAssemblyTypeNamesByAttribute(typeof(DynamicApiControllerAttribute));

            List<OptionItem> optionItems = new() { new OptionItem() };

            optionItems.AddRange((from c in controllers
                              select new OptionItem
                              {
                                  Id = c.Name.Replace("Controller", ""),
                                  Display = c.DisplayName.Replace("Controller", "")
                              }).ToList());

            return Task.FromResult(optionItems.AsEnumerable());
        }

        private async Task<IEnumerable<OptionItem>> ConfigOptionItems(List<Arg> args)
        {
            var configs = await applicationDbContext.Configs
                .OrderBy(c => c.Name)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            List<OptionItem> optionItems = new() { new OptionItem() };

            optionItems.AddRange(configs.Select(c => new OptionItem { Id = c.Name, Display = c.Title }).ToList());

            return optionItems;
        }
    }
}

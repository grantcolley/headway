using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class OptionsRepository : RepositoryBase<OptionsRepository>, IOptionsRepository
    {
        private readonly Dictionary<string, Func<List<Arg>, Task<IEnumerable<OptionItem>>>> optionItems = new();
        private readonly Dictionary<string, Func<List<Arg>, Task<string>>> complexOptionItems = new();

        public OptionsRepository(ApplicationDbContext applicationDbContext, ILogger<OptionsRepository> logger)
            : base(applicationDbContext, logger)
        {
            optionItems[Options.CONTROLLER_OPTION_ITEMS] = new Func<List<Arg>, Task<IEnumerable<OptionItem>>>(GetControllerOptionItemsAsync);
            optionItems[Options.CONFIG_OPTION_ITEMS] = new Func<List<Arg>, Task<IEnumerable<OptionItem>>>(GetConfigOptionItems);
            optionItems[Options.PERMISSIONS_OPTION_ITEMS] = new Func<List<Arg>, Task<IEnumerable<OptionItem>>>(GetPermissionsOptionItems);

            complexOptionItems[Options.CONFIG_CONTAINERS] = new Func<List<Arg>, Task<string>>(GetConfigContainers);
            complexOptionItems[Options.MODULES_OPTION_ITEMS] = new Func<List<Arg>, Task<string>>(GetModules);
            complexOptionItems[Options.CATEGORIES_OPTION_ITEMS] = new Func<List<Arg>, Task<string>>(GetCategories);
            complexOptionItems[Options.COMPLEX_OPTION_ITEMS] = new Func<List<Arg>, Task<string>>(GetDemoModelComplexProperties);            
        }

        public async Task<string> GetComplexOptionItemsAsync(List<Arg> args)
        {
            var optionsCode = args.Single(a => a.Name.Equals(Options.OPTIONS_CODE)).Value.ToString();

            if (complexOptionItems.ContainsKey(optionsCode))
            {
                return await complexOptionItems[optionsCode].Invoke(args).ConfigureAwait(false);
            }

            throw new NotImplementedException(optionsCode);
        }

        public async Task<IEnumerable<OptionItem>> GetOptionItemsAsync(List<Arg> args)
        {
            var optionsCode = args.Single(a => a.Name.Equals(Options.OPTIONS_CODE)).Value.ToString();

            if (optionItems.ContainsKey(optionsCode))
            {
                return await optionItems[optionsCode].Invoke(args).ConfigureAwait(false);
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
                                  Id = c.Name.Replace(Options.CONTROLLER, ""),
                                  Display = c.DisplayName.Replace(Options.CONTROLLER, "")
                              }).ToList());

            return Task.FromResult(optionItems.AsEnumerable());
        }

        private async Task<IEnumerable<OptionItem>> GetPermissionsOptionItems(List<Arg> args)
        {
            var configs = await applicationDbContext.Permissions
                .OrderBy(p => p.Name)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            List<OptionItem> optionItems = new() { new OptionItem() };

            optionItems.AddRange(configs.Select(p => new OptionItem { Id = p.Name, Display = p.Name }).ToList());

            return optionItems;
        }

        private async Task<IEnumerable<OptionItem>> GetConfigOptionItems(List<Arg> args)
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

        private async Task<string> GetConfigContainers(List<Arg> args)
        {
            var configId = args.First(a => a.Name.Equals(Args.LINK_VALUE)).Value;

            var configs = await applicationDbContext.Configs
                .Include(c => c.ConfigContainers)
                .Where(c => c.ConfigId.Equals(int.Parse(configId)))
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            if (configs.Any())
            {
                var configContainers = configs.Single().ConfigContainers.ToList();
                configContainers.Insert(0, new ConfigContainer());
                return JsonSerializer.Serialize(configContainers);
            }
            else
            {
                return string.Empty;
            }
        }

        private async Task<string> GetModules(List<Arg> args)
        {
            var modules = await applicationDbContext.Modules
                .AsNoTracking()
                .OrderBy(m => m.Name)
                .ToListAsync()
                .ConfigureAwait(false);

            if (modules.Any())
            {
                modules.Insert(0, new Module());
                return JsonSerializer.Serialize(modules);
            }
            else
            {
                return string.Empty;
            }
        }

        private async Task<string> GetCategories(List<Arg> args)
        {
            var categories = await applicationDbContext.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync()
                .ConfigureAwait(false);

            if (categories.Any())
            {
                categories.Insert(0, new Category());
                return JsonSerializer.Serialize(categories);
            }
            else
            {
                return string.Empty;
            }
        }

        private async Task<string> GetDemoModelComplexProperties(List<Arg> args)
        {
            var demoModelComplexProperties = await applicationDbContext.DemoModelComplexProperties
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync()
                .ConfigureAwait(false);

            if (demoModelComplexProperties.Any())
            {
                demoModelComplexProperties.Insert(0, new DemoModelComplexProperty());
                return JsonSerializer.Serialize(demoModelComplexProperties);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}

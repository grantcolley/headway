using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Extensions;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.RemediatR.Core.Constants;
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
            optionItems[Options.COUNTRY_OPTION_ITEMS] = new Func<List<Arg>, Task<IEnumerable<OptionItem>>>(GetCountryOptionItems);
            optionItems[RemediatROptions.Programs] = new Func<List<Arg>, Task<IEnumerable<OptionItem>>>(GetRemediatRPrograms);

            complexOptionItems[Options.CONFIG_CONTAINERS] = new Func<List<Arg>, Task<string>>(GetConfigContainers);
            complexOptionItems[Options.MODULES_OPTION_ITEMS] = new Func<List<Arg>, Task<string>>(GetModules);
            complexOptionItems[Options.CATEGORIES_OPTION_ITEMS] = new Func<List<Arg>, Task<string>>(GetCategories);
            complexOptionItems[Options.COMPLEX_OPTION_ITEMS] = new Func<List<Arg>, Task<string>>(GetDemoModelComplexProperties);
        }

        public async Task<string> GetComplexOptionItemsAsync(List<Arg> args)
        {
            var optionsCode = args.ArgValue(Options.OPTIONS_CODE);

            if (complexOptionItems.ContainsKey(optionsCode))
            {
                return await complexOptionItems[optionsCode].Invoke(args).ConfigureAwait(false);
            }

            throw new NotImplementedException(optionsCode);
        }

        public async Task<IEnumerable<OptionItem>> GetOptionItemsAsync(List<Arg> args)
        {
            var optionsCode = args.ArgValue(Options.OPTIONS_CODE);

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

        private async Task<IEnumerable<OptionItem>> GetCountryOptionItems(List<Arg> args)
        {
            var countries = await applicationDbContext.Countries
                .OrderBy(c => c.Name)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            var unitedKingdom = countries.First(c => c.Code.Equals("GB"));

            var ukOptionItem = new OptionItem
            {
                Id = unitedKingdom.Name,
                Display = unitedKingdom.Name
            };

            List<OptionItem> optionItems = new() { new OptionItem(), ukOptionItem };

            optionItems.AddRange(countries
                                    .Where(c => !c.Code.Equals("GB"))
                                    .Select(c => new OptionItem 
                                    {
                                        Id = c.Name, 
                                        Display = c.Name 
                                    }).ToList());

            return optionItems;
        }

        private async Task<IEnumerable<OptionItem>> GetRemediatRPrograms(List<Arg> args)
        {
            var configs = await applicationDbContext.Programs
                .OrderBy(p => p.Name)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            List<OptionItem> optionItems = new() { new OptionItem() };

            optionItems.AddRange(configs.Select(p => new OptionItem { Id = p.Name, Display = p.Name }).ToList());

            return optionItems;
        }

        private async Task<string> GetConfigContainers(List<Arg> args)
        {
            var configId = args.ArgValue(Args.LINK_VALUE);

            var configs = await applicationDbContext.Configs
                .Include(c => c.ConfigContainers)
                .Where(c => c.ConfigId.Equals(int.Parse(configId)))
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);

            if (configs.Any())
            {
                var configContainers = configs.First().ConfigContainers.ToList();
                configContainers.Insert(0, new ConfigContainer());
                return JsonSerializer.Serialize(configContainers);
            }
            else
            {
                return JsonSerializer.Serialize(new List<ConfigContainer>());
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
                return JsonSerializer.Serialize(modules);
            }
            else
            {
                return JsonSerializer.Serialize(new List<Module>());
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
                return JsonSerializer.Serialize(categories);
            }
            else
            {
                return JsonSerializer.Serialize(new List<Category>());
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
                return JsonSerializer.Serialize(new List<DemoModelComplexProperty>());
            }
        }
    }
}

using Headway.Core.Args;
using Headway.Core.Attributes;
using Headway.Core.Constants;
using Headway.Core.Extensions;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RemediatR.Core.Constants;
using RemediatR.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Headway.Repository.Repositories
{
    public class OptionsRepository : RepositoryBase<OptionsRepository>, IOptionsRepository
    {
        private readonly Dictionary<string, Func<List<Arg>, Task<IEnumerable<OptionCheckItem>>>> checkListItems = new();
        private readonly Dictionary<string, Func<List<Arg>, Task<IEnumerable<OptionItem>>>> optionItems = new();
        private readonly Dictionary<string, Func<List<Arg>, Task<IEnumerable<string>>>> checkTextItems = new();
        private readonly Dictionary<string, Func<List<Arg>, Task<string>>> complexOptionItems = new();

        public OptionsRepository(ApplicationDbContext applicationDbContext, ILogger<OptionsRepository> logger)
            : base(applicationDbContext, logger)
        {
            optionItems[Options.CONTROLLER_OPTION_ITEMS] = new Func<List<Arg>, Task<IEnumerable<OptionItem>>>(GetControllerOptionItemsAsync);
            optionItems[Options.CONFIG_OPTION_ITEMS] = new Func<List<Arg>, Task<IEnumerable<OptionItem>>>(GetConfigOptionItems);
            optionItems[Options.PERMISSIONS_OPTION_ITEMS] = new Func<List<Arg>, Task<IEnumerable<OptionItem>>>(GetPermissionsOptionItems);
            optionItems[Options.COUNTRY_OPTION_ITEMS] = new Func<List<Arg>, Task<IEnumerable<OptionItem>>>(GetCountryOptionItems);
            optionItems[Options.AUTHORIZED_USERS] = new Func<List<Arg>, Task<IEnumerable<OptionItem>>>(GetAuthorisedUsers);
            optionItems[RemediatROptions.PROGRAMS_OPTION_ITEMS] = new Func<List<Arg>, Task<IEnumerable<OptionItem>>>(GetRemediatRPrograms);

            complexOptionItems[Options.CONFIG_CONTAINERS] = new Func<List<Arg>, Task<string>>(GetConfigContainers);
            complexOptionItems[Options.MODULES_OPTION_ITEMS] = new Func<List<Arg>, Task<string>>(GetModules);
            complexOptionItems[Options.CATEGORIES_OPTION_ITEMS] = new Func<List<Arg>, Task<string>>(GetCategories);
            complexOptionItems[Options.COMPLEX_OPTION_ITEMS] = new Func<List<Arg>, Task<string>>(GetDemoModelComplexProperties);
            complexOptionItems[Options.FLOWS_COMPLEX_OPTION_ITEMS] = new Func<List<Arg>, Task<string>>(GetComplexFlows);
            complexOptionItems[RemediatROptions.PROGRAMS_COMPLEX_OPTION_ITEMS] = new Func<List<Arg>, Task<string>>(GetRemediatRComplexPrograms);
        }

        public async Task<IEnumerable<string>> GetOptionTextItemsAsync(List<Arg> args)
        {
            var optionsCode = args.FirstArgValue(Options.OPTIONS_CODE);

            if (optionItems.ContainsKey(optionsCode))
            {
                return await checkTextItems[optionsCode].Invoke(args).ConfigureAwait(false);
            }

            throw new NotImplementedException(optionsCode);
        }

        public async Task<IEnumerable<OptionCheckItem>> GetOptionCheckItemsAsync(List<Arg> args)
        {
            var optionsCode = args.FirstArgValue(Options.OPTIONS_CODE);

            if (optionItems.ContainsKey(optionsCode))
            {
                return await checkListItems[optionsCode].Invoke(args).ConfigureAwait(false);
            }

            throw new NotImplementedException(optionsCode);
        }

        public async Task<string> GetComplexOptionItemsAsync(List<Arg> args)
        {
            var optionsCode = args.FirstArgValue(Options.OPTIONS_CODE);

            if (complexOptionItems.ContainsKey(optionsCode))
            {
                return await complexOptionItems[optionsCode].Invoke(args).ConfigureAwait(false);
            }

            throw new NotImplementedException(optionsCode);
        }

        public async Task<IEnumerable<OptionItem>> GetOptionItemsAsync(List<Arg> args)
        {
            var optionsCode = args.FirstArgValue(Options.OPTIONS_CODE);

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

        private async Task<IEnumerable<OptionItem>> GetAuthorisedUsers(List<Arg> args)
        {
            List<User> users = new List<User>();

            var level = args.FirstArgValue(Args.AUTHORIZED_LEVEL);
            var value = args.FirstArgValue(Args.VALUE);

            if (level != null)
            {
                if (level.Equals(Args.AUTHORIZED_ROLE))
                {
                    users.AddRange(await applicationDbContext.Users
                        .Where(u => u.Roles.Any(r => r.Name.Equals(value)))
                        .ToListAsync());
                }
                else if (level.Equals(Args.AUTHORIZED_PERMISSION))
                {
                    users.AddRange(await applicationDbContext.Users
                        .Where(u => u.Permissions.Any(p => p.Name.Equals(value))
                            || u.Roles.SelectMany(r => r.Permissions).Any(p => p.Name.Equals(value)))
                        .ToListAsync());
                }
            }

            List<OptionItem> optionItems = new() { new OptionItem() };

            if (users.Any())
            {
                optionItems.AddRange(users.Select(u => new OptionItem { Id = u.Email, Display = u.Email }).ToList());
            }

            return optionItems;
        }

        private async Task<string> GetConfigContainers(List<Arg> args)
        {
            var configId = args.FirstArgValue(Args.LINK_VALUE);

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

        private async Task<string> GetComplexFlows(List<Arg> args)
        {
            var flows = await applicationDbContext.Flows
                .AsNoTracking()
                .OrderBy(f => f.Name)
                .ToListAsync()
                .ConfigureAwait(false);

            if (flows.Any())
            {
                flows.Insert(0, new Flow());
                return JsonSerializer.Serialize(flows);
            }
            else
            {
                return JsonSerializer.Serialize(new List<Flow>());
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

        private async Task<string> GetRemediatRComplexPrograms(List<Arg> args)
        {
            var remediatRPrograms = await applicationDbContext.Programs
                .AsNoTracking()
                .OrderBy(p => p.Name)
                .ToListAsync()
                .ConfigureAwait(false);

            if (remediatRPrograms.Any())
            {
                return JsonSerializer.Serialize(remediatRPrograms);
            }
            else
            {
                return JsonSerializer.Serialize(new List<Program>());
            }
        }
    }
}

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

            complexOptionItems[Options.CONFIG_CONTAINERS] = new Func<List<Arg>, Task<string>>(GetConfigContainers);
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
            var configName = args.Single(a => a.Name.Equals(Args.SEARCH_PARAMETER)).Value;

            var configs = await applicationDbContext.Configs
                .Include(c => c.ConfigContainers)
                .Where(c => c.Name.Equals(configName))
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
    }
}

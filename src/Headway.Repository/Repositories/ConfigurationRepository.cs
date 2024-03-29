﻿using Headway.Core.Constants;
using Headway.Core.Helpers;
using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Constants;
using Headway.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Headway.Repository.Repositories
{
    public class ConfigurationRepository : RepositoryBase<ConfigurationRepository>, IConfigurationRepository
    {
        private readonly GenericTreeHelperArgs genericTreeHelperArgs;

        public ConfigurationRepository(ApplicationDbContext applicationDbContext, ILogger<ConfigurationRepository> logger)
            : base(applicationDbContext, logger)
        {
            genericTreeHelperArgs = new GenericTreeHelperArgs
            {
                ModelIdProperty = GenericTreeArgs.CONFIG_ID,
                ItemsProperty = GenericTreeArgs.CONFIG_CONTAINERS,
                ItemCodeProperty = Args.CODE,
                ParentItemCodeProperty = Args.CODE_PARENT,
                OrderByProperty = GenericTreeArgs.CONFIG_CONTAINER_ORDER
            };
        }

        public async Task<IEnumerable<Config>> GetConfigsAsync()
        {
            return await applicationDbContext.Configs
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Config> GetConfigAsync(int id)
        {
            var config = await applicationDbContext.Configs
                .AsNoTracking()
                .Include(c => c.Flow)
                .Include(c => c.ConfigContainers)
                .Include(c => c.ConfigSearchItems)
                .Include(c => c.ConfigItems)
                .ThenInclude(ci => ci.ConfigContainer)
                .FirstAsync(c => c.ConfigId.Equals(id))
                .ConfigureAwait(false);

            config.ConfigContainers = GenericTreeHelper.GetTree<Config, ConfigContainer>(config, genericTreeHelperArgs);

            return config;
        }

        public async Task<Config> GetConfigAsync(string name)
        {
            var config = await applicationDbContext.Configs
                .AsNoTracking()
                .Include(c => c.Flow)
                .Include(c => c.ConfigContainers)
                .Include(c => c.ConfigSearchItems)
                .Include(c => c.ConfigItems)
                .ThenInclude(ci => ci.ConfigContainer)
                .FirstAsync(c => c.Name.Equals(name))
                .ConfigureAwait(false);

            config.ConfigContainers = GenericTreeHelper.GetTree<Config, ConfigContainer>(config, genericTreeHelperArgs);

            return config;
        }

        public async Task<Config> AddConfigAsync(Config config)
        {
            if (config.Flow != null)
            {
                var flow = await applicationDbContext.Flows
                    .FirstAsync(f => f.FlowId.Equals(config.Flow.FlowId))
                    .ConfigureAwait(false);

                config.FlowId = flow.FlowId;
                config.Flow = flow;
            }

            config.ConfigContainers = GenericTreeHelper.GetFlattenedTree<Config, ConfigContainer>(config, genericTreeHelperArgs);

            await applicationDbContext.Configs
                .AddAsync(config)
                .ConfigureAwait(false);

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            config.ConfigContainers = GenericTreeHelper.GetTree<Config, ConfigContainer>(config, genericTreeHelperArgs);

            return config;
        }

        public async Task<Config> UpdateConfigAsync(Config config)
        {
            var existing = await applicationDbContext.Configs
                .Include(c => c.Flow)
                .Include(c => c.ConfigItems)
                .Include(c => c.ConfigSearchItems)
                .Include(c => c.ConfigContainers)
                .FirstOrDefaultAsync(c => c.ConfigId.Equals(config.ConfigId))
                .ConfigureAwait(false);

            if (existing == null)
            {
                throw new NullReferenceException(
                    $"{nameof(config)} ConfigId {config.ConfigId} not found.");
            }
            else
            {
                if (config.Flow == null)
                {
                    existing.FlowId = null;
                    existing.Flow = null;
                }
                else if (config.Flow != null)
                {
                    var flow = await applicationDbContext.Flows
                            .FirstAsync(f => f.FlowId.Equals(config.Flow.FlowId))
                            .ConfigureAwait(false);

                    config.FlowId = flow.FlowId;
                }

                applicationDbContext
                    .Entry(existing)
                    .CurrentValues.SetValues(config);

                var removeConfigItems = (from configItem in existing.ConfigItems
                                         where !config.ConfigItems.Any(i => i.ConfigItemId.Equals(configItem.ConfigItemId))
                                         select configItem)
                                         .ToList();

                applicationDbContext.RemoveRange(removeConfigItems);

                foreach (var configItem in config.ConfigItems)
                {
                    ConfigItem existingConfigItem = null;

                    if (configItem.ConfigItemId > 0)
                    {
                        existingConfigItem = existing.ConfigItems
                            .FirstOrDefault(ci => ci.ConfigItemId.Equals(configItem.ConfigItemId));
                    }

                    if (existingConfigItem == null)
                    {
                        existing.ConfigItems.Add(configItem);
                    }
                    else
                    {
                        applicationDbContext.Entry(existingConfigItem).CurrentValues.SetValues(configItem);
                    }
                }

                var removeConfigSearchItems = (from configSearchItem in existing.ConfigSearchItems
                                               where !config.ConfigSearchItems.Any(i => i.ConfigSearchItemId.Equals(configSearchItem.ConfigSearchItemId))
                                               select configSearchItem)
                                         .ToList();

                applicationDbContext.RemoveRange(removeConfigSearchItems);

                foreach (var configSearchItem in config.ConfigSearchItems)
                {
                    ConfigSearchItem existingConfigSearchItem = null;

                    if (configSearchItem.ConfigSearchItemId > 0)
                    {
                        existingConfigSearchItem = existing.ConfigSearchItems
                            .FirstOrDefault(ci => ci.ConfigSearchItemId.Equals(configSearchItem.ConfigSearchItemId));
                    }

                    if (existingConfigSearchItem == null)
                    {
                        existing.ConfigSearchItems.Add(configSearchItem);
                    }
                    else
                    {
                        applicationDbContext.Entry(existingConfigSearchItem).CurrentValues.SetValues(configSearchItem);
                    }
                }

                config.ConfigContainers =
                    GenericTreeHelper.GetFlattenedTree<Config, ConfigContainer>(config, genericTreeHelperArgs);

                var removeConfigContainers = (from configContainer in existing.ConfigContainers
                                              where !config.ConfigContainers.Any(c => c.Code.Equals(configContainer.Code))
                                              select configContainer)
                                                .ToList();

                applicationDbContext.RemoveRange(removeConfigContainers);

                foreach (var configContainer in config.ConfigContainers)
                {
                    ConfigContainer existingConfigContainer = null;

                    if (configContainer.ConfigContainerId > 0)
                    {
                        existingConfigContainer = existing.ConfigContainers
                            .FirstOrDefault(c => c.ConfigContainerId.Equals(configContainer.ConfigContainerId));
                    }

                    if (existingConfigContainer == null)
                    {
                        existing.ConfigContainers.Add(configContainer);
                    }
                    else
                    {
                        applicationDbContext.Entry(existingConfigContainer).CurrentValues.SetValues(configContainer);
                    }
                }
            }

            await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return config;
        }

        public async Task<int> DeleteConfigAsync(int configId)
        {
            var config = await applicationDbContext.Configs
                .FirstAsync(c => c.ConfigId.Equals(configId))
                .ConfigureAwait(false);

            applicationDbContext.Configs.Remove(config);

            return await applicationDbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }
    }
}

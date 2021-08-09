using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Core.Options;
using Headway.Repository.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class OptionsRepository : RepositoryBase, IOptionsRepository
    {
        private readonly Dictionary<string, IOptionItems> localOptionItems = new();

        public OptionsRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
            localOptionItems.Add(typeof(ControllerOptionItems).Name, new ControllerOptionItems());
        }

        public async Task<IEnumerable<OptionItem>> GetOptionItemsAsync(string optionsCode)
        {
            if (localOptionItems.ContainsKey(optionsCode))
            {
                return await localOptionItems[optionsCode].GetOptionItemsAsync();
            }

            // Get option items from the database...

            throw new NotImplementedException();
        }
    }
}

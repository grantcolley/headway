using Headway.Core.Interface;
using Headway.Core.Model;
using Headway.Repository.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Headway.Repository
{
    public class OptionsRepository : RepositoryBase, IOptionsRepository
    {
        public OptionsRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }

        public Task<IServiceResult<IEnumerable<OptionItem>>> GetOptionItemsAsync(string source)
        {
            throw new NotImplementedException();
        }
    }
}

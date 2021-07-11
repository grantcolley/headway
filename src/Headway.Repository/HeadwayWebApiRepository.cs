using Headway.Core.Interface;
using Headway.Repository.Data;

namespace Headway.Repository
{
    public class HeadwayWebApiRepository : RepositoryBase, IHeadwayWebApiRepository
    {
        public HeadwayWebApiRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace Headway.Database.OptionsBuilder
{
    public interface IHeadwayDbContextOptionsBuilderFactory
    {
        IHeadwayDbContextOptionsBuilder<T> GetOptionsBuilder<T>(string databaseType) where T : DbContext;
    }
}

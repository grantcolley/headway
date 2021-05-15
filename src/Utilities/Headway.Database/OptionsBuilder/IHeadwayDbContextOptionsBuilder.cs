using Microsoft.EntityFrameworkCore;

namespace Headway.Database.OptionsBuilder
{
    public interface IHeadwayDbContextOptionsBuilder<T> where T : DbContext
    {
        string DataSource { get; }
        DbContextOptions<T> GetOptions();
    }
}

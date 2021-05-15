using Microsoft.EntityFrameworkCore;

namespace Headway.Database.OptionsBuilder
{
    public class SQLServerOptionsBuilder<T> : IHeadwayDbContextOptionsBuilder<T> where T : DbContext
    {
        public SQLServerOptionsBuilder(string database)
        {
            DataSource = $@"Server=(localdb)\mssqllocaldb;Database={database};Trusted_Connection=True;";
        }

        public string DataSource { get; private set; }

        public DbContextOptions<T> GetOptions()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<T>();
            dbContextOptionsBuilder.UseSqlServer(DataSource);
            return dbContextOptionsBuilder.Options;
        }
    }
}

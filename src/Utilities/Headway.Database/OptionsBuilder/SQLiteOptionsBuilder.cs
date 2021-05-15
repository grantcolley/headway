using Microsoft.EntityFrameworkCore;

namespace Headway.Database.OptionsBuilder
{
    public class SQLiteOptionsBuilder<T> : IHeadwayDbContextOptionsBuilder<T> where T : DbContext
    {
        public SQLiteOptionsBuilder(string database)
        {
            DataSource = $@"Data Source=..\..\..\db\{database}.db;";
        }

        public string DataSource { get; private set; }

        public DbContextOptions<T> GetOptions() 
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<T>();
            dbContextOptionsBuilder.UseSqlite(DataSource);
            return dbContextOptionsBuilder.Options;
        }
    }
}

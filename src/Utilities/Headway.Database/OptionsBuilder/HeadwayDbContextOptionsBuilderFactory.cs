using Headway.Database.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Headway.Database.OptionsBuilder
{
    public class HeadwayDbContextOptionsBuilderFactory: IHeadwayDbContextOptionsBuilderFactory
    {
        public IHeadwayDbContextOptionsBuilder<T> GetOptionsBuilder<T>(string databaseType) where T : DbContext
        {
            string database = typeof(T).Name switch
            {
                nameof(HeadwayDbContext) => "Headway.Identity",
                _ => throw new NotSupportedException(typeof(T).Name),
            };

            return databaseType.ToLower() switch
            {
                "sqlite" => new SQLiteOptionsBuilder<T>(database),
                "sqlserver" => new SQLServerOptionsBuilder<T>(database),
                _ => throw new NotSupportedException(databaseType.ToLower()),
            };
        }
    }
}

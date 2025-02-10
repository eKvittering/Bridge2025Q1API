using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Webminux.Optician.EntityFrameworkCore
{
    public static class OpticianDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<OpticianDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<OpticianDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}

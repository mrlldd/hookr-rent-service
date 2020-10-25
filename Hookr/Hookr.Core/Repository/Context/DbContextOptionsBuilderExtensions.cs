using System.Runtime.CompilerServices;
using Hookr.Core.Config.Database;
using Microsoft.EntityFrameworkCore;
[assembly: InternalsVisibleTo("Hookr.Migrator")]
namespace Hookr.Core.Repository.Context
{
    internal static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseHookrCoreConfig(this DbContextOptionsBuilder builder,
            IDatabaseConfig databaseConfig) 
            => builder
                .UseSqlServer(
                    databaseConfig.ConnectionString,
                    serverBuilder => serverBuilder
                        .MigrationsAssembly("Hookr.Migrations")
                        .EnableRetryOnFailure(databaseConfig.Retries)
                        .CommandTimeout(databaseConfig.Timeout)
                );
    }
}
using System;
using System.Reflection;
using Hookr.Core.Config.Database;
using Microsoft.EntityFrameworkCore;

namespace Hookr.Core.Repository.Context
{
    public static class DbContextOptionsBuilderExtensions
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
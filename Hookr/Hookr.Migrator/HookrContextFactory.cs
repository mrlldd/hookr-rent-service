using System;
using System.IO;
using System.Text.Json;
using Hookr.Core.Config;
using Hookr.Core.Repository.Context;
using Hookr.Core.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Hookr.Migrator
{
    public class HookrContextFactory : IDesignTimeDbContextFactory<HookrContext>
    {
        public HookrContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddUserSecrets<HookrContextFactory>()
                .AddEnvironmentVariables()
                .Build()
                .Map(root => new CoreApplicationConfig()
                    .SideEffect(root.Bind)
                );
            return new DbContextOptionsBuilder<HookrContext>()
                .UseHookrCoreConfig(config.Database)
                .Map(x => new HookrContext(x.Options));
        }
    }
}
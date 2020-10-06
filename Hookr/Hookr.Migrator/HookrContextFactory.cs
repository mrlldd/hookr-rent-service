using System;
using System.IO;
using System.Text.Json;
using Hookr.Core.Config;
using Hookr.Core.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Hookr.Migrator
{
    public class HookrContextFactory : IDesignTimeDbContextFactory<HookrContext>
    {
        public HookrContext CreateDbContext(string[] args)
        {
            Console.WriteLine("got");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddUserSecrets<HookrContextFactory>()
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<HookrContext>();
            var config = new CoreApplicationConfig();
            configuration.Bind(config);
            Console.WriteLine(JsonSerializer.Serialize(config));
            optionsBuilder
                .UseSqlServer(config.Database.ConnectionString);
            return new HookrContext(optionsBuilder.Options);
        }
    }
}
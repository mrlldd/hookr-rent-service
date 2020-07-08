using System;
using System.IO;
using HookrTelegramBot.Utilities.App.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;

namespace HookrTelegramBot.Repository.Context
{
    public class HookrContextFactory : IDesignTimeDbContextFactory<HookrContext>
    {
        public HookrContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile($"appsettings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<HookrContext>();
            var config = new AppSettings();
            configuration.Bind(config);
            Console.WriteLine(JsonConvert.SerializeObject(config));
            optionsBuilder.UseSqlServer(config.Database.ConnectionString);
            return new HookrContext(optionsBuilder.Options, null);
        }
    }
}
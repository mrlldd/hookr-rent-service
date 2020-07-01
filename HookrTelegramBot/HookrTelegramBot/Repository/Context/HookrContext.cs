using System;
using System.Threading;
using System.Threading.Tasks;
using HookrTelegramBot.Repository.Context.Entities.Base;
using HookrTelegramBot.Utilities.Extensions;
using HookrTelegramBot.Utilities.Telegram.Bot;
using Microsoft.EntityFrameworkCore;

namespace HookrTelegramBot.Repository.Context
{
    public class HookrContext : DbContext
    {
        private readonly IUserContextProvider userContextProvider;

        public HookrContext(DbContextOptions options, IUserContextProvider userContextProvider) : base(options) 
            => this.userContextProvider = userContextProvider;

        public DbSet<TelegramUser> TelegramUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<TelegramUser>(user =>
                {
                    user
                        .Property(x => x.State)
                        .HasDefaultValue(TelegramUserStates.Default);
                    user
                        .HasKey(x => x.Id);
                    user
                        .Property(x => x.Id)
                        .ValueGeneratedNever();
                });
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            OnPreSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            OnPreSaving();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnPreSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            OnPreSaving();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void OnPreSaving()
        {
            var entries = ChangeTracker.Entries();
            var messageSource = userContextProvider.Message.From;
            entries.ForEach(x =>
            {
                if (!(x.Entity is Entity entity))
                {
                    return;
                }

                var now = DateTime.Now;
                var userEntity = messageSource.ToDatabaseUser();
                userEntity.LastUpdatedAt = now;
                if (x.State == EntityState.Added)
                {
                    entity.CreatedAt = now;
                    entity.CreatedBy = userEntity;
                    return;
                }

                if (x.State == EntityState.Modified)
                {
                    entity.UpdatedAt = now;
                    entity.UpdatedBy = userEntity;
                }
            });
        }
    }
}
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using HookrTelegramBot.Repository.Context.Entities;
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

        public DbSet<Hookah> Hookahs { get; set; }

        public DbSet<Tobacco> Tobaccos { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderedTobacco> OrderedTobaccos { get; set; }

        public DbSet<OrderedHookah> OrderedHookahs { get; set; }

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
            modelBuilder
                .Entity<OrderedHookah>(hookah =>
                {
                    hookah
                        .HasOne(x => x.CreatedBy)
                        .WithMany()
                        .HasForeignKey(x => x.CreatedById)
                        .OnDelete(DeleteBehavior.NoAction);
                    hookah
                        .HasOne(x => x.UpdatedBy)
                        .WithMany()
                        .HasForeignKey(x => x.UpdatedById)
                        .OnDelete(DeleteBehavior.NoAction);
                    hookah
                        .HasKey(x => x.Id);
                    hookah
                        .Property(x => x.Id)
                        .ValueGeneratedOnAdd();

                    hookah
                        .HasOne(x => x.Product)
                        .WithMany()
                        .HasForeignKey(x => x.ProductId)
                        .OnDelete(DeleteBehavior.Restrict);
                    hookah
                        .HasOne(x => x.Order)
                        .WithMany(x => x.OrderedHookahs)
                        .HasForeignKey(x => x.OrderId)
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder
                .Entity<Tobacco>(tobacco =>
                {
                    tobacco
                        .HasOne(x => x.CreatedBy)
                        .WithMany()
                        .HasForeignKey(x => x.CreatedById)
                        .OnDelete(DeleteBehavior.NoAction);
                    tobacco
                        .HasOne(x => x.UpdatedBy)
                        .WithMany()
                        .HasForeignKey(x => x.UpdatedById)
                        .OnDelete(DeleteBehavior.NoAction);
                    tobacco
                        .HasKey(x => x.Id);
                    tobacco
                        .Property(x => x.Id)
                        .ValueGeneratedOnAdd();
                });

            modelBuilder
                .Entity<Hookah>(tobacco =>
                {
                    tobacco
                        .HasOne(x => x.CreatedBy)
                        .WithMany()
                        .HasForeignKey(x => x.CreatedById)
                        .OnDelete(DeleteBehavior.NoAction);
                    tobacco
                        .HasOne(x => x.UpdatedBy)
                        .WithMany()
                        .HasForeignKey(x => x.UpdatedById)
                        .OnDelete(DeleteBehavior.NoAction);
                    tobacco
                        .HasKey(x => x.Id);
                    tobacco
                        .Property(x => x.Id)
                        .ValueGeneratedOnAdd();
                });

            modelBuilder
                .Entity<OrderedTobacco>(orderedTobacco =>
                {
                    orderedTobacco
                        .HasOne(x => x.CreatedBy)
                        .WithMany()
                        .HasForeignKey(x => x.CreatedById)
                        .OnDelete(DeleteBehavior.NoAction);
                    orderedTobacco
                        .HasOne(x => x.UpdatedBy)
                        .WithMany()
                        .HasForeignKey(x => x.UpdatedById)
                        .OnDelete(DeleteBehavior.NoAction);
                    orderedTobacco
                        .HasKey(x => x.Id);
                    orderedTobacco
                        .Property(x => x.Id)
                        .ValueGeneratedOnAdd();
                    orderedTobacco
                        .HasOne(x => x.Product)
                        .WithMany()
                        .HasForeignKey(x => x.ProductId)
                        .OnDelete(DeleteBehavior.Restrict);
                    orderedTobacco
                        .HasOne(x => x.Order)
                        .WithMany(x => x.OrderedTobaccos)
                        .HasForeignKey(x => x.OrderId)
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder
                .Entity<Order>(order =>
                {
                    order
                        .HasOne(x => x.CreatedBy)
                        .WithMany()
                        .HasForeignKey(x => x.CreatedById)
                        .OnDelete(DeleteBehavior.NoAction);
                    order
                        .HasOne(x => x.UpdatedBy)
                        .WithMany()
                        .HasForeignKey(x => x.UpdatedById)
                        .OnDelete(DeleteBehavior.NoAction);
                    order
                        .HasKey(x => x.Id);
                    order
                        .Property(x => x.Id)
                        .ValueGeneratedOnAdd();
                    order
                        .Property(x => x.State)
                        .HasDefaultValue(OrderStates.Constructing);
                    order
                        .HasOne(x => x.DeletedBy)
                        .WithMany()
                        .HasForeignKey(x => x.DeletedById)
                        .OnDelete(DeleteBehavior.NoAction);
                });
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
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

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnPreSaving();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void OnPreSaving()
        {
            var entries = ChangeTracker.Entries();
            var messageSource = userContextProvider.Update.RealMessage.From;
            entries.ForEach(x =>
            {
                if (!(x.Entity is Entity entity))
                {
                    return;
                }

                var now = DateTime.Now;
                var userEntity = messageSource.ToDatabaseUser();
                userEntity.LastUpdatedAt = now;
                switch (x.State)
                {
                    case EntityState.Added:
                        entity.CreatedAt = now;
                        entity.CreatedBy = userContextProvider.DatabaseUser;
                        break;
                    case EntityState.Modified:
                        entity.UpdatedAt = now;
                        entity.UpdatedBy = userContextProvider.DatabaseUser;
                        break;
                    case EntityState.Deleted:
                        if (entity is ISoftDeletable softDeletable)
                        {
                            x.State = EntityState.Modified;
                            softDeletable.DeletedAt = now;
                            softDeletable.DeletedBy = userContextProvider.DatabaseUser;
                            softDeletable.IsDeleted = true;
                        }

                        break;
                }
            });
        }
    }
}
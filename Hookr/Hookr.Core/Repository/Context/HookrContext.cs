using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hookr.Core.Repository.Context.Entities;
using Hookr.Core.Repository.Context.Entities.Base;
using Hookr.Core.Repository.Context.Entities.Products;
using Hookr.Core.Repository.Context.Entities.Products.Ordered;
using Hookr.Core.Repository.Context.Entities.Products.Photo;
using Hookr.Core.Repository.Context.Entities.Translations;
using Hookr.Core.Repository.Context.Entities.Translations.Telegram;
using Hookr.Core.Utilities.Extensions;
using Hookr.Core.Utilities.Loaders;
using Hookr.Core.Utilities.Providers;
using Microsoft.EntityFrameworkCore;
using static System.Linq.Expressions.Expression;

namespace Hookr.Core.Repository.Context
{
    public class HookrContext : DbContext
    {

        public HookrContext(DbContextOptions options) : base(options)
        {
        }

        [NotNull] public DbSet<TelegramUser>? TelegramUsers { get; set; }

        [NotNull] public DbSet<Hookah>? Hookahs { get; set; }

        [NotNull] public DbSet<Tobacco>? Tobaccos { get; set; }

        [NotNull] public DbSet<Order>? Orders { get; set; }

        [NotNull] public DbSet<OrderedTobacco>? OrderedTobaccos { get; set; }

        [NotNull] public DbSet<OrderedHookah>? OrderedHookahs { get; set; }

        [NotNull] public DbSet<Translation<TelegramTranslationKeys>>? TelegramTranslations { get; set; }

        [NotNull] public DbSet<HookahPhoto>? HookahPhotos { get; set; }

        [NotNull] public DbSet<TobaccoPhoto>? TobaccoPhotos { get; set; }
#pragma warning disable 8602
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Model.GetEntityTypes()
                .Where(x => typeof(ISoftDeletable).IsAssignableFrom(x.ClrType))
                .ForEach(entityType =>
                {
                    var pe = Parameter(entityType.ClrType, "y");
                    var propertyInfo = entityType.ClrType.GetProperty(nameof(ISoftDeletable.IsDeleted));
                    var lambda =
                        Lambda(Not(Property(pe, propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo)))),
                            pe);
                    entityType.SetQueryFilter(lambda);
                });
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

            modelBuilder
                .Entity<Translation<TelegramTranslationKeys>>(translation =>
                {
                    translation
                        .HasKey(x => x.Id);
                    translation
                        .HasIndex(x => x.Language);
                });

            modelBuilder
                .Entity<HookahPhoto>(photo =>
                {
                    photo
                        .HasOne(x => x.CreatedBy)
                        .WithMany()
                        .HasForeignKey(x => x.CreatedById)
                        .OnDelete(DeleteBehavior.NoAction);
                    photo
                        .HasOne(x => x.UpdatedBy)
                        .WithMany()
                        .HasForeignKey(x => x.UpdatedById)
                        .OnDelete(DeleteBehavior.NoAction);
                    photo
                        .HasOne(x => x.DeletedBy)
                        .WithMany()
                        .HasForeignKey(x => x.DeletedById)
                        .OnDelete(DeleteBehavior.NoAction);
                    photo
                        .HasOne(x => x.Hookah)
                        .WithMany(x => x.Photos)
                        .HasForeignKey(x => x.HookahId)
                        .OnDelete(DeleteBehavior.Cascade);
                });
            modelBuilder
                .Entity<TobaccoPhoto>(photo =>
                {
                    photo
                        .HasOne(x => x.CreatedBy)
                        .WithMany()
                        .HasForeignKey(x => x.CreatedById)
                        .OnDelete(DeleteBehavior.NoAction);
                    photo
                        .HasOne(x => x.UpdatedBy)
                        .WithMany()
                        .HasForeignKey(x => x.UpdatedById)
                        .OnDelete(DeleteBehavior.NoAction);
                    photo
                        .HasOne(x => x.DeletedBy)
                        .WithMany()
                        .HasForeignKey(x => x.DeletedById)
                        .OnDelete(DeleteBehavior.NoAction);
                    photo
                        .HasOne(x => x.Tobacco)
                        .WithMany(x => x.Photos)
                        .HasForeignKey(x => x.TobaccoId)
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
#pragma warning restore

        public async Task<int> SaveChangesAsync(
            ITelegramUserIdProvider telegramUserIdProvider,
            CachingLoaderDispatcher cachingLoaderDispatcher,
            CancellationToken token = default)
        {
            await OnPreSavingAsync(telegramUserIdProvider, cachingLoaderDispatcher);
            return await base.SaveChangesAsync(token);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException("Use another overload with services in parameters.");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException("Use another overload with services in parameters.");
        }
        
        public override int SaveChanges()
        {
            throw new NotSupportedException("Async only.");
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            throw new NotSupportedException("Async only.");
        }

        private async Task OnPreSavingAsync(ITelegramUserIdProvider telegramUserIdProvider,
            CachingLoaderDispatcher cachingLoaderDispatcher)
        {
            var entries = ChangeTracker.Entries()
                .ToArray();
            if (!entries.Any())
            {
                return;
            }

            var user = await cachingLoaderDispatcher
                .GetOrLoadAsync<int, TelegramUser>(telegramUserIdProvider.ProvidedValue);
            entries
                .ForEach(x =>
                {
                    if (!(x.Entity is Entity entity))
                    {
                        return;
                    }

                    var now = DateTime.Now;
                    user.LastUpdatedAt = now;
                    switch (x.State)
                    {
                        case EntityState.Added:
                            entity.CreatedAt = now;
                            entity.CreatedBy = user;
                            break;
                        case EntityState.Modified:
                            entity.UpdatedAt = now;
                            entity.UpdatedBy = user;
                            break;
                        case EntityState.Deleted:
                            if (entity is ISoftDeletable softDeletable)
                            {
                                x.State = EntityState.Modified;
                                softDeletable.DeletedAt = now;
                                softDeletable.DeletedBy = user;
                                softDeletable.IsDeleted = true;
                            }

                            break;
                    }
                });
        }
    }
}
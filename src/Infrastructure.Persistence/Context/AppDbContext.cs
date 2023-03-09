using Core.Application.Contracts.Interfaces;
using Core.Domain.Persistence.Common;
using Core.Domain.Persistence.Entities;
using Infrastructure.Persistence.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context
{
    public class AppDbContext : IdentityDbContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUser _authenticatedUser;
        public AppDbContext(DbContextOptions<AppDbContext> options, IAuthenticatedUser authenticatedUser, IDateTimeService dateTime)
            : base(options)
        {
            _authenticatedUser = authenticatedUser;
            _dateTime = dateTime;
        }

        public DbSet<Brand> Brands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region Soft delete setup
            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(type.ClrType))
                    modelBuilder.SetSoftDeleteFilter(type.ClrType);
            }
            #endregion

            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> SaveChangesAsync()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreationDate = _dateTime.NowUtc;
                        entry.Entity.CreatedBy = _authenticatedUser.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastUpdatedDate = _dateTime.NowUtc;
                        entry.Entity.LastUpdatedBy = _authenticatedUser.UserId;
                        break;
                }
            }
            return await base.SaveChangesAsync();
        }
    }
}

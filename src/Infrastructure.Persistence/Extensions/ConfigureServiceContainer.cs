using Core.Domain.Persistence.Contracts;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Extensions
{
    public static class ConfigureServiceContainer
    {
        public static void AddPersistenceDbContext(this IServiceCollection services, IConfiguration configuration,string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            #region Identity setup
            services.AddIdentityCore<IdentityUser>(op =>
            {
                op.Password.RequiredLength = 5;
                op.Password.RequireLowercase = false;
                op.Password.RequireNonAlphanumeric = false;
                op.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<AppDbContext>();
            #endregion
        }

        public static void AddPersistenceRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddTransient<IPersistenceUnitOfWork, PersistenceUnitOfWork>();
        }
    }
}

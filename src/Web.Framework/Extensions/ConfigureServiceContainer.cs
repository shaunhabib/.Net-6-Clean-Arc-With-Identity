using AutoMapper;
using Core.Application.Contracts.Interfaces;
using Infrastructure.Persistence.Extensions;
using Infrastructure.Shared;
using Web.Framework.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Core.Application;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace Web.Framework.Extensions
{
    public static class ConfigureServiceContainer
    {
        public static void AddAutoMapper(this IServiceCollection serviceCollection)
        {
            var mappingConfig = new MapperConfiguration(cfg =>
                    cfg.AddMaps(new[] {
                        "Web.Framework",
                        "Core.Application",
                        "Core.Application.Contracts",
                        "Infrastructure.Persistence"
                    })
                );
            IMapper mapper = mappingConfig.CreateMapper();
            serviceCollection.AddSingleton(mapper);
        }

        public static void AddFramework(this IServiceCollection services, IConfiguration configuration,string connectionString)
        {

            services.AddPersistenceDbContext(configuration, connectionString);
            services.AddPersistenceRepositories();
            services.AddApplicationLayer();
            services.AddSharedInfrastructure();
            services.AddHttpContextAccessor();
            services.AddTransient<IAuthenticatedUser, AuthenticatedUser>();
            services.AddTransient<IDateTimeService, DateTimeService>();

        }
    }
}

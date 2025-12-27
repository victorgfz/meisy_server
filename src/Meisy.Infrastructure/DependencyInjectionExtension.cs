using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Company;
using Meisy.Domain.Repositories.User;
using Meisy.Domain.Security.Cryptography;
using Meisy.Domain.Security.Token;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Infrastructure.Data;
using Meisy.Infrastructure.Data.Repositories.Company;
using Meisy.Infrastructure.Data.Repositories.User;
using Meisy.Infrastructure.Security.Cryptography;
using Meisy.Infrastructure.Security.Token;
using Meisy.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Meisy.Infrastructure
{
        public static class DependencyInjectionExtension
        {

            public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
            {
                AddRepositories(services);
                AddDbContext(services, configuration);
                AddToken(services, configuration);
                 services.AddScoped<IUnitOfWork, UnitOfWork>();
                 services.AddScoped<IPasswordEncrypter, Bcrypt>();
                 services.AddScoped<ILoggedUser, LoggedUser>();

            }

            private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("Connection");
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 44));

            services.AddDbContext<MeisyDbContext>(config => config.UseMySql(connectionString, serverVersion));
        }

            private static void AddRepositories(IServiceCollection services)
            {
                services.AddScoped<IUserReadRepository, UserRepository>();
                services.AddScoped<IUserWriteRepository, UserRepository>();
                services.AddScoped<ICompanyReadRepository, CompanyRepository>();
                services.AddScoped<ICompanyWriteRepository, CompanyRepository>();
            }

            private static void AddToken(IServiceCollection services, IConfiguration configuration)
            {
            var expirationTimeMinutes = uint.Parse(configuration["Settings:Jwt:ExpiresMinutes"]!);
            var signingKey = configuration["Settings:Jwt:SigningKey"];

            services.AddScoped<ITokenGenerator>(config => new TokenGenerator(expirationTimeMinutes, signingKey!));

            }

    }
}

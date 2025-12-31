using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Company;
using Meisy.Domain.Repositories.Input;
using Meisy.Domain.Repositories.User;
using Meisy.Domain.Security.Cryptography;
using Meisy.Domain.Security.Token;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Infrastructure.Data;
using Meisy.Infrastructure.Data.Repositories.Company;
using Meisy.Infrastructure.Data.Repositories.Input;
using Meisy.Infrastructure.Data.Repositories.User;
using Meisy.Infrastructure.Data.Repositories.Overhead;
using Meisy.Infrastructure.Security.Cryptography;
using Meisy.Infrastructure.Security.Token;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Meisy.Domain.Repositories.Overhead;
using Meisy.Domain.Repositories.Products;
using Meisy.Infrastructure.Data.Repositories.Product;

namespace Meisy.Infrastructure
{
        public static class DependencyInjectionExtension
        {

            public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
            {
                AddRepositories(services);
                AddDbContext(services, configuration);
                AddToken(services, configuration);
            //
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
                services.AddScoped<IInputReadOnlyRepository, InputRepository>();
                services.AddScoped<IInputWriteOnlyRepository, InputRepository>();
                services.AddScoped<IOverheadReadOnlyRepository, OverheadRepository>();
                services.AddScoped<IOverheadWriteOnlyRepository, OverheadRepository>();
                services.AddScoped<IProductReadOnlyRepository, ProductRepository>();
                services.AddScoped<IProductWriteOnlyRepository, ProductRepository>();
        }

            private static void AddToken(IServiceCollection services, IConfiguration configuration)
            {
            var expirationTimeMinutes = uint.Parse(configuration["Settings:Jwt:ExpiresMinutes"]!);
            var signingKey = configuration["Settings:Jwt:SigningKey"];

            services.AddScoped<ITokenGenerator>(config => new TokenGenerator(expirationTimeMinutes, signingKey!));

            }

    }
}

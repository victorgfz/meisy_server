using Meisy.Application.AutoMapper;
using Meisy.Application.UseCases.Auth.Login;
using Meisy.Application.UseCases.Auth.Register;
using Microsoft.Extensions.DependencyInjection;

namespace Meisy.Application
{
    public static class DependencyInjectionExtension
    {

        public static void AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapping));
            AddUseCases(services);
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<ILoginUseCase, LoginUseCase>();
        }
    }
}

using Meisy.Application.AutoMapper;
using Meisy.Application.UseCases.Auth.Login;
using Meisy.Application.UseCases.Auth.Register;
using Meisy.Application.UseCases.Clients.Delete;
using Meisy.Application.UseCases.Clients.GetAll;
using Meisy.Application.UseCases.Clients.Register;
using Meisy.Application.UseCases.Clients.Update;
using Meisy.Application.UseCases.Inputs.Delete;
using Meisy.Application.UseCases.Inputs.GetAll;
using Meisy.Application.UseCases.Inputs.Register;
using Meisy.Application.UseCases.Inputs.Update;
using Meisy.Application.UseCases.Orders.GetAll;
using Meisy.Application.UseCases.Orders.Register;
using Meisy.Application.UseCases.Orders.Update;
using Meisy.Application.UseCases.Overheads.GetAll;
using Meisy.Application.UseCases.Overheads.Register;
using Meisy.Application.UseCases.Overheads.Update;
using Meisy.Application.UseCases.Products.Delete;
using Meisy.Application.UseCases.Products.Get;
using Meisy.Application.UseCases.Products.GetAll;
using Meisy.Application.UseCases.Products.Register;
using Meisy.Application.UseCases.Products.Update;
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

            services.AddScoped<IRegisterInputUseCase, RegisterInputUseCase>();
            services.AddScoped<IGetAllInputUseCase, GetAllInputUseCase>();
            services.AddScoped<IUpdateInputUseCase, UpdateInputUseCase>();
            services.AddScoped<IDeleteInputUseCase, DeleteInputUseCase>();

            services.AddScoped<IRegisterOverheadUseCase, RegisterOverheadUseCase>();
            services.AddScoped<IGetAllOverheadUseCase, GetAllOverheadUseCase>();
            services.AddScoped<IUpdateOverheadUseCase, UpdateOverheadUseCase>();

            services.AddScoped<IRegisterProductUseCase, RegisterProductUseCase>();
            services.AddScoped<IGetAllProductUseCase, GetAllProductUseCase>();
            services.AddScoped<IGetProductUseCase, GetProductUseCase>();
            services.AddScoped<IUpdateProductUseCase, UpdateProductUseCase>();
            services.AddScoped<IDeleteProductUseCase, DeleteProductUseCase>();

            services.AddScoped<IRegisterClientUseCase, RegisterClientUseCase>();
            services.AddScoped<IGetAllClientUseCase, GetAllClientUseCase>();
            services.AddScoped<IUpdateClientUseCase, UpdateClientUseCase>();
            services.AddScoped<IDeleteClientUseCase, DeleteClientUseCase>();

            services.AddScoped<IRegisterOrderUseCase, RegisterOrderUseCase>();
            services.AddScoped<IGetAllOrderUseCase, GetAllOrderUseCase>();
            services.AddScoped<IUpdateOrderStatusUseCase, UpdateOrderStatusUseCase>();
            services.AddScoped<ICancelOrderStatusUseCase, CancelOrderStatusUseCase>();


        }
    }
}

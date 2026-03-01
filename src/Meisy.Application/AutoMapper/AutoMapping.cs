using AutoMapper;
using Meisy.Communication.Requests.Auth;
using Meisy.Communication.Requests.Clients;
using Meisy.Communication.Requests.Inputs;
using Meisy.Communication.Requests.Orders;
using Meisy.Communication.Requests.Overheads;
using Meisy.Communication.Requests.Products;
using Meisy.Communication.Responses.Clients;
using Meisy.Communication.Responses.Inputs;
using Meisy.Communication.Responses.Orders;
using Meisy.Communication.Responses.Overheads;
using Meisy.Communication.Responses.Products;
using Meisy.Domain.Entities;
using System.Text.RegularExpressions;

namespace Meisy.Application.AutoMapper
{
    public class AutoMapping : Profile
    {

        public AutoMapping()
        {
            RequestToEntity();
            EntityToResponse();
        }


        private void RequestToEntity()
        {
            CreateMap<RequestRegisterUserJson, User>()
                .ForMember(destination => destination.Password, config => config.Ignore())
                .ForMember(destination => destination.CreatedAt, config => config.MapFrom(_=> DateTime.UtcNow))
                .ForMember(destination => destination.UpdatedAt, config => config.MapFrom(_=> DateTime.UtcNow));

            CreateMap<RequestRegisterInputJson, Input>();
            CreateMap<RequestUpdateInputJson, Input>();

            CreateMap<RequestRegisterOverheadJson, Overhead>();
            CreateMap<RequestUpdateOverheadJson, Overhead>();

            CreateMap<RequestRegisterProductJson, Product>();
            CreateMap<RequestRegisterProductInputJson, ProductInput>();
            CreateMap<RequestUpdateProductJson, Product>();

            CreateMap<RequestClientJson, Client>()
                .ForMember(destination => destination.Phone, config => config.MapFrom(item => string.IsNullOrWhiteSpace(item.Phone) ? null : Regex.Replace(item.Phone, @"\D", "")));

            CreateMap<RequestRegisterOrderJson, Order>();
            CreateMap<RequestRegisterOrderProductJson, OrderProduct>();
        }

        private void EntityToResponse()
        {
            CreateMap<Input, ResponseInputJson>();
            CreateMap<Overhead, ResponseOverheadJson>();
            CreateMap<Product, ResponseProductJson>();

            CreateMap<Product, ResponseDetailedProductJson>()
                .ForMember(destination => destination.ProductInputs, opt => opt.Ignore())
                .ForMember(destination => destination.ProductOverheads, opt => opt.Ignore());

            CreateMap<Client, ResponseClientJson>();

            CreateMap<Order, ResponseOrderJson>();
            CreateMap<Order, ResponseDetailedOrderJson>()
                .ForMember(destination => destination.OrderProducts, opt => opt.Ignore())
                .ForMember(destination => destination.Client, opt=>opt.Ignore())
                .ForMember(destination => destination.Seller, opt=>opt.Ignore());

            CreateMap<Client, ResponseOrderUserJson>();
            CreateMap<User, ResponseOrderUserJson>();

        }
    }
}

using AutoMapper;
using Meisy.Communication.Responses.Orders;
using Meisy.Domain.Entities;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Client;
using Meisy.Domain.Repositories.Order;
using Meisy.Domain.Repositories.Product;
using Meisy.Domain.Repositories.User;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Orders.GetAll
{
    public class GetAllOrderUseCase : IGetAllOrderUseCase
    {

        private readonly ILoggedUser _loggedUser;
        private readonly IOrderReadOnlyRepository _orderReadRepository;
        private readonly IClientReadOnlyRepository _clientReadRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IMapper _mapper;

        public GetAllOrderUseCase(
            ILoggedUser loggedUser,
            IOrderReadOnlyRepository orderReadRepository,
            IClientReadOnlyRepository clientReadRepository,
            IUserReadRepository userReadRepository,
            IMapper mapper
            )
        {
            _loggedUser = loggedUser;
            _orderReadRepository = orderReadRepository;
            _mapper = mapper;
            _clientReadRepository = clientReadRepository;
            _userReadRepository = userReadRepository;
        }

        public async Task<List<ResponseDetailedOrderJson>> Execute()
        {
            var companyId = _loggedUser.GetCompanyId();
            var orders = await _orderReadRepository.GetAll(companyId);
            var result = _mapper.Map<List<ResponseDetailedOrderJson>>(orders);

           for(var i =0; i < orders.Count; i++)
            {
                var seller = await _userReadRepository.GetById(companyId, orders[i].SellerId) ?? throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);
                var clientId = orders[i].ClientId;

                var client = clientId is null ? null : (await _clientReadRepository.GetById(companyId, clientId.Value) ??
                    throw new NotFoundException(ResourceErrorMessages.CLIENT_NOT_FOUND));
                result[i].Client = client is null ? null :  _mapper.Map<ResponseOrderUserJson>(client);
                result[i].Seller = _mapper.Map<ResponseOrderUserJson>(seller);

                List<ResponseOrderProductJson> products = [];
                foreach (var item in orders[i].OrderProducts)
                {
                    products.Add(new ResponseOrderProductJson
                    {   Id = item.Product.Id,
                        Description = item.Product.Description,
                        Amount = item.Amount,
                        PriceAtTheMoment = item.PriceAtTheMoment,
                    });
                }

                result[i].OrderProducts = products; 
            }
            return result;
        }


    }
}

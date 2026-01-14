using AutoMapper;
using Meisy.Communication.Responses.Products;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Input;
using Meisy.Domain.Repositories.Products;
using Meisy.Domain.Services.LoggedUser;

namespace Meisy.Application.UseCases.Products.GetAll
{
    public class GetAllProductUseCase : IGetAllProductUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IProductReadOnlyRepository _productReadRepository;
        private readonly IMapper _mapper;

        public GetAllProductUseCase(
            ILoggedUser loggedUser,
            IProductReadOnlyRepository productReadRepository,
            IMapper mapper
            )
        {
            _loggedUser = loggedUser;
            _productReadRepository = productReadRepository;
            _mapper = mapper;
        }

        public async Task<List<ResponseProductJson>> Execute()
        {
            var companyId = _loggedUser.GetCompanyId();
            var products = await _productReadRepository.GetAll(companyId);
            return _mapper.Map<List<ResponseProductJson>>(products);
        }
    }
}

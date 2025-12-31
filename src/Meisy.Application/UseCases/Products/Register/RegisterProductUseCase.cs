using AutoMapper;
using Meisy.Communication.Requests;
using Meisy.Communication.Responses;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Products;
using Meisy.Domain.Services.LoggedUser;

namespace Meisy.Application.UseCases.Products.Register
{
    public class RegisterProductUseCase : IRegisterProductUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IProductWriteOnlyRepository _productWriteRepository;
        private readonly IProductReadOnlyRepository _productReadRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterProductUseCase(
            ILoggedUser loggedUser,
            IProductWriteOnlyRepository productWriteRepository,
            IProductReadOnlyRepository productReadRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            _loggedUser = loggedUser;
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }

        public async Task<ResponseProductJson> Execute(RequestRegisterProductJson request)
        {
            throw new NotImplementedException();
        }
    }
}

using AutoMapper;
using Meisy.Communication.Requests.Products;
using Meisy.Communication.Responses.Products;
using Meisy.Domain.Entities;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Input;
using Meisy.Domain.Repositories.Product;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Products.Register
{
    public class RegisterProductUseCase : IRegisterProductUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IProductWriteOnlyRepository _productWriteRepository;
        private readonly IInputReadOnlyRepository _inputReadRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterProductUseCase(
            ILoggedUser loggedUser,
            IProductWriteOnlyRepository productWriteRepository,
            IInputReadOnlyRepository inputReadRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            _loggedUser = loggedUser;
            _productWriteRepository = productWriteRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _inputReadRepository = inputReadRepository;
        }

        public async Task<ResponseProductJson> Execute(RequestRegisterProductJson request)
        {
            var companyId= _loggedUser.GetCompanyId();
            Validate(request);


            var entityProduct = _mapper.Map<Product>(request);
            entityProduct.ProductInputs = _mapper.Map<List<ProductInput>>(request.ProductInputs);
            entityProduct.CompanyId = companyId;

            foreach(var item in entityProduct.ProductInputs)
            {
                var input = await _inputReadRepository.GetById(companyId, item.InputId) ?? throw new NotFoundException(ResourceErrorMessages.INPUT_NOT_FOUND);
                
                item.CompanyId = companyId;
                item.ProductionAmount = item.ProductionAmount / entityProduct.Servings;
            }


            await _productWriteRepository.Add(entityProduct);
            await _unitOfWork.Commit();

            return _mapper.Map<ResponseProductJson>(entityProduct);
        }


        private void Validate(RequestRegisterProductJson request)
        {
            var result = new RegisterProductValidator().Validate(request);
            
            if(!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errors);
            }
        }
    }
}

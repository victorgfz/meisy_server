using AutoMapper;
using Meisy.Application.UseCases.Products.Register;
using Meisy.Communication.Requests.Products;
using Meisy.Domain.Entities;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Input;
using Meisy.Domain.Repositories.Product;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Products.Update
{
    public class UpdateProductUseCase : IUpdateProductUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IProductWriteOnlyRepository _productWriteRepository;
        private readonly IProductReadOnlyRepository _productReadRepository;
        private readonly IInputReadOnlyRepository _inputReadRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductUseCase(
            ILoggedUser loggedUser,
            IProductWriteOnlyRepository productWriteRepository,
            IProductReadOnlyRepository productReadRepository,
            IInputReadOnlyRepository inputReadRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            _loggedUser = loggedUser;
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _inputReadRepository = inputReadRepository;
        }
        public async Task Execute(RequestUpdateProductJson request, int id)
        {
            var companyId = _loggedUser.GetCompanyId();
            Validate(request);


            var product = await _productReadRepository.GetByIdForUpdate(companyId, id);
            if(product is null)
            {
                throw new NotFoundException(ResourceErrorMessages.PRODUCT_NOT_FOUND);
            }


            foreach (var item in request.ProductInputs)
            {
                var input = await _inputReadRepository.GetById(companyId, item.InputId);
                if (input is null)
                {
                    throw new NotFoundException(ResourceErrorMessages.INPUT_NOT_FOUND);
                }

                item.ProductionAmount = item.ProductionAmount / request.Servings;

            }


            product.ProductInputs.Where(item => request.ProductInputs.Any(r => r.InputId == item.InputId)).ToList();
            

            _mapper.Map(request, product);

            foreach(var item in product.ProductInputs)
            {

                item.ProductId = product.Id;
                item.CompanyId = companyId;
            }

            await _unitOfWork.Commit();

        }

        private void Validate(RequestUpdateProductJson request)
        {
            var result = new UpdateProductValidator().Validate(request);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errors);
            }
        }

        
    }
}

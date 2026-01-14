using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Products;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Products.Delete
{
    public class DeleteProductUseCase : IDeleteProductUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IProductWriteOnlyRepository _productWriteRepository;
        private readonly IProductReadOnlyRepository _productReadRepository;

        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductUseCase(
            ILoggedUser loggedUser,
            IProductWriteOnlyRepository productWriteRepository,
            IProductReadOnlyRepository productReadRepository,
            IUnitOfWork unitOfWork
            )
        {
            _loggedUser = loggedUser;
            _productWriteRepository = productWriteRepository;
           _productReadRepository = productReadRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task Execute(int id)
        {
            var companyId = _loggedUser.GetCompanyId();
            var product = await _productReadRepository.GetByIdForUpdate(companyId, id) ?? throw new NotFoundException(ResourceErrorMessages.PRODUCT_NOT_FOUND);
            _productWriteRepository.Delete(product);
            await _unitOfWork.Commit();

        }
    }
}

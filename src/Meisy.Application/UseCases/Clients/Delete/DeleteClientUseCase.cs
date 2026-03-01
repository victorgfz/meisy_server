using AutoMapper;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Client;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Clients.Delete
{
    public class DeleteClientUseCase : IDeleteClientUseCase
    {

        private readonly ILoggedUser _loggedUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientWriteOnlyRepository _clientWriteRepository;
        private readonly IClientReadOnlyRepository _clientReadRepository;

        public DeleteClientUseCase(
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork,
            IClientWriteOnlyRepository clientWriteRepository,
            IClientReadOnlyRepository clientReadRepository
            )
        {
            _loggedUser = loggedUser;
            _unitOfWork = unitOfWork;
            _clientWriteRepository = clientWriteRepository;
            _clientReadRepository = clientReadRepository;

        }

        public async Task Execute(int id)
        {
            var companyId = _loggedUser.GetCompanyId();
             var client = await _clientReadRepository.GetByIdForUpdate(companyId, id) ?? throw new NotFoundException(ResourceErrorMessages.CLIENT_NOT_FOUND);
            _clientWriteRepository.Delete(client);

            await _unitOfWork.Commit();
        }
    }
}

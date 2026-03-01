using AutoMapper;
using Meisy.Communication.Requests.Clients;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Client;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Clients.Update
{
    public class UpdateClientUseCase : IUpdateClientUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientReadOnlyRepository _clientReadRepository;

        public UpdateClientUseCase(
            ILoggedUser loggedUser,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IClientReadOnlyRepository clientReadRepository
            )
        {
            _loggedUser = loggedUser;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _clientReadRepository = clientReadRepository;

        }




        public async Task Execute(RequestClientJson request, int id)
        {
            var companyId = _loggedUser.GetCompanyId();
            Validate(request);
            var client = await _clientReadRepository.GetByIdForUpdate(companyId, id) ?? throw new NotFoundException(ResourceErrorMessages.CLIENT_NOT_FOUND);
            _mapper.Map(request, client);
            await _unitOfWork.Commit();
        }

        private void Validate(RequestClientJson request)
        {
            var result = new ClientValidator().Validate(request);
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errors);
            }
        }
    }
}

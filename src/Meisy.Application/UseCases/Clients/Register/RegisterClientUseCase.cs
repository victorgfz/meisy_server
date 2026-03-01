using AutoMapper;
using Meisy.Communication.Requests.Clients;
using Meisy.Communication.Responses.Clients;
using Meisy.Domain.Entities;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Client;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Clients.Register
{
    public class RegisterClientUseCase : IRegisterClientUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientWriteOnlyRepository _clientWriteRepository;

        public RegisterClientUseCase(
            ILoggedUser loggedUser,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IClientWriteOnlyRepository clientWriteRepository
            )
        {
            _loggedUser = loggedUser;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _clientWriteRepository = clientWriteRepository;
            
        }


        public async Task<ResponseClientJson> Execute(RequestClientJson request)
        {
            var companyId = _loggedUser.GetCompanyId();
            Validate(request);

            var entityClient = _mapper.Map<Client>(request);
            entityClient.CompanyId = companyId;

            await _clientWriteRepository.Add(entityClient);
            await _unitOfWork.Commit();

            return _mapper.Map<ResponseClientJson>(entityClient);


        }

        private void Validate(RequestClientJson request)
        {
            var result = new ClientValidator().Validate(request);
            if(!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errors);
            }
        }
    }
}

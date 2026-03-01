using AutoMapper;
using Meisy.Communication.Responses.Clients;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Client;
using Meisy.Domain.Services.LoggedUser;

namespace Meisy.Application.UseCases.Clients.GetAll
{
    public class GetAllClientUseCase : IGetAllClientUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClientReadOnlyRepository _clientReadRepository;

        public GetAllClientUseCase(
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


        public async Task<List<ResponseClientJson>> Execute()
        {
            var companyId = _loggedUser.GetCompanyId();
            var clients = await _clientReadRepository.GetAll(companyId);
            return _mapper.Map<List<ResponseClientJson>>( clients );
        }
    }
}

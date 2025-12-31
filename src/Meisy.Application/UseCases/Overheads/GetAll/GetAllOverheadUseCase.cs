using AutoMapper;
using Meisy.Communication.Responses;
using Meisy.Domain.Repositories.Input;
using Meisy.Domain.Repositories.Overhead;
using Meisy.Domain.Services.LoggedUser;

namespace Meisy.Application.UseCases.Overheads.GetAll
{
    public class GetAllOverheadUseCase : IGetAllOverheadUseCase
    {

        private readonly ILoggedUser _loggedUser;
        private readonly IOverheadReadOnlyRepository _overheadReadRepository;
        private readonly IMapper _mapper;

        public GetAllOverheadUseCase(
            ILoggedUser loggedUser,
            IOverheadReadOnlyRepository overheadReadRepository,
            IMapper mapper)
        {

            _loggedUser = loggedUser;
            _overheadReadRepository = overheadReadRepository;
            _mapper = mapper;
        }

        public async Task<List<ResponseOverheadJson>> Execute()
        {
            var companyId = _loggedUser.GetCompanyId();
            var overheads = await _overheadReadRepository.GetAll(companyId);
            return _mapper.Map<List<ResponseOverheadJson>>(overheads);
        }
    }
}

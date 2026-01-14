using AutoMapper;
using Meisy.Communication.Responses.Inputs;
using Meisy.Domain.Repositories.Input;
using Meisy.Domain.Services.LoggedUser;

namespace Meisy.Application.UseCases.Inputs.GetAll
{
    public class GetAllInputUseCase : IGetAllInputUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IInputReadOnlyRepository _inputReadRepository;
        private readonly IMapper _mapper;

        public GetAllInputUseCase(
            ILoggedUser loggedUser,
            IInputReadOnlyRepository inputReadRepository,
            IMapper mapper)
        {
            
            _loggedUser = loggedUser;
            _inputReadRepository = inputReadRepository;
            _mapper = mapper;
        }



        public async Task<List<ResponseInputJson>> Execute(Communication.Enums.InputType type)
        {
            var companyId = _loggedUser.GetCompanyId();
            
            var result = await _inputReadRepository.GetAllByType(companyId, (Domain.Enums.InputType)type);
            return _mapper.Map<List<ResponseInputJson>>(result);
        }
    }
}

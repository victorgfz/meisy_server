using AutoMapper;
using Meisy.Communication.Requests.Overheads;
using Meisy.Communication.Responses.Overheads;
using Meisy.Domain.Entities;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Input;
using Meisy.Domain.Repositories.Overhead;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Overheads.Register
{
    public class RegisterOverheadUseCase : IRegisterOverheadUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IOverheadWriteOnlyRepository _overheadWriteRepository;
        private readonly IOverheadReadOnlyRepository _overheadReadRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterOverheadUseCase(
            ILoggedUser loggedUser,
            IOverheadWriteOnlyRepository overheadWriteRepository,
            IOverheadReadOnlyRepository overheadReadRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            _loggedUser = loggedUser;
            _overheadWriteRepository = overheadWriteRepository;
            _overheadReadRepository = overheadReadRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            
        }

        public async Task<ResponseOverheadJson> Execute(RequestRegisterOverheadJson request)
        {
            var companyId = _loggedUser.GetCompanyId();
            Validate(request);
            var overheadExists = await _overheadReadRepository.OverheadExists((Domain.Enums.OverheadType)request.Type, companyId);
            if (overheadExists)
            {
                throw new OverheadLimitExceededException(ResourceErrorMessages.OVERHEAD_LIMIT_EXCEEDED);
            }
            var entityOverhead = _mapper.Map<Overhead>(request);
            entityOverhead.CompanyId = companyId;
            await _overheadWriteRepository.Add(entityOverhead);
            await _unitOfWork.Commit();
            return _mapper.Map<ResponseOverheadJson>(entityOverhead);

        }

        private void Validate(RequestRegisterOverheadJson request)
        {
            var result = new RegisterOverheadValidator().Validate(request);
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errors);
            }
        }
    }
}


using AutoMapper;
using Meisy.Communication.Requests.Inputs;
using Meisy.Communication.Responses.Inputs;
using Meisy.Domain.Entities;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Input;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Inputs.Register
{
    public class RegisterInputUseCase : IRegisterInputUseCase
    {

        private readonly ILoggedUser _loggedUser;
        private readonly IInputWriteOnlyRepository _inputWriteRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterInputUseCase(
            ILoggedUser loggedUser,
            IInputWriteOnlyRepository inputWriteRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            _inputWriteRepository = inputWriteRepository;
            _loggedUser = loggedUser;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseInputJson> Execute(RequestRegisterInputJson request)
        {
            var companyId = _loggedUser.GetCompanyId();
            
            Validate(request);

            var entityInput = _mapper.Map<Input>(request);
            entityInput.CompanyId = companyId;

             await _inputWriteRepository.Add(entityInput);
            await _unitOfWork.Commit();

            return _mapper.Map<ResponseInputJson>(entityInput);
            
        }

        private void Validate(RequestRegisterInputJson request)
        {
            var result = new RegisterInputValidator().Validate(request); ;
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errors);
            }
        }
    }
}

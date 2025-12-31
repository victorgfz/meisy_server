using AutoMapper;
using Meisy.Communication.Requests;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Input;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Inputs.Update
{
    public class UpdateInputUseCase : IUpdateInputUseCase
    {
        private readonly IInputReadOnlyRepository _inputReadRepository;
        private readonly IInputWriteOnlyRepository _inputWriteRepository;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateInputUseCase(
            IInputReadOnlyRepository inputReadRepository,
            IInputWriteOnlyRepository inputWriteRepository,
            IMapper mapper,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork
            )
        {
            _inputReadRepository = inputReadRepository;
            _inputWriteRepository = inputWriteRepository;
            _mapper = mapper;
            _loggedUser = loggedUser;
            _unitOfWork = unitOfWork;
            
        }
        public async Task Execute(int id, RequestUpdateInputJson request)
        {
            var companyId = _loggedUser.GetCompanyId();
            Validate(request);
            var input = await _inputReadRepository.GetById(companyId, id);

            if(input is null)
            {
                throw new NotFoundException(ResourceErrorMessages.INPUT_NOT_FOUND);
            }
            _mapper.Map(request, input);
            _inputWriteRepository.Update(input);
            await _unitOfWork.Commit();
        }

        private void Validate(RequestUpdateInputJson request)
        {
            var result = new UpdateInputValidator().Validate(request);
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errors);
            }
        }

    }
}

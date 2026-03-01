
using AutoMapper;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Input;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Inputs.Delete
{
    public class DeleteInputUseCase : IDeleteInputUseCase
    {

        private readonly IInputReadOnlyRepository _inputReadRepository;
        private readonly IInputWriteOnlyRepository _inputWriteRepository;
        private readonly ILoggedUser _loggedUser;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteInputUseCase(
            IInputReadOnlyRepository inputReadRepository,
            IInputWriteOnlyRepository inputWriteRepository,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork
            )
        {
            _inputReadRepository = inputReadRepository;
            _inputWriteRepository = inputWriteRepository;
            _loggedUser = loggedUser;
            _unitOfWork = unitOfWork;

        }


        public async Task Execute(int id)
        {
            var companyId = _loggedUser.GetCompanyId();
            var input = await _inputReadRepository.GetById(companyId, id) ?? throw new NotFoundException(ResourceErrorMessages.INPUT_NOT_FOUND);

            var isInputBeingUsed = await _inputReadRepository.IsInputBeingUsed(companyId, id);

            if (isInputBeingUsed)
            {
                throw new BusinessRuleException(ResourceErrorMessages.INPUT_BEING_USED);
            }

            _inputWriteRepository.Delete(input);
            await _unitOfWork.Commit();

        }
    }
}

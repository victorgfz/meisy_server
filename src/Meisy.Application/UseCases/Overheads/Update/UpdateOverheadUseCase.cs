using AutoMapper;
using Meisy.Communication.Requests.Overheads;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Overhead;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Overheads.Update
{
    public class UpdateOverheadUseCase : IUpdateOverheadUseCase
    {

        private readonly ILoggedUser _loggedUser;
        private readonly IOverheadReadOnlyRepository _overheadReadRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateOverheadUseCase(
            ILoggedUser loggedUser,
            IOverheadReadOnlyRepository overheadReadRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork
            )
        {
            _loggedUser = loggedUser;
            _overheadReadRepository = overheadReadRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        public async Task Execute(RequestUpdateOverheadJson request, int id)
        {
            var companyId = _loggedUser.GetCompanyId();

            Validate(request);

            var overhead = await _overheadReadRepository.GetById(companyId, id) ?? throw new NotFoundException(ResourceErrorMessages.OVERHEAD_NOT_FOUND);
            
            _mapper.Map(request, overhead);
            await _unitOfWork.Commit();
        }

        private void Validate(RequestUpdateOverheadJson request)
        {
            var result = new UpdateOverheadValidator().Validate(request);
            if(!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errors);
            }
        }
    }
}

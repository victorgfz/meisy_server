using AutoMapper;
using FluentValidation.Results;
using Meisy.Communication.Requests.Auth;
using Meisy.Communication.Responses.Auth;
using Meisy.Domain.Entities;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Company;
using Meisy.Domain.Repositories.User;
using Meisy.Domain.Security.Cryptography;
using Meisy.Domain.Security.Token;
using Meisy.Exception;
using Meisy.Exception.ExceptionBase;
using System.Security.Cryptography;

namespace Meisy.Application.UseCases.Auth.Register
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly ICompanyWriteRepository _companyWriteRepository;
        private readonly ICompanyReadRepository _companyReadRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordEncrypter _bcrypt;
        private readonly ITokenGenerator _tokenGenerator;
        public RegisterUserUseCase(
            IUnitOfWork unitOfWork,
            IUserWriteRepository userWriteRepository,
            IUserReadRepository userReadRepository,
            ICompanyWriteRepository companyWriteRepository,
            ICompanyReadRepository companyReadRepository,
            IMapper mapper,
            IPasswordEncrypter bcrypt,
            ITokenGenerator tokenGenerator
            )
        {
            _tokenGenerator = tokenGenerator;
            _mapper = mapper;
            _bcrypt = bcrypt;
            _unitOfWork = unitOfWork;
            _userWriteRepository = userWriteRepository;
            _userReadRepository = userReadRepository;
            _companyWriteRepository = companyWriteRepository;
            _companyReadRepository = companyReadRepository;
        }

        public async Task<ResponseLoginJson> Execute(RequestRegisterUserJson request)
        {
            if(string.IsNullOrWhiteSpace(request.CompanyCode))
            {
                var companyCode = GenerateCompanyCode();
                var entityCompany = new Company
                {
                    Code = companyCode,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                await _companyWriteRepository.Add( entityCompany );
                await _unitOfWork.Commit();
                request.CompanyCode = companyCode;
            }
            

            await Validate(request);
            var company = await _companyReadRepository.GetByCode(request.CompanyCode);
            var entityUser = _mapper.Map<User>(request);
            entityUser.Password = _bcrypt.Encrypt( request.Password);
            entityUser.CompanyId = company.Id;


            await _userWriteRepository.Add( entityUser );
            await _unitOfWork.Commit();
            
            return new ResponseLoginJson
            {
                Name = request.Name,
                Token = _tokenGenerator.GenerateToken(entityUser),
            };
        }

        private string GenerateCompanyCode(int size = 6)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var bytes = new byte[size];
            RandomNumberGenerator.Fill(bytes);

            var result = new char[size];
            for (int i = 0; i < size; i++)
                result[i] = chars[bytes[i] % chars.Length];

            return new string(result);
        }

        private async Task Validate(RequestRegisterUserJson request)
        {
            var result = new RegisterUserValidator().Validate(request);
            var emailExists = await _userReadRepository.EmailExists(request.Email);
            var companyExists = await _companyReadRepository.CompanyExists(request.CompanyCode!); 

            
            if (emailExists)
            {  
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.UNAVAILABLE_EMAIL));   
            }

            if (!companyExists)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.UNEXISTENT_COMPANY));
            }

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errors);
            }

        }
    }
}

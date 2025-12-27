using AutoMapper;
using Meisy.Communication.Requests;
using Meisy.Domain.Entities;

namespace Meisy.Application.AutoMapper
{
    public class AutoMapping : Profile
    {

        public AutoMapping()
        {
            RequestToEntity();
        
        }


        private void RequestToEntity()
        {
            CreateMap<RequestRegisterUserJson, User>()
                .ForMember(destination => destination.Password, config => config.Ignore())
                .ForMember(destination => destination.CreatedAt, config => config.MapFrom(_=> DateTime.UtcNow))
                .ForMember(destination => destination.UpdatedAt, config => config.MapFrom(_=> DateTime.UtcNow));
        }
    }
}

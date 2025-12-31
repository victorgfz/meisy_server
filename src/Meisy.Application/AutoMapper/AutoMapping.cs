using AutoMapper;
using Meisy.Communication.Requests;
using Meisy.Communication.Responses;
using Meisy.Domain.Entities;

namespace Meisy.Application.AutoMapper
{
    public class AutoMapping : Profile
    {

        public AutoMapping()
        {
            RequestToEntity();
            EntityToResponse();
        }


        private void RequestToEntity()
        {
            CreateMap<RequestRegisterUserJson, User>()
                .ForMember(destination => destination.Password, config => config.Ignore())
                .ForMember(destination => destination.CreatedAt, config => config.MapFrom(_=> DateTime.UtcNow))
                .ForMember(destination => destination.UpdatedAt, config => config.MapFrom(_=> DateTime.UtcNow));

            CreateMap<RequestRegisterInputJson, Input>();
            CreateMap<RequestUpdateInputJson, Input>();
            CreateMap<RequestRegisterOverheadJson, Overhead>();
            CreateMap<RequestUpdateOverheadJson, Overhead>();
        }

        private void EntityToResponse()
        {
            CreateMap<Input, ResponseInputJson>();
            CreateMap<Overhead, ResponseOverheadJson>();

        }
    }
}

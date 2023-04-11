using AutoMapper;
using RlakeFunctionApp.Contracts.Responses;
using RlakeFunctionApp.Entities;

namespace RlakeFunctionApp.Mapper
{
    public class DtoMapperProfile : Profile
    {
        public DtoMapperProfile()
        {
            CreateMap<ChatService.Location, Point>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.AdditionalInfo, opt => opt.MapFrom(src => src.AdditionalInfo))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Longitude));

            CreateMap<ChatService.CompletionResult, CreateConversationResponse>()
                .ForMember(dest => dest.SearchText, opt => opt.MapFrom(src => src.ResponseText))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Locations));

        }
    }
}

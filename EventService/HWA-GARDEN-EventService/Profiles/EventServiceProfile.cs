using AutoMapper;
using HWA.GARDEN.EventService.Domain.Requests;
using HWA.GARDEN.EventService.Models;
using HWA.GARDEN.Utilities.Extensions;

namespace HWA.GARDEN.EventService.Profiles
{
    internal sealed class EventServiceProfile : Profile
    {
        public EventServiceProfile()
        {
            CreateMap<GetEventsByPeriodModel, GetEventListByPeriodQuery>()
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToDateOnly()))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToDateOnly()));
        }
    }
}

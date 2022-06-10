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

            CreateMap<EventModel, CreateEventRequest>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToDateOnly()))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToDateOnly()))
                .IncludeMembers(src => src.Calendar, src => src.Group);

            CreateMap<CalendarModel, CreateEventRequest>()
                .ForMember(dest => dest.CalendarName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CalendarDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CalendarYear, opt => opt.MapFrom(src => src.Year));

            CreateMap<EventGroupModel, CreateEventRequest>()
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.GroupDescription, opt => opt.MapFrom(src => src.Description));
        }
    }
}

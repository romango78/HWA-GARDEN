using AutoMapper;
using HWA.GARDEN.Contracts;
using HWA.GARDEN.EventService.Data.Entities;
using HWA.GARDEN.EventService.Domain.Requests;
using HWA.GARDEN.Utilities.Extensions;

namespace HWA.GARDEN.EventService.Domain.Profiles
{
    internal sealed class EventDomainProfile : Profile
    {
        public EventDomainProfile()
        {
            CreateMap<DateOnly, GetCalendarListQuery>()
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year));

            CreateMap<EventGroupEntity, EventGroup>();

            CreateMap<(EventEntity eventEntity, EventGroupEntity groupEntity, Calendar calendar), Event>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.eventEntity.Id))
                .ForMember(dest => dest.Calendar, opt => opt.MapFrom(src => src.calendar))
                .ForMember(dest => dest.Group, opt =>
                    opt.MapFrom((src, destEvent, destEventGroup, context) =>
                        src.groupEntity != null ? context.Mapper.Map<EventGroup>(src.groupEntity) : null))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.eventEntity.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.eventEntity.Description))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src =>
                    src.eventEntity.StartDt.ToDateOnly(src.calendar.Year).ToDateTime(new TimeOnly())))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => 
                    src.eventEntity.EndDt.ToDateOnly(src.calendar.Year).ToDateTime(new TimeOnly())));

            CreateMap<CreateEventRequest, GetOrCreateCalendarRequest>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CalendarName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.CalendarDescription))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.CalendarYear));
            
        }
    }
}

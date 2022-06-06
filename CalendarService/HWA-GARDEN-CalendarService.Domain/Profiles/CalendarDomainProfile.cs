using AutoMapper;
using HWA.GARDEN.CalendarService.Data.Entities;
using HWA.GARDEN.CalendarService.Domain.Requests;
using HWA.GARDEN.Contracts;

namespace HWA.GARDEN.CalendarService.Domain.Profiles
{
    internal sealed class CalendarDomainProfile : Profile
    {
        private const int DefaultCalendarId = 0;

        public CalendarDomainProfile()
        {
            CreateMap<CalendarEntity, Calendar>();
            CreateMap<CalendarListQuery, Calendar>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => DefaultCalendarId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => Strings.DefaultCalendarName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => Strings.DefaultCalendarDescription))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year));
        }
    }
}

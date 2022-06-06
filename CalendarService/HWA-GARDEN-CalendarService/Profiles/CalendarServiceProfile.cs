using AutoMapper;
using HWA.GARDEN.CalendarService.Domain.Requests;

namespace HWA.GARDEN.CalendarService.Profiles
{
    internal sealed class CalendarServiceProfile : Profile
    {
        public CalendarServiceProfile()
        {
            CreateMap<int, CalendarListQuery>()
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src));
        }
    }
}

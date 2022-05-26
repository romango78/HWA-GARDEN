﻿using HWA.GARDEN.Contracts;
using MediatR;

namespace HWA.GARDEN.EventService.Domain.Requests
{
    internal sealed class CalendarListQuery : IRequest<Calendar>
    {
        public int Year { get; set; }
    }
}

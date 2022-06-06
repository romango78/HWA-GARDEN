using HWA.GARDEN.EventService.Models.Vaidators;
using System.ComponentModel.DataAnnotations;

namespace HWA.GARDEN.EventService.Models
{
    public sealed class EventModel
    {
        public CalendarModel? Calendar { get; set; }

        [Required]
        public EventGroupModel Group { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [ValidDateOnly]
        public string StartDate { get; set; }

        [Required]
        [ValidDateOnly]
        public string EndDate { get; set; }
    }
}

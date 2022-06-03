using HWA.GARDEN.EventService.Models.Vaidators;
using System.ComponentModel.DataAnnotations;

namespace HWA.GARDEN.EventService.Models
{
    public sealed class GetEventsByPeriod
    {
        [Required]
        [ValidDateOnly]
        public string StartDate { get; set; }

        [Required]
        [ValidDateOnly]
        public string EndDate { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace HWA.GARDEN.EventService.Models
{
    public sealed class CalendarModel
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public int Year { get; set; }
    }
}

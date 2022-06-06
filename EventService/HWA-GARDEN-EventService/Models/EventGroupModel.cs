using System.ComponentModel.DataAnnotations;

namespace HWA.GARDEN.EventService.Models
{
    public sealed class EventGroupModel
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }
    }
}

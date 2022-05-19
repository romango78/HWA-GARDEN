namespace HWA.GARDEN.EventService.Domain.DTO
{
    [Serializable]
    public class Event
    {
        public int Id { get; set; }

        public EventGroup Group { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }
    }
}

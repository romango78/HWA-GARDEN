namespace HWA.GARDEN.Contracts
{
    [Serializable]
    public class Event
    {
        public int Id { get; set; }

        public Calendar Calendar { get; set; }

        public EventGroup Group { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}

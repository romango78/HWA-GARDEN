namespace HWA.GARDEN.Contracts
{
    [Serializable]
    public class Event
    {
        public int Id { get; set; }

        public Calendar Calendar { get; set; }

        public EventGroup Group { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }
    }
}

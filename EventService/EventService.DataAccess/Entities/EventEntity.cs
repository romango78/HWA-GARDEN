namespace HWA.GARDEN.EventService.Data.Entities
{
    [Serializable]
    public class EventEntity
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public CalendarEntity Calendar { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int StartDt { get; set; }

        public int EndDt { get; set; }



    }
}

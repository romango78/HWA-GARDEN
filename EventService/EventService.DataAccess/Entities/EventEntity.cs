namespace HWA.GARDEN.EventService.DataAccess.Entities
{
    [Serializable]
    public class EventEntity
    {
        public int Id { get; set; }
        
        public EventGroupEntity Group { get; set; }

        public CalendarEntity Calendar { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int StartDt { get; set; }

        public int EndDt { get; set; }



    }
}

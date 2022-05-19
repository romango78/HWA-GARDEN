namespace HWA.GARDEN.EventService.Domain.DTO
{
    [Serializable]
    public class EventGroup
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }
    }
}

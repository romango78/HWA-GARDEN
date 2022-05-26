namespace HWA.GARDEN.Contracts
{
    [Serializable]
    public class Calendar
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Year { get; set; }
    }
}

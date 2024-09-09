namespace E_Administration.Models
{
    public class FloorViewModel
    {
        public int FloorId { get; set; }
        public int FloorName { get; set; }  // Keeping FloorName as an int
        public string InstituteName { get; set; }  // Institute name to display
        public DateTime CreatedAt { get; set; }
    }
}

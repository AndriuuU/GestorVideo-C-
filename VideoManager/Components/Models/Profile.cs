namespace VideoMatrix.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Device> Devices { get; set; } = new List<Device>();
    }
}
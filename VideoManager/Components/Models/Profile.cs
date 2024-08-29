namespace VideoMatrix.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Transmitter> Transmitters { get; set; } = new List<Transmitter>();
    }
}

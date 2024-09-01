namespace VideoMatrix.Models
{
    public class Profil
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> TransmitterIds { get; set; } = new List<int>(); 
    }
}

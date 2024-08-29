namespace VideoMatrix.Models
{
    public enum DeviceStatus
    {
        On,
        Standby,
        Off
    }

    public abstract class Device
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public DeviceStatus Status { get; set; }
        public string ImageUrl { get; set; } // URL de la imagen simulando la transmisión de video
    }

    public class Transmitter : Device
    {
        public List<Receiver> Receivers { get; set; } = new List<Receiver>();
    }

    public class Receiver : Device
    {
        public int TransmitterId { get; set; }
        public Transmitter Transmitter { get; set; }
    }
}
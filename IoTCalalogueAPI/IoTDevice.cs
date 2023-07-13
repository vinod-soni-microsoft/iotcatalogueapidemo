namespace IoTCalalogueAPI
{
    public class IoTDevice
    {
        public int Id { get; set; }

        public string? Model { get; set; }

        public string? DeviceType { get; set; }

        public double Price { get; set; }

        public bool IsEdge { get; set; }
    }
}
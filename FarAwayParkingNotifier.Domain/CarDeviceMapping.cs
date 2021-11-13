namespace FarAwayParkingNotifier.Domain
{
    /// <summary>
    /// Mapping table between driver's car and device Mac Addresses (Ids)
    /// </summary>
    public class CarDeviceMapping
    {
        public string CarId { get; set; }
        public string DeviceId { get; set; }
    }
}

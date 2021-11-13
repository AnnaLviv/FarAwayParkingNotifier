using NetTopologySuite.Geometries;

namespace FarAwayParkingNotifier.Domain
{
    public class SignalLocation
    {
        public string SignalSourceId { get; set; }
        public Point Location { get; set; }
    }
}

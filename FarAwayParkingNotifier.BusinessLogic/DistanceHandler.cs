using FarAwayParkingNotifier.Domain;

namespace FarAwayParkingNotifier.BusinessLogic
{
    public static class DistanceHandler
    {
        public static bool IsDistanceFarAway(this SignalLocation signalLocation, SignalLocation signalLocationMapped, double farAwayDistanceValue)
        {
            var distance = signalLocation.Location.Coordinate.Distance(signalLocationMapped.Location.Coordinate);
            if (distance >= farAwayDistanceValue)
                return true;
            return false;
        }
    }
}

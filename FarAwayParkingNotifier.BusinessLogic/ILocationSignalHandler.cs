using FarAwayParkingNotifier.Domain;
using System.Threading.Tasks;

namespace FarAwayParkingNotifier.BusinessLogic
{
    public interface ILocationSignalHandler
    {
        public Task ProcessIncomingSignalAsync(SignalLocation signalLocation);
        public bool IsDistanceFarAway(SignalLocation signalLocation, SignalLocation signalLocationMapped);
    }
}

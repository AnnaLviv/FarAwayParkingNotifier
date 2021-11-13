using FarAwayParkingNotifier.Domain;
using FarAwayParkingNotifier.Repository;
using System;
using System.Threading.Tasks;

namespace FarAwayParkingNotifier.BusinessLogic
{
    public class LocationSignalHandler:ILocationSignalHandler
    {
        private readonly ICarDeviceRepository repository;
        private readonly double farAwayDistance;

        public LocationSignalHandler(ICarDeviceRepository repository, double farAwayDistance)
        {
            this.repository = repository;
            this.farAwayDistance = farAwayDistance;
        }

        public async Task ProcessIncomingSignalAsync(SignalLocation signalLocation)
        {
            await repository.AddOrUpdateSignalLocationAsync(signalLocation);
            var mappedLocationId = await repository.GetMappedLocationSourceAsync(signalLocation.SignalSourceId);
            if (mappedLocationId != null)
            {
                var signalLocationMapped = await repository.GetSignalLocationByIdAsync(mappedLocationId);
                if(signalLocationMapped != null)
                {
                    var isDistanceFarAway =  IsDistanceFarAway(signalLocation, signalLocationMapped);
                    if(isDistanceFarAway)
                        await SendNotification(signalLocation.Location);
                }
            }
        }

        public bool IsDistanceFarAway(SignalLocation signalLocation, SignalLocation signalLocationMapped)
        {
            var distance = signalLocation.Location.Coordinate.Distance(signalLocationMapped.Location.Coordinate);
            if (distance >= farAwayDistance)
                return true;
            return false;               
        }

        private Task SendNotification(NetTopologySuite.Geometries.Point location)
        {
            throw new NotImplementedException();
        }
    
    }
}

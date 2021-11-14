using FarAwayParkingNotifier.Domain;
using FarAwayParkingNotifier.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace FarAwayParkingNotifier.BusinessLogic
{
    public class LocationSignalHandler:ILocationSignalHandler
    {
        private const string farAwayDistanceConfigurationKey = "FarAwayDistanceValue";
        private readonly ICarDeviceRepository repository;
        private readonly IConfiguration configuration;

        public LocationSignalHandler(ICarDeviceRepository repository, IConfiguration configuration)
        {
            this.repository = repository;
            this.configuration = configuration;
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
                    var farAwayDistanceValue = configuration.GetValue<double>(farAwayDistanceConfigurationKey);
                    var isDistanceFarAway = signalLocation.IsDistanceFarAway(signalLocationMapped, farAwayDistanceValue);
                    if(isDistanceFarAway)
                        await SendNotification(signalLocation.Location);
                }
            }
        }

        private Task SendNotification(NetTopologySuite.Geometries.Point location)
        {
            throw new NotImplementedException();
        }
    
    }
}

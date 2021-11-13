using FarAwayParkingNotifier.Domain;
using System.Threading.Tasks;

namespace FarAwayParkingNotifier.Repository
{
    public interface ICarDeviceRepository
    {
        public Task<bool> SignalLocationExistsAsync(string signalSourceId);
        public Task AddOrUpdateSignalLocationAsync(SignalLocation signalLocation);
        public Task<SignalLocation> GetSignalLocationByIdAsync(string signalSourceId);
        public Task<string> GetMappedLocationSourceAsync(string signalSourceId);

    }
}

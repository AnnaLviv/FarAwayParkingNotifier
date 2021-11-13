using FarAwayParkingNotifier.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FarAwayParkingNotifier.Repository
{
    public  class CarDeviceRepository: ICarDeviceRepository
    {
        private readonly CarDeviceDbContext context;

        public CarDeviceRepository(CarDeviceDbContext context)
        {
            this.context = context;
        }

        public Task<bool> SignalLocationExistsAsync(string signalSourceId)
        {
            return context.SignalLocations.AnyAsync(signalLocation => signalLocation.SignalSourceId == signalSourceId);
        }

        public async Task AddOrUpdateSignalLocationAsync(SignalLocation signalLocation)
        {
            if (await SignalLocationExistsAsync(signalLocation.SignalSourceId))
            {
                context.Entry(signalLocation).State = EntityState.Modified;
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await SignalLocationExistsAsync(signalLocation.SignalSourceId))
                    {
                        await AddSignalLocationAsync(signalLocation);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                await AddSignalLocationAsync(signalLocation);
            }
        }

        private async Task AddSignalLocationAsync(SignalLocation signalLocation)
        {
            await context.SignalLocations.AddAsync(signalLocation);
            await context.SaveChangesAsync();
        }

        public async Task<SignalLocation> GetSignalLocationByIdAsync(string signalSourceId)
        {
            return await context.SignalLocations.FirstOrDefaultAsync(signalLocation => signalLocation.SignalSourceId == signalSourceId);
        }

        public async Task<string> GetMappedLocationSourceAsync(string signalSourceId)
        {
            var mappedLocationSourceId = (await context.CarDeviceMappings.FirstOrDefaultAsync(mapping => mapping.CarId == signalSourceId))?.DeviceId;
            if (mappedLocationSourceId == null)
                mappedLocationSourceId = (await context.CarDeviceMappings.FirstOrDefaultAsync(mapping => mapping.DeviceId == signalSourceId))?.CarId;
            return mappedLocationSourceId;
        }
    }
}

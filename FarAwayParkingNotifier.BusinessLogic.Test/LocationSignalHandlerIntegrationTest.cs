using FarAwayParkingNotifier.Domain;
using FarAwayParkingNotifier.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Threading.Tasks;

namespace FarAwayParkingNotifier.BusinessLogic.Test
{
    [TestFixture]
    public class LocationSignalHandlerIntegrationTest
    {
        private string connectionString;
        private double farAwayDistance;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(@"appsettings.json")
               .Build();
            connectionString = configuration.GetConnectionString("CarDeviceTestConnex");
            farAwayDistance = configuration.GetValue<double>("FarAwayDistance");
        }

        [Test]
        public async Task ProcessIncomingSignal_NoNotificationTest()
        {
            using var context = new CarDeviceDbContext(connectionString);
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var carDeviceMapping1 = new CarDeviceMapping() { CarId = "VV:AA:AA:AA:01", DeviceId = "DD:AA:AA:01" };
            var carDeviceMapping2 = new CarDeviceMapping() { CarId = "VV:AA:AA:AA:02", DeviceId = "DD:AA:AA:02" };
            var carDeviceMapping3 = new CarDeviceMapping() { CarId = "VV:AA:AA:AA:03", DeviceId = "DD:AA:AA:03" };
            await context.CarDeviceMappings.AddRangeAsync(carDeviceMapping1, carDeviceMapping2, carDeviceMapping3);

            var signalLocationDevice1 = new SignalLocation()
            {
                SignalSourceId = "DD:AA:AA:01",
                Location = new NetTopologySuite.Geometries.Point(13.003725d, 55.604870d) { SRID = 4326 }
            };

            await context.SignalLocations.AddAsync(signalLocationDevice1);
            await context.SaveChangesAsync();

            var repository = new CarDeviceRepository(context);
            var locationSignalHandler = new LocationSignalHandler(repository, farAwayDistance);

            var signalLocation = new SignalLocation()
            {
                SignalSourceId = "VV:AA:AA:AA:01",
                Location = new NetTopologySuite.Geometries.Point(13.003725d, 55.604870d) { SRID = 4326 }
            };
            await locationSignalHandler.ProcessIncomingSignalAsync(signalLocation);
        }

        [Test]
        public async Task ProcessIncomingSignal_SendNotificationTest()
        {
            using var context = new CarDeviceDbContext(connectionString);
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var carDeviceMapping1 = new CarDeviceMapping() { CarId = "VV:AA:AA:AA:01", DeviceId = "DD:AA:AA:01" };
            var carDeviceMapping2 = new CarDeviceMapping() { CarId = "VV:AA:AA:AA:02", DeviceId = "DD:AA:AA:02" };
            var carDeviceMapping3 = new CarDeviceMapping() { CarId = "VV:AA:AA:AA:03", DeviceId = "DD:AA:AA:03" };
            await context.CarDeviceMappings.AddRangeAsync(carDeviceMapping1, carDeviceMapping2, carDeviceMapping3);

            var signalLocationDevice1 = new SignalLocation()
            {
                SignalSourceId = "DD:AA:AA:01",
                Location = new NetTopologySuite.Geometries.Point(13.003725d, 55.604870d) { SRID = 4326 }
            };

            await context.SignalLocations.AddAsync(signalLocationDevice1);
            await context.SaveChangesAsync();

            var repository = new CarDeviceRepository(context);
            var locationSignalHandler = new LocationSignalHandler(repository, farAwayDistance);

            var signalLocation = new SignalLocation()
            {
                SignalSourceId = "VV:AA:AA:AA:01",
                Location = new NetTopologySuite.Geometries.Point(13.003725d, 58.604870d) { SRID = 4326 }
            };
            await locationSignalHandler.ProcessIncomingSignalAsync(signalLocation);
        }
    }
}

using FarAwayParkingNotifier.Domain;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;

namespace FarAwayParkingNotifier.Repository.Test
{
    [TestFixture]
    public class CarDeviceRepositoryTest
    {

        [Test]
        public async Task SignalLocationExistsTest()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("SignalLocationExists");
            using var context = new CarDeviceDbContext(builder.Options);

            var repository = new CarDeviceRepository(context);

            var result = await repository.SignalLocationExistsAsync("VV:AA:AA:AA:01");
            Assert.IsFalse(result);

            var signalLocation = new SignalLocation()
            {
                SignalSourceId = "VV:AA:AA:AA:01",
                Location = new NetTopologySuite.Geometries.Point(13.003725d, 55.604870d) { SRID = 4326 }
            };

            await context.SignalLocations.AddAsync(signalLocation);
            Assert.AreEqual(EntityState.Added, context.Entry(signalLocation).State);

            result = await repository.SignalLocationExistsAsync("VV:AA:AA:AA:01");
            Assert.IsFalse(result);

            await context.SaveChangesAsync();

            result = await repository.SignalLocationExistsAsync("VV:AA:AA:AA:01");
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetSignalLocationByIdTest()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("GetSignalLocationById");
            using var context = new CarDeviceDbContext(builder.Options);

            var repository = new CarDeviceRepository(context);

            var signalLocation = new SignalLocation()
            {
                SignalSourceId = "VV:AA:AA:AA:01",
                Location = new NetTopologySuite.Geometries.Point(13.003725d, 55.604870d) { SRID = 4326 }
            };

            await context.SignalLocations.AddAsync(signalLocation);

            var result = await repository.GetSignalLocationByIdAsync("VV:AA:AA:AA:01");
            Assert.AreEqual(null, result);

            await context.SaveChangesAsync();
            result = await repository.GetSignalLocationByIdAsync("VV:AA:AA:AA:01");
            Assert.AreEqual(signalLocation, result);
        }

        [Test]
        public async Task AddOrUpdateSignalLocationTest()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("AddOrUpdateSignalLocation");

            using var context = new CarDeviceDbContext(builder.Options);

            var repository = new CarDeviceRepository(context);

            var signalLocation = new SignalLocation()
            {
                SignalSourceId = "VV:AA:AA:AA:01",
                Location = new NetTopologySuite.Geometries.Point(13.003725d, 55.604870d) { SRID = 4326 }
            };
            await repository.AddOrUpdateSignalLocationAsync(signalLocation);
            var result = await repository.GetSignalLocationByIdAsync("VV:AA:AA:AA:01");
            Assert.AreEqual(signalLocation, result);

            signalLocation.Location = new NetTopologySuite.Geometries.Point(13.003725d, 56.604870d) { SRID = 4326 };
            await repository.AddOrUpdateSignalLocationAsync(signalLocation);
            result = await repository.GetSignalLocationByIdAsync("VV:AA:AA:AA:01");
            Assert.AreEqual(signalLocation, result);
        }


        [TestCase("VV:AA:AA:AA:01", "DD:AA:AA:01")]
        [TestCase("DD:AA:AA:01", "VV:AA:AA:AA:01")]
        [TestCase("VV:AA:AA:AA:02", "DD:AA:AA:02")]
        [TestCase("DD:AA:AA:02", "VV:AA:AA:AA:02")]
        [TestCase("nonExisting", null)]
        public async Task GetMappedLocationSourceTest(string signalSourceId, string expectedResult)
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("GetMappedLocationSource");

            using var context = new CarDeviceDbContext(builder.Options);
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            var repository = new CarDeviceRepository(context);

            var carDeviceMapping1 = new CarDeviceMapping() { CarId = "VV:AA:AA:AA:01", DeviceId = "DD:AA:AA:01" };
            var carDeviceMapping2 = new CarDeviceMapping() { CarId = "VV:AA:AA:AA:02", DeviceId = "DD:AA:AA:02" };
            var carDeviceMapping3 = new CarDeviceMapping() { CarId = "VV:AA:AA:AA:03", DeviceId = "DD:AA:AA:03" };
            await context.CarDeviceMappings.AddRangeAsync(carDeviceMapping1, carDeviceMapping2, carDeviceMapping3);
            await context.SaveChangesAsync();

            var result = await repository.GetMappedLocationSourceAsync(signalSourceId);
            Assert.AreEqual(expectedResult, result);
        }
    }
}
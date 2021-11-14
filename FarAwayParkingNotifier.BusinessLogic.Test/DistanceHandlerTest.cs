using FarAwayParkingNotifier.Domain;
using NUnit.Framework;

namespace FarAwayParkingNotifier.BusinessLogic.Test
{
    [TestFixture]
    public class DistanceHandlerTest
    {
        [TestCase(13.003725d, 55.604870d, 0.5, false)]
        [TestCase(13.003725d, 58.604870d, 0.5, true)]
        [TestCase(13.003725d, 58.604870d, 5, false)]
        public void IsDistanceFarAwayTest(double pointX, double pointY, double farAwayDistance, bool expectedResult)
        {
            var signalLocation = new SignalLocation()
            {
                SignalSourceId = "VV:AA:AA:AA:01",
                Location = new NetTopologySuite.Geometries.Point(13.003725d, 55.604870d) { SRID = 4326 }
            };

            var signalLocationMapped = new SignalLocation()
            {
                SignalSourceId = "VV:AA:AA:AA:01",
                Location = new NetTopologySuite.Geometries.Point(pointX, pointY) { SRID = 4326 }
            };

            var isDistancesFarAway = signalLocation.IsDistanceFarAway(signalLocationMapped, farAwayDistance);
            Assert.AreEqual(expectedResult, isDistancesFarAway);


        }
    }
}



namespace Nz.Co.Dunkley.Cornerman.Api.IntegrationTests.Repositories
{
    using System;
    using System.Linq;
    using Api.Repositories;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Ploeh.AutoFixture;

    [TestClass]
    public class RidePointRepositoryTests
    {
        private readonly RidePointRepository _ridePointRepository;
        private static IFixture _fixture;

        public RidePointRepositoryTests()
        {
            _ridePointRepository = new RidePointRepository();
            _fixture = new Fixture();
        }

        [TestMethod]
        public void WhenCreateRidePoint_IsShouldExistInStorage()
        {
            // ARRANGE – this should be any code that needs to setup the test Mock / Dummy objects etc.
            var expected = _fixture
                .Build<RidePoint>()
                .Create();

            var expectedRider = _fixture
                .Build<Rider>()
                .Create();

            string ridePointId = "";

            try
            {

                // ACT – Execute the actual functionality being tested
                ridePointId = _ridePointRepository.CreateOrUpdate(
                    expectedRider.RiderId,
                    expected.PointDateTime,
                    expected.Altitude,
                    expected.Longitude,
                    expected.Latitude);

                // ASSERT – check that the results of the test are as expected
                var actual = _ridePointRepository.Retrieve(expectedRider.RiderId, ridePointId);
                Assert.IsNotNull(actual);
                Assert.AreEqual(actual.Altitude, expected.Altitude);
                Assert.AreEqual(actual.Longitude, expected.Longitude);
                Assert.AreEqual(actual.Latitude, expected.Latitude);
            }
            finally
            {
                // TEAR-DOWN
                _ridePointRepository.Delete(expectedRider.RiderId, ridePointId);
            }
        }

        [TestMethod]
        public void WhenCreatedManyRidePoints_TheyShouldBeRetrievedInPointDateTimeOrderAsc()
        {
            // ARRANGE – this should be any code that needs to setup the test Mock / Dummy objects etc.
            var expected = _fixture
                .Build<RidePoint>()
                .CreateMany(10)
                .ToList();

            var expectedRider = _fixture
                .Build<Rider>()
                .Create();

            foreach (var expectedRidePoint in expected)
            {
                _ridePointRepository.CreateOrUpdate(
                    expectedRider.RiderId,
                    expectedRidePoint.PointDateTime,
                    expectedRidePoint.Altitude,
                    expectedRidePoint.Longitude,
                    expectedRidePoint.Latitude);
            }

            try
            {
                // ACT – Execute the actual functionality being tested
                var actual = _ridePointRepository.RetrieveForRiderId(expectedRider.RiderId);

                // ASSERT – check that the results of the test are as expected
                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.Count == expected.Count);

                var lastPointDateTime = DateTime.MinValue;

                var results = 
                    from a in actual
                        join e in expected on 
                            a.PointDateTime equals e.PointDateTime
                    select a;

                foreach (var actualRidePoint in results)
                {
                    Assert.IsTrue(lastPointDateTime < actualRidePoint.PointDateTime);
                    lastPointDateTime = actualRidePoint.PointDateTime;
                }
            }
            finally
            {
                // TEAR-DOWN
                _ridePointRepository.DeleteForRiderId(expectedRider.RiderId);
            }            
        }
    }
}

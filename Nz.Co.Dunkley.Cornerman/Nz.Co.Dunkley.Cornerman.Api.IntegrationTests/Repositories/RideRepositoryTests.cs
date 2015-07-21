
namespace Nz.Co.Dunkley.Cornerman.Api.IntegrationTests.Repositories
{
    using System;
    using System.Linq;
    using Api.Repositories;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Ploeh.AutoFixture;
    using Assert = NUnit.Framework.Assert;

    [TestClass]
    public class RideRepositoryTests
    {
        private readonly IRideRepository _rideRepository;
        private static IFixture _fixture;

        public RideRepositoryTests()
        {
            _rideRepository = new RideRepositoryMongoDb();
            _fixture = new Fixture();
        }

        [TestMethod]
        public void WhenCreateRide_ItShouldExistInStorage()
        {
            // ARRANGE – this should be any code that needs to setup the test Mock / Dummy objects etc.
            var expected = _fixture
                .Build<Ride>()
                .Create();

            expected.CompletedDateTime = _fixture.Create<DateTime>().ToUniversalTime();
            expected.StartDateTime = _fixture.Create<DateTime>().ToUniversalTime();

            // ACT – Execute the actual functionality being tested
            _rideRepository.Upsert(expected);

            try
            {
                // ASSERT – check that the results of the test are as expected
                var actual = _rideRepository.Retrieve(expected.RideId);
                Assert.IsNotNull(actual);

                Assert.Less(Math.Abs(actual.StartDateTime.Ticks - expected.StartDateTime.Ticks), 10000);
                Assert.Less(Math.Abs(actual.CompletedDateTime.Ticks - expected.CompletedDateTime.Ticks), 10000);
                Assert.AreEqual(actual.Lead.MembershipId, expected.Lead.MembershipId);
                Assert.AreEqual(actual.Tailgunner.MembershipId, expected.Tailgunner.MembershipId);

                /*foreach (var aw in actual.Wingmen)
                {
                    Assert.IsTrue(expected.Wingmen.Any(ew => ew.MembershipId == aw.MembershipId));
                }

                foreach (var ar in actual.Riders)
                {
                    Assert.IsTrue(expected.Riders.Any(er => er.MembershipId == ar.MembershipId));
                }*/
            }
            finally
            {
                // TEAR-DOWN
                _rideRepository.Delete(expected.RideId);    
            }
        }

        [TestMethod]
        public void WhenCreatedManyRidesForAMemberRetrievingRidesForAMember_ItShouldReturnManyRides()
        {
            // ARRANGE – this should be any code that needs to setup the test Mock / Dummy objects etc.
            var expected = _fixture
                .Build<Ride>()
                .CreateMany()
                .ToList();

            var membershipId = expected.First().Lead.MembershipId;

            foreach (var ride in expected)
            {
                ride.Lead.MembershipId = membershipId;

                _rideRepository.Upsert(ride);
            }

            try
            {
                // ACT – Execute the actual functionality being tested
                var actual = _rideRepository.RetrieveForMembershipId(membershipId);
                
                // ASSERT – check that the results of the test are as expected
                Assert.IsNotNull(actual);
                Assert.AreEqual(actual.Count, expected.Count);
            }
            finally
            {
                // TEAR-DOWN
                foreach (var ride in expected)
                {
                    _rideRepository.Delete(ride.RideId);
                }
            }
        }

        [TestMethod]
        public void WhenInsertingALeadForARideWithoutOne_ItShouldReturnTheRideWithTheLeadInserted()
        {
            // ARRANGE – this should be any code that needs to setup the test Mock / Dummy objects etc.
            var expectedRide = _fixture
                .Build<Ride>()
                .Without(r => r.Lead)
                .Without(r => r.Created)
                .Without(r => r.LastModified)
                .Create();

            var expectedLead = _fixture.Create<Rider>();
            var expectedRideId = _rideRepository.Upsert(expectedRide);

            try
            {
                // ACT – Execute the actual functionality being tested
                var actualRiderId = _rideRepository.UpsertLead(expectedRideId, expectedLead);

                // ASSERT – check that the results of the test are as expected
                var actualRide = _rideRepository.Retrieve(expectedRideId);

                Assert.IsNotNull(actualRide);
                Assert.IsNotNull(actualRide.Lead);
                Assert.AreEqual(expectedLead.RiderId, actualRiderId);
                Assert.AreEqual(expectedLead.RiderId, actualRide.Lead.RiderId);
                Assert.Greater(actualRide.LastModified, actualRide.Created);
            }
            finally
            {
                // TEAR-DOWN
                _rideRepository.Delete(expectedRideId);
            }
        }

        [TestMethod]
        public void WhenUpdatingALeadForARide_ItShouldReturnTheRideWithTheLeadUpdated()
        {
            // ARRANGE – this should be any code that needs to setup the test Mock / Dummy objects etc.
            var expectedRide = _fixture
                .Build<Ride>()
                .Without(r => r.Created)
                .Without(r => r.LastModified)
                .Create();

            var expectedLead = _fixture.Create<Rider>();
            var expectedRideId = _rideRepository.Upsert(expectedRide);

            try
            {
                // ACT – Execute the actual functionality being tested
                var actualRiderId = _rideRepository.UpsertLead(expectedRideId, expectedLead);

                // ASSERT – check that the results of the test are as expected
                var actualRide = _rideRepository.Retrieve(expectedRideId);

                Assert.IsNotNull(actualRide);
                Assert.AreNotEqual(expectedRide.Lead.RiderId, actualRide.Lead.RiderId);
                Assert.AreEqual(expectedLead.RiderId, actualRiderId);
                Assert.AreEqual(expectedLead.RiderId, actualRide.Lead.RiderId);
                Assert.Greater(actualRide.LastModified, actualRide.Created);
            }
            finally
            {
                // TEAR-DOWN
                _rideRepository.Delete(expectedRideId);
            }
        }

        [TestMethod]
        public void WhenUpdatingATailgunnerForARideWithoutOne_ItShouldReturnTheRideWithTheTailgunnerInserted()
        {
            // ARRANGE – this should be any code that needs to setup the test Mock / Dummy objects etc.
            var expectedRide = _fixture
                .Build<Ride>()
                .Without(r => r.Tailgunner)
                .Without(r => r.Created)
                .Without(r => r.LastModified)
                .Create();

            var expectedTailgunner = _fixture.Create<Rider>();
            var expectedRideId = _rideRepository.Upsert(expectedRide);

            try
            {
                // ACT – Execute the actual functionality being tested
                var actualRiderId = _rideRepository.UpsertTailgunner(expectedRideId, expectedTailgunner);

                // ASSERT – check that the results of the test are as expected
                var actualRide = _rideRepository.Retrieve(expectedRideId);

                Assert.IsNotNull(actualRide);
                Assert.IsNotNull(actualRide.Tailgunner);
                Assert.AreEqual(expectedTailgunner.RiderId, actualRide.Tailgunner.RiderId);
                Assert.AreEqual(expectedTailgunner.RiderId, actualRiderId);
                Assert.Greater(actualRide.LastModified, actualRide.Created);
            }
            finally
            {
                // TEAR-DOWN
                _rideRepository.Delete(expectedRideId);
            }
        }

        [TestMethod]
        public void WhenUpdatingATailgunnerForARide_ItShouldReturnTheRideWithTheTailgunnerUpdated()
        {
            // ARRANGE – this should be any code that needs to setup the test Mock / Dummy objects etc.
            var expectedRide = _fixture
                .Build<Ride>()
                .Without(r => r.Created)
                .Without(r => r.LastModified)
                .Create();

            var expectedTailgunner = _fixture.Create<Rider>();
            var expectedRideId = _rideRepository.Upsert(expectedRide);

            try
            {
                // ACT – Execute the actual functionality being tested
                var actualRiderId = _rideRepository.UpsertTailgunner(expectedRideId, expectedTailgunner);

                // ASSERT – check that the results of the test are as expected
                var actualRide = _rideRepository.Retrieve(expectedRideId);

                Assert.IsNotNull(actualRide);
                Assert.AreNotEqual(expectedRide.Tailgunner.RiderId, actualRide.Tailgunner.RiderId);
                Assert.AreEqual(expectedTailgunner.RiderId, actualRide.Tailgunner.RiderId);
                Assert.AreEqual(expectedTailgunner.RiderId, actualRiderId);
                Assert.Greater(actualRide.LastModified, actualRide.Created);
            }
            finally
            {
                // TEAR-DOWN
                _rideRepository.Delete(expectedRideId);
            }
        }

        [TestMethod]
        public void WhenInsertingAWingmanForARideWithoutWingmen_ItShouldReturnTheRideWithTheWingmanInserted()
        {
            // ARRANGE – this should be any code that needs to setup the test Mock / Dummy objects etc.
            var expectedRide = _fixture
                .Build<Ride>()
                .Without(r => r.Wingmen)
                .Without(r => r.Created)
                .Without(r => r.LastModified)
                .Create();

            var expectedWingman = _fixture.Create<Rider>();
            var expectedRideId = _rideRepository.Upsert(expectedRide);

            try
            {
                // ACT – Execute the actual functionality being tested
                var actualWingmanRiderId = _rideRepository.UpsertWingman(expectedRideId, expectedWingman);

                // ASSERT – check that the results of the test are as expected
                var actualRide = _rideRepository.Retrieve(expectedRideId);

                Assert.IsNotNull(actualRide);
                Assert.IsTrue(actualRide.Wingmen.Any(w => w.RiderId == expectedWingman.RiderId));
                Assert.AreEqual(expectedWingman.RiderId, actualWingmanRiderId);
                Assert.Greater(actualRide.LastModified, actualRide.Created);
            }
            finally
            {
                // TEAR-DOWN
                _rideRepository.Delete(expectedRideId);
            }
        }

        [TestMethod]
        public void WhenInsertingAWingmanForARideWithExistingWingmen_ItShouldReturnTheRideWithTheWingmanInserted()
        {
            // ARRANGE – this should be any code that needs to setup the test Mock / Dummy objects etc.
            var expectedRide = _fixture
                .Build<Ride>()
                .Without(r => r.Created)
                .Without(r => r.LastModified)
                .Create();

            var expectedWingman = _fixture.Create<Rider>();
            var expectedRideId = _rideRepository.Upsert(expectedRide);

            try
            {
                // ACT – Execute the actual functionality being tested
                var actualWingmanRiderId = _rideRepository.UpsertWingman(expectedRideId, expectedWingman);

                // ASSERT – check that the results of the test are as expected
                var actualRide = _rideRepository.Retrieve(expectedRideId);

                Assert.IsNotNull(actualRide);
                Assert.IsTrue(actualRide.Wingmen.Any(w => w.RiderId == expectedWingman.RiderId));
                Assert.AreEqual(expectedWingman.RiderId, actualWingmanRiderId);
                Assert.Greater(actualRide.LastModified, actualRide.Created);
            }
            finally
            {
                // TEAR-DOWN
                _rideRepository.Delete(expectedRideId);
            }
        }

        [TestMethod]
        public void WhenInsertingARiderForARide_ItShouldReturnTheRideWithTheRiderInserted()
        {
            // ARRANGE – this should be any code that needs to setup the test Mock / Dummy objects etc.
            var expectedRide = _fixture
                .Build<Ride>()
                .Without(r => r.Created)
                .Without(r => r.LastModified)
                .Create();

            var expectedRider = _fixture.Create<Rider>();
            var expectedRideId = _rideRepository.Upsert(expectedRide);

            try
            {
                // ACT – Execute the actual functionality being tested
                var actualRiderId = _rideRepository.UpsertRider(expectedRideId, expectedRider);

                // ASSERT – check that the results of the test are as expected
                var actualRide = _rideRepository.Retrieve(expectedRideId);

                Assert.IsNotNull(actualRide);
                Assert.IsTrue(actualRide.Riders.Any(w => w.RiderId == expectedRider.RiderId));
                Assert.AreEqual(expectedRider.RiderId, actualRiderId);
                Assert.Greater(actualRide.LastModified, actualRide.Created);
            }
            finally
            {
                // TEAR-DOWN
                _rideRepository.Delete(expectedRideId);
            }
        }    

    }
}

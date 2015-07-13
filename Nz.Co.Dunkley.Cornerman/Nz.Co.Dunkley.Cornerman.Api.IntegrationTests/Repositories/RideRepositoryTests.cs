
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
            _rideRepository = new RideRepository();
            _fixture = new Fixture();
        }

        [TestMethod]
        public void WhenCreateRide_IsShouldExistInStorage()
        {
            // ARRANGE – this should be any code that needs to setup the test Mock / Dummy objects etc.
            var expected = _fixture
                .Build<Ride>()
                .Create();

            expected.CompletedDateTime = _fixture.Create<DateTime>().ToUniversalTime();
            expected.StartDateTime = _fixture.Create<DateTime>().ToUniversalTime();

            // ACT – Execute the actual functionality being tested
            _rideRepository.Create(expected).GetAwaiter().GetResult();

            try
            {
                // ASSERT – check that the results of the test are as expected
                var actual = _rideRepository.Retrieve(expected.RideId).Result;
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
        public void WhenCreatedManyRidesForAMemberRetrievingRidesForAMember_IsShouldReturnManyRides()
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

                _rideRepository.Create(ride).GetAwaiter().GetResult();
            }

            try
            {
                // ACT – Execute the actual functionality being tested
                var actual = _rideRepository.RetrieveForMembershipId(membershipId).Result;
                
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
    }
}


namespace Nz.Co.Dunkley.Cornerman.Api.IntegrationTests.Repositories
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Assert = NUnit.Framework.Assert;
    using MongoRepository;
    using Ploeh.AutoFixture;

    [TestClass]
    public class RideRepositoryTests
    {
        private readonly MongoRepository<Ride> _rideRepository;
        private static IFixture _fixture;

        public RideRepositoryTests()
        {
            _rideRepository = new MongoRepository<Ride>();
            _fixture = new Fixture();
        }

        [TestMethod]
        public void WhenCreateRide_IsShouldExistInStorage()
        {
            // ARRANGE – this should be any code that needs to setup the test Mock / Dummy objects etc.
            var expected = _fixture
                .Build<Ride>()
                .Without(a => a.Id)
                .Create();

            // ACT – Execute the actual functionality being tested
            _rideRepository.Add(expected);

            try
            {
                // ASSERT – check that the results of the test are as expected
                var actual = _rideRepository.GetById(expected.Id);
                Assert.IsNotNull(actual);
                Assert.AreEqual(actual.StartDateTime, expected.StartDateTime);
                Assert.AreEqual(actual.Lead.MembershipId, expected.Lead.MembershipId);
                Assert.AreEqual(actual.Tailgunner.MembershipId, expected.Tailgunner.MembershipId);

                foreach (var aw in actual.Wingmen)
                {
                    Assert.IsTrue(expected.Wingmen.Any(ew => ew.MembershipId == aw.MembershipId));
                }

                foreach (var ar in actual.Riders)
                {
                    Assert.IsTrue(expected.Riders.Any(er => er.MembershipId == ar.MembershipId));
                }
            }
            finally
            {
                // TEAR-DOWN
                _rideRepository.Delete(expected.Id);    
            }
        }
    }
}

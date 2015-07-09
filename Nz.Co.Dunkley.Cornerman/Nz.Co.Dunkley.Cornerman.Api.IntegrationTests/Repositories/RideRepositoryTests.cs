
namespace Nz.Co.Dunkley.Cornerman.Api.IntegrationTests.Repositories
{
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
        public void WhenCreateActivity_IsShouldExistInStorage()
        {
            // ARRANGE – this should be any code that needs to setup the test Mock / Dummy objects etc.
            var expected = _fixture
                .Build<Ride>()
                .Without(a => a.Id)
                .Create();

            // ACT – Execute the actual functionality being tested
            _rideRepository.Add(expected);

            // ASSERT – check that the results of the test are as expected

            var actual = _rideRepository.GetById(expected.Id);
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.StartDateTime, expected.StartDateTime);
            Assert.AreEqual(actual.Lead, expected.Lead);
            Assert.AreEqual(actual.Tailgunner, expected.Tailgunner);

            CollectionAssert.AreEquivalent(actual.Wingmen, expected.Wingmen);

            _rideRepository.Delete(expected.Id);
        }
    }
}

using DatabaseComparer;
using FluentAssertions;
using NUnit.Framework;

namespace DatabaseComparerTests
{
    public class RandomDbComparerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(10)]
        public void RandomDiff1ToDiff2ShouldBeState1ToState2Test(int n)
        {
            var dbComparer = new DbComparer();
            var dbState0 = TestDataGenerator.GetTestDbRandomState(n);
            var dbState1 = TestDataGenerator.GetTestDbRandomState(n);
            var dbState2 = TestDataGenerator.GetTestDbRandomState(n);
            var diff1 = dbComparer.GetDbDiff(dbState0, dbState1);
            var diff2 = dbComparer.GetDbDiff(dbState0, dbState2);
            var diff3 = dbComparer.GetDbDiff(dbState1, dbState2);
            var diffOfDiff = dbComparer.GetDbDiff(diff1, diff2);

            diffOfDiff.Should().Be(diff3);
        }
    }
}
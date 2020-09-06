using DatabaseComparer;
using FluentAssertions;
using NUnit.Framework;

namespace DatabaseComparerTests
{
    public class DbComparerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        private static object[] _testDbStateTriple =
        {
            new object[]
            {
                TestDataGenerator.GetTestDbState0(),
                TestDataGenerator.GetTestDbState1(),
                TestDataGenerator.GetTestDbState2()
            }
        };

        [TestCaseSource(nameof(_testDbStateTriple))]
        public void Diff1ToDiff2ShouldBeState1ToState2Test(IDbStateReference dbState0, IDbStateReference dbState1, IDbStateReference dbState2)
        {
            var dbComparer = new DbComparer();
            var diff1 = dbComparer.GetDbDiff(dbState0, dbState1);
            var diff2 = dbComparer.GetDbDiff(dbState0, dbState2);
            var diff3 = dbComparer.GetDbDiff(dbState1, dbState2);
            var diffOfDiff = dbComparer.GetDbDiff(diff1, diff2);

            diffOfDiff.Should().Be(diff3);
        }
    }
}
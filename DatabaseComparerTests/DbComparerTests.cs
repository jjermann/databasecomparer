using DatabaseComparer;
using NUnit.Framework;
using Shouldly;

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
                SqlTestDataGenerator.GetTestDbState0(),
                SqlTestDataGenerator.GetTestDbState1(),
                SqlTestDataGenerator.GetTestDbState2()
            }
        };

        [TestCaseSource(nameof(_testDbStateTriple))]
        public void Diff1ToDiff2ShouldBeState1ToState2(IDbStateReference dbState0, IDbStateReference dbState1, IDbStateReference dbState2)
        {
            var dbComparer = new DbComparer();
            var diff1 = dbComparer.GetDbDiff(dbState0, dbState1);
            var diff2 = dbComparer.GetDbDiff(dbState0, dbState2);
            var diff3 = dbComparer.GetDbDiff(dbState1, dbState2);
            var diffOfDiff = dbComparer.GetDbDiff(diff1, diff2);

            diffOfDiff.ShouldBe(diff3);
        }
    }
}
using Monads.Maybe;
using NUnit.Framework;

namespace Monads.Tests.Maybe
{
    [TestFixture]
    public class MaybeFunctorTests
    {
        private MaybeMaker<int> maker;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            maker = new MaybeMaker<int>();
        }

        [Test]
        public void Maybe_Functor_FMap_Just()
        {
            var a = maker.Unit(1);
            var b = a.FMap(x => x + 1);

            Assert.IsTrue(b.HasValue);
            Assert.AreEqual(2, b.Value);
        }

        [Test]
        public void Maybe_Functor_FMap_Nothing()
        {
            var a = Maybe<int>.Nothing;
            var b = a.FMap(x => x + 1);

            Assert.IsTrue(!b.HasValue);
        }


    }
}

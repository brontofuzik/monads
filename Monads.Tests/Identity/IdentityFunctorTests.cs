using Monads.Identity;
using NUnit.Framework;

namespace Monads.Tests.Identity
{
    [TestFixture]
    public class IdentityFunctorTests
    {
        private IdentityMaker<int> maker;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            maker = new IdentityMaker<int>();
        }

        [Test]
        public void Identity_Functor_FMap()
        {
            var a = maker.Unit(1);
            var b = a.FMap(x => x + 1);
            
            Assert.AreEqual(2, b.Value);
        }

        [Test]
        public void Identity_to_Maybe()
        {
            var a = maker.Unit(1);

            // Compilation error!
            //var b = a.FMap<int, MaybeClass.Maybe<int>>(x => x + 1);
        }
    }
}

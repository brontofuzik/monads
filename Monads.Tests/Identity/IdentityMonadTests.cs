using Monads.Identity;
using NUnit.Framework;

namespace Monads.Tests.Identity
{
    [TestFixture]
    public class IdentityMonadTests
    {
        private IdentityMaker<int> maker;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            maker = new IdentityMaker<int>();
        }

        [Test]
        public void Identity_Monad_Unit()
        {
            var a = maker.Unit(1);

            Assert.AreEqual(1, a.Value);
        }

        [Test]
        public void Identity_Monad_Bind()
        {
            var a = maker.Unit(1);
            var b = a.Bind(x => maker.Unit(x + 1));

            Assert.AreEqual(2, b.Value);
        }
    }
}

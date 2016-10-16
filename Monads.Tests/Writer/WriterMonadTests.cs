using System.Linq;
using Monads.Writer;
using NUnit.Framework;

namespace Monads.Tests.Writer
{
    [TestFixture]
    public class WriterMonadTests
    {
        [Test]
        public void Test()
        {
            var result = Half(8).RunWriter_Uncurried<string, int>();
            
            Assert.AreEqual(4, result.Value);
            Assert.AreEqual("Calling Half(8)", result.Output.First());
        }

        private Writer<string, int> Half(int x)
        {
            var writer = new WriterMaker<string, int>();

            return writer.Tell($"Calling Half({x})").Bind<int, Writer<string,int>>(_ =>
                writer.Unit(x/2));
        }
    }
}

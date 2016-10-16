using System;
using Monads.State;
using NUnit.Framework;

namespace Monads.Tests.State
{
    [TestFixture]
    public class StateMonadTests
    {
        [Test]
        public void Test()
        {
            var S = new StateMonadFactory<int, int>();

            // tick :: State Int Int
            // tick = do
            //     n <- get
            //     put(n + 1)
            //     return n
            var tick = S.Get().Bind(n =>
                S.Put(n + 1).Bind(_ =>
                S.Unit(n)));

            // plusOne :: Int -> Int
            // plusOne n = execState tick n
            Func<int, int> plusOne = n => S.ExecState(tick, n);

            Assert.AreEqual(2, plusOne(1));
        }
    }
}

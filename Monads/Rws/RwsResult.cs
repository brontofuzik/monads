using System.Collections.Generic;

namespace Monads.Rws
{
    public class RwsResult<W, S, A>
    {
        internal RwsResult(A value, IEnumerable<W> output, S state)
        {
            Value = value;
            Output = output;
            State = state;
        }

        public A Value { get; }

        public IEnumerable<W> Output { get; }

        public S State { get; }
    }

    public static class RwsResult
    {
        public static RwsResult<W, S, A> New<W, S, A>(A value, IEnumerable<W> output, S state)
            => new RwsResult<W, S, A>(value, output, state);
    }
}
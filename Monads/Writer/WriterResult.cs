using System.Collections.Generic;
using Monads.Common;

namespace Monads.Writer
{
    public class WriterResult<W, A> : Pair<A, IEnumerable<W>>
    {
        public WriterResult(A a, IEnumerable<W> output)
            : base(a, output)
        {
        }

        public A Value => First;

        public IEnumerable<W> Output => Second;
    }

    public static class WriterResult
    {
        public static WriterResult<W, A> New<W, A>(A a, IEnumerable<W> output)
            => new WriterResult<W, A>(a, output);
    }
}
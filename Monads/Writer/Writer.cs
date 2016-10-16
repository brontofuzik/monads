using System;
using System.Linq;
using Monads.Common;

namespace Monads.Writer
{
    // Maker
    public class WriterMaker<W, A> : IMonadMaker<Writer, A, Writer<W, A>>
    {
        // Writer
        public Writer<W, A> Writer<W, A>(WriterF<W, A> func) => WriterMonad.Writer(func);

        public Writer<W, Common.Void> Tell<W>(W w) => WriterMonad.Tell(w);

        // Unit
        public Writer<W, A> Unit(A value) => WriterMonad.Unit<W, A>(value);
    }

    public abstract class Writer { }

    public sealed class Writer<W, A>
        : Writer,
        IGeneric<Writer, A>,                // Writer<A>
        IFunctor<Writer, A, Writer<W, A>>,  // Functor
        IMonad<Writer, A>                   // Monad
    {
        public WriterF<W, A> Func { get; }

        internal Writer(WriterF<W, A> func)
        {
            Func = func;
        }

        public WriterF<W, A> RunWriter<W, A>() => WriterMonad.RunWriter(this as Writer<W, A>);

        // Uncurried
        public WriterResult<W, A> RunWriter_Uncurried<W, A>() => WriterMonad.RunWriter_Uncurried(this as Writer<W, A>);

        #region IFunctor

        public WriterB FMap<B, WriterB>(Func<A, B> f)
            where WriterB : Writer, IGeneric<Writer, B>
        {
            var result = WriterFunctor.FMap(this, f);
            return result.AsGeneric().Cast<WriterB>();
        }

        #endregion // IFunctor

        #region IMonad

        // Bind
        public WriterB Bind<B, WriterB>(Func<A, WriterB> f)
            where WriterB : Writer, IGeneric<Writer, B>, IMonad<Writer, B>
        {
            Func<A, Writer<W, B>> fa = a => f(a).AsGeneric().Cast<Writer<W, B>>();
            var result = WriterMonad.Bind(this, fa);
            return result.AsGeneric().Cast<WriterB>();
        }

        #endregion // IMonad
    }

    internal static class WriterFunctor
    {
        public static Writer<W, B> FMap<W, A, B>(Writer<W, A> writer, Func<A, B> func)
        {
            // TODO
            throw new NotImplementedException();
        }
    }

    internal static class WriterMonad
    {
        #region Conversion

        public static Writer<W, A> Writer<W, A>(WriterF<W, A> func)
            => new Writer<W, A>(func);

        public static WriterF<W, A> RunWriter<W, A>(Writer<W, A> m)
            => m.Func;

        // Uncurried
        public static WriterResult<W, A> RunWriter_Uncurried<W, A>(Writer<W, A> m)
            => RunWriter(m)();

        #endregion // Conversion

        public static Writer<W, Common.Void> Tell<W>(W w)
            => Writer<W, Common.Void>(() => WriterResult.New(Common.Void.Default, new[] { w }));

        #region IMonad

        public static Writer<W, A> Unit<W, A>(A a)
            => Writer(() => WriterResult.New(a, Enumerable.Empty<W>()));

        //public static Writer<W, B> Bind<W, A, B>(Writer<W, A> m, WriterFunc<W, A, B> f)
        //{
        //    return Writer(() =>
        //    {
        //        var resultA = m.Func();
        //        var writerB = f(resultA.Value);
        //        var resultB = writerB.Func();
        //        return WriterResult.New(resultB.Value, resultA.Output.Concat(resultB.Output));
        //    });
        //}

        public static Writer<W, B> Bind<W, A, B>(Writer<W, A> m, Func<A, Writer<W, B>> f)
        {
            return Writer(() =>
            {
                var resultA = m.Func();
                var writerB = f(resultA.Value);
                var resultB = writerB.Func();
                return WriterResult.New(resultB.Value, resultA.Output.Concat(resultB.Output));
            });
        }

        #endregion // IMonad
    }
}

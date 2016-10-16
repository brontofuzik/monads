using Monads.Common;

namespace Monads.Reader
{
    public class Reader<R, A>
    {
        public ReaderFunc<R, A> Func { get; }

        internal Reader(ReaderFunc<R, A> func)
        {
            Func = func;
        }  
    }

    public static class Reader
    {
        #region Conversion

        /// <summary>
        /// Retrieves a function of the current r.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <typeparam name="A"></typeparam>
        /// <param name="readerFunc"></param>
        /// <returns></returns>
        public static Reader<R, A> reader<R, A>(ReaderFunc<R, A> readerFunc)
            => new Reader<R, A>(readerFunc);

        /// <summary>
        /// Runs a reader and extracts the final first from it. (The inverse of reader.)
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <typeparam name="A"></typeparam>
        /// <param name="m"></param>
        /// <returns></returns>
        public static ReaderFunc<R, A> RunReader<R, A>(Reader<R, A> m)
            => m.Func;

        // Uncurried
        public static A RunReader<R, A>(Reader<R, A> m, R r)
            => RunReader(m)(r);

        #endregion // Conversion

        /// <summary>
        /// Retrieves the monad r.
        /// Haskell:
        /// Ask :: reader R R
        /// Ask  = reader $ \x -> x
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <returns></returns>
        public static Reader<R, R> Ask<R>()
            => reader<R, R>(Functions.Id);

        #region IMonad

        // Haskell:
        // Unit  :: A -> reader R A
        // Unit a = reader $ \_ -> a
        public static Reader<R, A> Unit<R, A>(this A a)
            => reader<R, A>(_ => a);

        // Haskell:
        // Bind    :: reader R A -> (A -> reader R B) -> reader R B
        // Bind m f = reader $ \r -> runReader (f (runReader m r)) r
        public static Reader<R, B> Bind<R, A, B>(this Reader<R, A> m, MonadicFunc<R, A, B> f)
            => reader<R, B>(e => RunReader(f(RunReader(m, e)), e));

        #endregion // IMonad

        // TODO
        // MapReader

        // TODO
        // WithReader
    }
}

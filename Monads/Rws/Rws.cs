using System.Linq;

namespace Monads.Rws
{
    public class Rws<R, W, S, A>
    {
        internal Rws(RwsM<R, W, S, A> func)
        {
            Func = func;
        }

        public RwsM<R, W, S, A> Func { get; }
    }

    public static class Rws
    {
        #region Conversion

        // rws
        public static Rws<R, W, S, A> rws<R, W, S, A>(RwsM<R, W, S, A> func)
            => new Rws<R, W, S, A>(func);

        // runRws
        public static RwsM<R, W, S, A> runRws<R, W, S, A>(Rws<R, W, S, A> m)
            => m.Func;

        #endregion // Conversion

        #region reader

        // ask
        public static Rws<R, W, S, R> ask<R, W, S>()
            => rws<R, W, S, R>((r, s) => RwsResult.New(r, Enumerable.Empty<W>(), s));

        #endregion // reader

        #region Writer

        // tell
        public static Rws<R, W, S, Common.Void> tell<R, W, S>(W w)
            => rws<R, W, S, Common.Void>((r, s) => RwsResult.New(Common.Void.Default, new[] { w }, s));

        #endregion // Writer

        #region State

        // get
        public static Rws<R, W, S, S> get<R, W, S, A>()
            => rws<R, W, S, S>((r, s) => RwsResult.New(s, Enumerable.Empty<W>(), s));

        // put
        public static Rws<R, W, S, Common.Void> put<R, W, S, A>(S s)
            => rws<R, W, S, Common.Void>((r, _) => RwsResult.New(Common.Void.Default, Enumerable.Empty<W>(), s));

        #endregion // State

        #region IMonad

        // unit
        public static Rws<R, W, S, A> unit<R, W, S, A>(this A a)
            => rws<R, W, S, A>((R r, S s) => new RwsResult<W, S, A>(a, Enumerable.Empty<W>(), s));

        // bind ???
        //public static Rws<R, W, S, B> bind<R, W, S, A, B>(this Rws<R, W, S, A> m, RwsFunc<R, W, S, A, B> f)
        //    where S : class
        //{
        //    return rws<R, W, S, A>((R r, S s) =>
        //    {
        //        var resultA = m.Func(r, s);
        //        var rwsB = f(resultA.Value);
        //        return RwsResult.New<W, S, B>(rwsB, resultA.Output, resultA.State ?? s);
        //    });
        //}

        #endregion // IMonad
    }
}

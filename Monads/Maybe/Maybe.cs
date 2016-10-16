using System;
using Monads.Common;

namespace Monads.Maybe
{
    // Maker
    public class MaybeMaker<A> : IMonadMaker<Maybe, A, Maybe<A>>
    {
        // Unit :: A -> Maybe A
        public Maybe<A> Unit(A value) => MaybeMonad.Unit(value);
    }

    public abstract class Maybe { }

    public sealed class Maybe<A>
        : Maybe,
        IGeneric<Maybe, A>,             // Maybe<T>
        IFunctor<Maybe, A, Maybe<A>>,   // Functor
        IMonad<Maybe, A>                // Monad
    {
        public static Maybe<A> Just(A value) => new Maybe<A>(value, true);

        public static Maybe<A> Nothing => new Maybe<A>(default(A), false);

        private Maybe(A value, bool hasValue)
        {
            Value = value;
            HasValue = hasValue;
        }

        public A Value { get; }

        public bool HasValue { get; }

        #region IFunctor

        public MaybeB FMap<B, MaybeB>(Func<A, B> f)
            where MaybeB : Maybe, IGeneric<Maybe, B>
        {
            var result = MaybeFunctor.FMap(this, f);
            return result.AsGeneric().Cast<MaybeB>();
        }

        public Maybe<B> FMap<B>(Func<A, B> f) => FMap<B, Maybe<B>>(f);

        #endregion IFunctor

        #region IMonad

        public MaybeB Bind<B, MaybeB>(Func<A, MaybeB> f)
            where MaybeB : Maybe, IGeneric<Maybe, B>, IMonad<Maybe, B>
        {
            Func<A, Maybe<B>> fa = a => f(a).AsGeneric().Cast<Maybe<B>>();
            var result = MaybeMonad.Bind(this, fa);
            return result.AsGeneric().Cast<MaybeB>();
        }

        public Maybe<B> Bind<B>(Func<A, Maybe<B>> f) => Bind<B, Maybe<B>>(f);

        #endregion // IMonad
    }

    public static class MaybeFunctor
    {
        // FMap
        public static Maybe<B> FMap<A, B>(Maybe<A> maybe, Func<A, B> f)
            => maybe.HasValue ? Maybe<B>.Just(f(maybe.Value)) : Maybe<B>.Nothing;
    }

    public static class MaybeMonad
    {
        // Unit
        public static Maybe<A> Unit<A>(A value) => Maybe<A>.Just(value);

        // TODO
        // Bind
        public static Maybe<B> Bind<A, B>(Maybe<A> m, Func<A, Maybe<B>> f) => null;
    }
}

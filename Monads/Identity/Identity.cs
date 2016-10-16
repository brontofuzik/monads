using System;
using Monads.Common;

namespace Monads.Identity
{
    // Maker
    public class IdentityMaker<A> : IMonadMaker<Identity, A, Identity<A>>
    {
        // Unit :: A -> Identity A
        public Identity<A> Unit(A value) => IdentityMonad.Unit(value);
    }

    public abstract class Identity { }

    public sealed class Identity<A> :
        Identity,
        IGeneric<Identity, A>,              // Identity<A>
        IFunctor<Identity, A, Identity<A>>, // Functor
        IMonad<Identity, A>                 // Monad
    {
        public Identity(A value)
        {
            Value = value;
        }

        public A Value { get; }

        #region IFunctor

        public IdentityB FMap<B, IdentityB>(Func<A, B> f)
            where IdentityB : Identity, IGeneric<Identity, B>
        {
            var result = IdentityFunctor.FMap(this, f);
            return result.AsGeneric().Cast<IdentityB>();
        }

        public Identity<B> FMap<B>(Func<A, B> f) => FMap<B, Identity<B>>(f);

        #endregion // IFunctor

        #region IMonad

        // Bind
        public IdentityB Bind<B, IdentityB>(Func<A, IdentityB> f)
            where IdentityB : Identity, IGeneric<Identity, B>, IMonad<Identity, B>
        {
            Func<A, Identity<B>> fa = a => f(a).AsGeneric().Cast<Identity<B>>();
            var result = IdentityMonad.Bind(this, fa);
            return result.AsGeneric().Cast<IdentityB>();
        }

        public Identity<B> Bind<B>(Func<A, Identity<B>> f) => Bind<B, Identity<B>>(f);

        #endregion // IMonad
    }

    internal static class IdentityFunctor
    {
        public static Identity<B> FMap<A, B>(Identity<A> identity, Func<A, B> f)
            => new Identity<B>(f(identity.Value));
    }

    internal static class IdentityMonad
    {
        public static Identity<A> Unit<A>(A a)
            => new Identity<A>(a);

        public static Identity<B> Bind<A, B>(Identity<A> m, Func<A, Identity<B>> f)
            => f(m.Value);
    }
}
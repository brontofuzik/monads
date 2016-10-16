using System;

namespace Monads.Common
{
    public interface IFunctor<Functor, A, FunctorA>
        where FunctorA : Functor, IGeneric<Functor, A>
    {
        FunctorB FMap<B, FunctorB>(Func<A, B> f)
            where FunctorB : Functor, IGeneric<Functor, B>;
    }

    // Not used
    //public interface IFunctorStatic<Functor>
    //{
    //    FunctorB FMap<A, B, FunctorA, FunctorB>(FunctorA a, Func<A, B> f)
    //        where FunctorA : IGeneric<Functor, A> // T<A>
    //        where FunctorB : IGeneric<Functor, B>; // T<B>
    //}
}

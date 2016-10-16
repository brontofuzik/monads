using System;

namespace Monads.Cont
{
    class Cont<A, R>
    {
        public readonly Func<Func<A, R>, R> cont;

        public Cont(Func<Func<A, R>, R> cont)
        {
            this.cont = cont;
        }

        public R Call(Func<A, R> c) => cont(c);
    }

    static class Cont
    {
        // cont
        public static Cont<A, R> cont<A, R>(Func<Func<A, R>, R> func) => new Cont<A, R>(func);

        // runCont
        public static Func<Func<A, R>, R> runCont<A, R>(Cont<A, R> cont) => cont.cont;
    }

    static class ContExtensions
    {
        // Unit : A -> Cont<A, R>
        public static Cont<A, R> Unit<A, R>(this A a)
            => new Cont<A, R>(c => c(a));

        // Bind : Cont<A, R> -> (A -> Cont<B, R>) -> Cont<B, R>
        public static Cont<B, R> Bind<A, B, R>(this Cont<A, R> m, Func<A, Cont<B, R>> f)
            => new Cont<B, R>(c => m.Call((A a) => f(a).Call(c)));
    }

    //delegate R Cont<A, R>(Func<A, R> k);

    //static class ContinuationExtensions
    //{
    //    // Unit : A -> Cont<A, R>
    //    public static Cont<A, R> Unit<A, R>(this A a)
    //        => c => c(a);

    //    // Bind : Cont<A, R> -> (A -> Cont<B, R>) -> Cont<B, R>
    //    public static Cont<B, R> Bind<A, B, R>(this Cont<A, R> m, Func<A, Cont<B, R>> f)
    //        => c => m((A a) => f(a)(c));
    //}
}
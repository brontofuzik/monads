using System;

namespace Monads.State
{
    /// <summary>
    /// A state monad parameterized by the type s of the state to carry.
    /// 
    /// The return function leaves the state unchanged,
    /// while >>= uses the final state of the first computation as the initial state of the second.
    /// </summary>
    /// <typeparam name="A"></typeparam>
    public class StateM<S, A>
    {
        public StateFunc<S, A> Func { get; set; }

        internal StateM(StateFunc<S, A> func)
        {
            Func = func;
        }
    }

    public static class StateM
    {
        #region Conversion

        /// <summary>
        /// Embed a simple state action into the monad.
        /// </summary>
        public static StateM<S, A> State<S, A>(StateFunc<S, A> stateFunc)
            => new StateM<S, A>(stateFunc);

        /// <summary>
        /// Unwrap a state monad computation as a function. (The inverse of state.)
        /// </summary>
        public static StateFunc<S, A> RunState<S, A>(StateM<S, A> m)
            => m.Func;

        /// <summary>
        /// Evaluate a state computation with the given initial state and return the final first, discarding the final state.
        /// </summary>
        public static Func<S, A> EvalState<S, A>(StateM<S, A> m)
            => s => RunState(m)(s).Value;

        // Uncurried
        public static A EvalState<S, A>(StateM<S, A> m, S s)
            => EvalState(m)(s);

        /// <summary>
        /// Evaluate a state computation with the given initial state and return the final state, discarding the final first.
        /// </summary>
        public static Func<S, S> ExecState<S, A>(StateM<S, A> m)
            => s => RunState(m)(s).State;

        // Uncurried
        public static S ExecState<S, A>(StateM<S, A> m, S s)
            => ExecState(m)(s);

        #endregion // Conversion

        /// <summary>
        /// Return the state from the internals of the monad.
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="A"></typeparam>
        /// <returns></returns>
        public static StateM<S, S> Get<S, A>()
            => State<S, S>(s => StateResult.New(s, s));

        /// <summary>
        /// Replace the state inside the monad.
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="A"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static StateM<S, Common.Void> Put<S, A>(S s)
            => State<S, Common.Void>(_ => StateResult.New(Common.Void.Default, s));

        #region IMonad

        // Unit
        public static StateM<S, A> Unit<S, A>(this A a)
            => State<S, A>(state => StateResult.New(a, state));

        // Bind
        public static StateM<S, B> Bind<S, A, B>(this StateM<S, A> m, MonadicFunc<S, A, B> f)
            => State<S, B>(state0 =>
            {
                var resultA = m.Func(state0);
                var a = resultA.Value;
                var state1 = resultA.State;
                return f(a).Func(state1);
            });

        #endregion // IMonad
    }

    public class StateMonadFactory<S, A>
    {
        public StateM<S, A> Unit(A a) => StateM.Unit<S, A>(a);

        public StateM<S, S> Get() => StateM.Get<S, A>();

        public StateM<S, Common.Void> Put(S s) => StateM.Put<S, A>(s);

        public Func<S, A> EvalState(StateM<S, A> m) => StateM.EvalState<S, A>(m);

        public A EvalState(StateM<S, A> m, S s) => StateM.EvalState<S, A>(m, s);

        public Func<S, S> ExecState(StateM<S, A> m) => StateM.ExecState<S, A>(m);

        public S ExecState(StateM<S, A> m, S s) => StateM.ExecState<S, A>(m, s);
    }
}

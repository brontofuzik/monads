namespace Monads.State
{
    public class StateResult<S, A> : Common.Pair<A, S>
    {
        public StateResult(A value, S state)
            : base(value, state)
        {
        }

        public A Value => First;

        public S State => Second;
    }

    public static class StateResult
    {
        public static StateResult<S, A> New<S, A>(A value, S state)
            => new StateResult<S, A>(value, state);
    }
}

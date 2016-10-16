namespace Monads.State
{
    /// <summary>
    /// Func :: S -> (A, S)
    /// </summary>
    public delegate StateResult<S, A> StateFunc<S, A>(S state);

    /// <summary>
    /// MonadicFunc :: A -> [S -> (B, S)]
    /// </summary>
    public delegate StateM<S, B> MonadicFunc<S, A, B>(A input);
}
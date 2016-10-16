namespace Monads.Reader
{
    /// <summary>
    /// Func :: R -> A
    /// </summary>
    public delegate A ReaderFunc<R, A>(R r);

    /// <summary>
    /// MonadicFunc :: A -> (R -> B)
    /// </summary>
    public delegate Reader<R, B> MonadicFunc<R, A, B>(A input);
}
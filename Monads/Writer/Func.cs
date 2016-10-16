namespace Monads.Writer
{
    /// <summary>
    /// WriterF :: () -> (A, W)
    /// </summary>
    public delegate WriterResult<W, A> WriterF<W, A>();

    /// <summary>
    /// WriterFunc :: A -> [() -> (B, W)]
    /// </summary>
    public delegate Writer<W, B> WriterFunc<W, A, B>(A input);
}
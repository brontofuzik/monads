namespace Monads.Rws
{
    public delegate RwsResult<W, S, A> RwsM<R, W, S, A>(R r, S s);

    public delegate Rws<R, W, S, B> RwsFunc<R, W, S, A, B>(A a);
}
using System;

namespace Monads.Common
{
    public delegate IMonad<B, TM> MonadFunc<A, B, TM>(A input);

    public interface IMonadMaker<Monad, A, MonadA>
        where MonadA : Monad, IGeneric<Monad, A>, IMonad<Monad, A>
    {
        MonadA Unit(A value);
    }

    public interface IMonad<Monad, A>
    {
        MonadB Bind<B, MonadB>(Func<A, MonadB> f)
            where MonadB : Monad, IGeneric<Monad, B>, IMonad<Monad, B>;
    }
}

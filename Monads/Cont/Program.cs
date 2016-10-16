using System;

namespace Monads.Cont
{
    public static class Program
    {
        static void Main(string[] args)
        {
            // Uncurried
            Func<Func<string, string>, string> comp =
                c => f1(i => f2(i, ch => f3(ch, c)));
            Console.WriteLine($"comp: {comp(id)}");

            // Curried
            Func<Func<string, string>, string> comp_curried =
                c => f1c<string>()(i => f2c<string>(i)(ch => f3c<string>(ch)(c)));
            Console.WriteLine($"comp_curried: {comp_curried(id)}");

            // Monad
            Cont<int, string> f1m = Cont.cont<int, string>(f1);
            Func<int, Cont<char, string>> f2m = compose<int, Func<Func<char, string>, string>, Cont<char, string>>
                (Cont.cont, f2c<string>);
            Func<char, Cont<string, string>> f3m = compose<char, Func<Func<string, string>, string>, Cont<string, string>>
                (Cont.cont, f3c<string>);

            Cont<string, string> comp_monad = f1m.Bind(f2m).Bind(f3m);
            Console.WriteLine($"comp_monad: {Cont.runCont(comp_monad)(id)}");
        }

        // f1 :: (int -> R) -> R
        static R f1<R>(Func<int, R> c)
        {
            int i = 1;
            return c(i);
        }
        // f1_curried :: () -> [(int -> R) -> R]
        static Func<Func<int, R>, R> f1c<R>()
            => (Func<int, R> c) =>
            {
                int i = 1;
                return c(i);
            };

        // f2 :: [int, (char -> R)] -> R
        static R f2<R>(int i, Func<char, R> c)
        {
            char ch = i.ToString()[0];
            return c(ch);
        }
        // f2_curried :: int -> [(char -> R) -> R]
        static Func<Func<char, R>, R> f2c<R>(int i)
            => (Func<char, R> c) =>
            {
                char ch = i.ToString()[0];
                return c(ch);
            };

        // f3 :: [char, (string -> R)] -> R
        static R f3<R>(char ch, Func<string, R> c)
        {
            string s = ch.ToString();
            return c(s);
        }
        // f3_curried :: char -> [(string -> R) -> R]
        static Func<Func<string, R>, R> f3c<R>(char ch)
            => (Func<string, R> c) =>
            {
                string s = ch.ToString();
                return c(s);
            };

        // id :: T -> T
        static T id<T>(T t) => t;

        // compose :: [(A -> B) -> (B -> C)] -> (A -> C)
        static Func<A, C> compose<A, B, C>(Func<B, C> f, Func<A, B> g) => a => f(g(a));
    }
}
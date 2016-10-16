namespace Monads.Common
{
    public class Pair<TFirst, TSecond>
    {
        public Pair(TFirst first, TSecond second)
        {
            First = first;
            Second = second;
        }

        public TFirst First { get; }

        public TSecond Second { get; }
    }
}
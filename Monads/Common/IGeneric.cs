using System.Runtime.Remoting;

namespace Monads.Common
{
    // TClass<T>
    public interface IGeneric<TypeCtor, T>
    {
    }

    // NEW
    public class Generic<TypeCtor, T>
    {
        public IGeneric<TypeCtor, T> generic;

        public Generic(IGeneric<TypeCtor, T> generic)
        {
            this.generic = generic;
        }

        public ConstrcutedType Cast<ConstrcutedType>()
            where ConstrcutedType : TypeCtor, IGeneric<TypeCtor, T>
        {
            return (ConstrcutedType)generic;
        }
    }

    public static class GenericExtensions
    {
        public static Generic<TypeCtor, T> AsGeneric<TypeCtor, T>(this IGeneric<TypeCtor, T> generic)
            => new Generic<TypeCtor, T>(generic);

        //public static ConstructedType ToConstructedType<ConstructedType, TypeCtor, T>(this IGeneric<TypeCtor, T> generic)
        //    where ConstructedType : TypeCtor, IGeneric<TypeCtor, T>
        //{
        //    return (ConstructedType)generic;
        //}
    }
}
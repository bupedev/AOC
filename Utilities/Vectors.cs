using System.Numerics;

namespace AOC.Utilities;

public class Vectors
{
    public sealed record Vec2<T>(T X, T Y) where T : INumber<T>
    {
        public static Vec2<T> operator +(Vec2<T> b, T a)
        {
            return new Vec2<T>(a + b.X, a + b.Y);
        }
        
        public static Vec2<T> operator +(T a, Vec2<T> b)
        {
            return new Vec2<T>(a + b.X, a + b.Y);
        }

        public static Vec2<T> operator *(T a, Vec2<T> b)
        {
            return new Vec2<T>(a * b.X, a * b.Y);
        }

        public static Vec2<T> operator *(Vec2<T> b, T a)
        {
            return new Vec2<T>(a * b.X, a * b.Y);
        }
    }
}
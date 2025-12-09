namespace AOC.Utilities;

public sealed record VecD3(double X, double Y, double Z)
{
    public static implicit operator VecD3((double, double, double) t) => new(t.Item1, t.Item2, t.Item3);
    
    public static implicit operator VecD3(double[] arr)
    {
        return arr.Length != 3 
            ? throw new ArgumentException($"Array must contain exactly 3 elements, but got {arr.Length}", nameof(arr)) 
            : new VecD3(arr[0], arr[1], arr[2]);
    }
    
    public static double EuclideanDistance(VecD3 a, VecD3 b)
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;
        var dz = a.Z - b.Z;
        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }
}
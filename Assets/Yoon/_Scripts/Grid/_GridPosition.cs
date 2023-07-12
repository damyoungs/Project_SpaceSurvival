


using System;

public struct _GridPosition : IEquatable<_GridPosition>
{
    public int x;
    public int z;

    public _GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public override bool Equals(object obj)
    {
        return obj is _GridPosition position &&
               x == position.x &&
               z == position.z;
    }

    public bool Equals(_GridPosition other)
    {
        return this == other;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }

    public override string ToString()
    {
        return $"x: {x}; z: {z}"; 
    }

    public static bool operator ==(_GridPosition a, _GridPosition b)
    {
        return a.x == b.x && a.z == b.z;
    }

    public static bool operator !=(_GridPosition a, _GridPosition b)
    {
        return !(a == b);
    }
}
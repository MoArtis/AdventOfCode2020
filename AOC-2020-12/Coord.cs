public struct Coord
{
    public override string ToString()
    {
        return $"x: {x} | y: {y}";
    }

    public static Coord operator -(Coord v) => new Coord(-v.x, -v.y);

    public static Coord operator +(Coord a, Coord b) => new Coord(a.x + b.x, a.y + b.y);

    public static Coord operator -(Coord a, Coord b) => new Coord(a.x - b.x, a.y - b.y);

    public static Coord operator *(Coord a, Coord b) => new Coord(a.x * b.x, a.y * b.y);

    public static Coord operator *(int a, Coord b) => new Coord(a * b.x, a * b.y);

    public static Coord operator *(Coord a, int b) => new Coord(a.x * b, a.y * b);

    public static Coord operator /(Coord a, int b) => new Coord(a.x / b, a.y / b);

    public static bool operator ==(Coord lhs, Coord rhs) => lhs.x == rhs.x && lhs.y == rhs.y;

    public static bool operator !=(Coord lhs, Coord rhs) => !(lhs == rhs);

    public static implicit operator Coord((int, int) tuple)
    {
        var (x, y) = tuple;
        return new Coord(x, y);
    }
    
    public bool Equals(Coord other)
    {
        return x == other.x && y == other.y;
    }

    public override bool Equals(object obj)
    {
        return obj is Coord other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (x * 397) ^ y;
        }
    }

    public static Coord Zero { get; } = new Coord(0, 0);

    public Coord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int x;
    public int y;
    
    public void Rotate(int rotation)
    {
        switch (rotation)
        {
            case 1:
                Rotate90();
                break;
            
            case 2:
                Rotate180();
                break;
            
            case 3:
                Rotate270();
                break;
        }
    }
    
    public void Rotate90()
    {
        var px = x;
        x = -y;
        y = px;
    }
    
    public void Rotate180()
    {
        x = -x;
        y = -y;
    }
    
    public void Rotate270()
    {
        var px = x;
        x = y;
        y = -px;
    }
}
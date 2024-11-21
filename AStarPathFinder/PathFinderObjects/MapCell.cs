namespace AStarPathFinder.PathFinderObjects;

public class MapCell
{
    private byte _walkCost;
    public int X { get; }
    public int Y { get; }
    public double G { get; set; }
    public double H { get; set; }
    public double F { get; set; }
    public int Index { get; set; }

    public byte WalkCost
    {
        get => _walkCost;
        set => _walkCost = value > 100 ? (byte)100 : value;
    }

    public bool IsWall =>
        WalkCost >= 100;

    public MapCell(int x, int y, byte walkCost)
    {
        X = x;
        Y = y;
        WalkCost = walkCost;
    }
}
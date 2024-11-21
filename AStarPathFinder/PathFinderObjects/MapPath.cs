using System.Collections.Generic;
using System.Linq;

namespace AStarPathFinder.PathFinderObjects;

public class MapPath : List<MapCell?>
{
    public MapPath()
    {
    }

    public MapPath(int count)
    {
        for (var i = 0; i < count; i++)
            Add(null);
    }

    public MapCell? GetCellAt(int x, int y) =>
        this.FirstOrDefault(c => c != null && c.X == x && c.Y == y);
}
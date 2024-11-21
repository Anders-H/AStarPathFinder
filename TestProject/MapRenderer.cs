using AStarPathFinder.PathFinderObjects;
using System.Drawing.Drawing2D;

namespace TestProject;

public class MapRenderer
{
    public Map Map { get; }
    public MapPath Path { get; }
    public int CellSize { get; }

    public MapRenderer(Map map, MapPath path, int cellSize)
    {
        Map = map;
        Path = path;
        CellSize = cellSize;
    }

    public void Draw(Graphics g, Font f, int offsetX, int offsetY)
    {
        var xPos = offsetX;
        var yPos = offsetY;
        g.SmoothingMode = SmoothingMode.None;
        g.InterpolationMode = InterpolationMode.NearestNeighbor;
        g.CompositingQuality = CompositingQuality.Default;

        for (var y = 0; y < Map.Height; y++)
        {
            for (var x = 0; x < Map.Width; x++)
            {
                using (var b = new SolidBrush(Color.FromArgb(Map.GetWalkCost(x, y) * 2, 0, 0)))
                    g.FillRectangle(b, xPos + 1, yPos + 1, CellSize - 2, CellSize - 2);
                
                var pathCell = Path.GetCellAt(x, y);

                if (pathCell != null)
                    g.FillRectangle(Brushes.CadetBlue, xPos + 5, yPos + 5, CellSize - 10, CellSize - 10);
                
                xPos += CellSize;
            }

            xPos = offsetX;
            yPos += CellSize;
        }
    }
}
using AStarPathFinder;
using AStarPathFinder.PathFinderObjects;

namespace TestProject;

public partial class Form1 : Form
{
    private Map? Map { get; set; }
    private MapPath? Path { get; set; }

    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        GenerateRandomMap();
    }

    private void Form1_Shown(object sender, EventArgs e)
    {
        Refresh();
    }

    private void GenerateRandomMap()
    {
        Map = new Map(75, 48);
        CreateTerrain(Map);
        var aStar = new AStar(Map);
        Path = aStar.FindPath(new Point(0, Map.Height - 1), new Point(Map.Width - 1, 0)) ?? [];
    }

    private void CreateTerrain(Map map)
    {
        var rnd = new Random();

        var walkCosts = new int[map.Width + 2, map.Height + 2];

        for (var y = 0; y < map.Height + 2; y++)
            for (var x = 0; x < map.Width + 2; x++)
                if (rnd.Next(0, 3) == 1)
                    walkCosts[x, y] = rnd.Next(-10, 300);

        int GetAverage(int[,] a, int x, int y) =>
            (a[x - 1, y - 1] + a[x, y - 1] + a[x + 1, y - 1]
             + a[x - 1, y] + a[x, y] + a[x + 1, y]
             + a[x - 1, y + 1] + a[x, y + 1] + a[x + 1, y + 1]) / 9;

        for (var y = 0; y < map.Height; y++)
        {
            for (var x = 0; x < map.Width; x++)
            {
                var avg = GetAverage(walkCosts, x + 1, y + 1);

                if (avg < 0)
                    avg = 0;
                else if (avg > 100)
                    avg = 100;

                map.SetWalkCost(x, y, (byte)avg);
            }
        }
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        if (Map == null || Path == null)
            return;

        e.Graphics.Clear(Color.Black);
        var m = new MapRenderer(Map, Path, 16);
        m.Draw(e.Graphics, Font, 0, 0);
    }

    private void Form1_MouseClick(object sender, MouseEventArgs e)
    {
        GenerateRandomMap();
        Invalidate();
    }
}
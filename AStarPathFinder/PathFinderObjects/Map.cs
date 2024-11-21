using System;
using System.Drawing;

namespace AStarPathFinder.PathFinderObjects;

public class Map
{
    private readonly MapCell?[,] _terrain;
    public int Width { get; }
    public int Height { get; }

    public Map(int width, int height)
    {
        if (width < 2 || width > 2000)
            throw new ArgumentOutOfRangeException(nameof(width));

        
        if (height < 2 || height > 2000)
            throw new ArgumentOutOfRangeException(nameof(height));
        
        Width = width;
        Height = height;
        _terrain = new MapCell[width, height];
    }

    public byte GetWalkCost(int x, int y)
    {
        if (x < 0 || x >= Width)
            throw new ArgumentOutOfRangeException(nameof(x));
            
        if (y < 0 || y >= Height)
            throw new ArgumentOutOfRangeException(nameof(y));
            
        var cell = _terrain[x, y];

        if (cell == null)
            return 0;

        return cell.WalkCost;
    }

    public void SetWalkCost(int x, int y, byte walkCost)
    {
        if (x < 0 || x >= Width)
            throw new ArgumentOutOfRangeException(nameof(x));
        
        if (y < 0 || y >= Height)
            throw new ArgumentOutOfRangeException(nameof(y));
        
        if (walkCost > 100)
            throw new ArgumentOutOfRangeException(nameof(walkCost));
        
        var cell = _terrain[x, y];
        
        if (cell == null)
        {
            cell = new MapCell(x, y, walkCost);
            _terrain[x, y] = cell;
            return;
        }

        cell.WalkCost = walkCost;
    }

    public void Clear()
    {
        for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
                _terrain[x, y] = null;
    }

    public MapCell? GetCell(int x, int y) =>
        GetCell(new Point(x, y));

    public MapCell? GetCell(Point p)
    {
        if (p.X < 0 || p.X >= Width || p.Y < 0 || p.Y >= Height)
            return null;

        return _terrain[p.X, p.Y];
    }

    public static double GetDistance(MapCell start, MapCell goal)
    {
        var actualDistance = Math.Sqrt((start.X - goal.X) * (start.X - goal.X) + (start.Y - goal.Y) * (start.Y - goal.Y));
        return actualDistance + (double)goal.WalkCost / 10;
    }

    public void Add(MapCell cell)
    {
        if (cell == null)
            throw new ArgumentNullException(nameof(cell));

        if (cell.X < 0 || cell.X >= Width)
            throw new ArgumentOutOfRangeException(nameof(cell.X));
        
        if (cell.Y < 0 || cell.Y >= Height)
            throw new ArgumentOutOfRangeException(nameof(cell.Y));
        
        if (_terrain[cell.X, cell.Y] != null)
            throw new SystemException();
        
        _terrain[cell.X, cell.Y] = cell;
    }

    public void Remove(MapCell cell)
    {
        if (cell == null)
            throw new ArgumentNullException(nameof(cell));
        
        if (_terrain[cell.X, cell.Y] == null)
            throw new SystemException();
        
        if (_terrain[cell.X, cell.Y] != cell)
            throw new SystemException();
        
        _terrain[cell.X, cell.Y] = null;
    }

    public bool Contains(MapCell cell) =>
        _terrain[cell.X, cell.Y] != null;

    public void SetWithHistory(int x, int y, MapCell cell) =>
        _terrain[x, y] = cell;

    public bool IsEmpty
    {
        get
        {
            for (var y = 0; y < Height; y++)
                for (var x = 0; x < Width; x++)
                    if (_terrain[x, y] != null)
                        return false;

            return true;
        }
    }

    public int CompareCells(MapCell c1, MapCell c2)
    {
        if (c1 == null)
            throw new ArgumentNullException(nameof(c1));

        if (c2 == null)
            throw new ArgumentNullException(nameof(c2));
        
        if (c1.F < c2.F)
            return -1;
        
        if (c1.F > c2.F)
            return 1;
        
        return 0;
    }
}
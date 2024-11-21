using System;
using System.Drawing;
using AStarPathFinder.PathFinderObjects;

namespace AStarPathFinder;

public class AStar
{
    private Map Map { get; }
    private Map ClosedSet { get; }
    private Map OpenSet { get; }
    private Map CameFrom { get; }
    private Map RuntimeGrid { get; }
    private PriorityQueue OrderedOpenSet { get; }

    public AStar(Map map)
    {
        Map = map;
        ClosedSet = new Map(Map.Width, Map.Height);
        OpenSet = new Map(Map.Width, Map.Height);
        CameFrom = new Map(Map.Width, Map.Height);
        RuntimeGrid = new Map(Map.Width, Map.Height);
        OrderedOpenSet = new PriorityQueue(Map);
    }

    public MapPath? FindPath(Point start, Point goal)
    {
        var startCell = Map.GetCell(start);

        if (startCell == null)
            throw new SystemException("Failed to get start cell.");

        var goalCell = Map.GetCell(goal);

        if (goalCell == null)
            throw new SystemException("Failed to get goal cell.");

        if (startCell == goalCell)
            return [startCell];
        
        ClosedSet.Clear();
        OpenSet.Clear();
        CameFrom.Clear();
        RuntimeGrid.Clear();
        OrderedOpenSet.Clear();
        startCell.G = 0.0;
        startCell.H = Map.GetDistance(startCell, goalCell);
        startCell.F = startCell.H;
        OpenSet.Add(startCell);
        OrderedOpenSet.Push(startCell);
        RuntimeGrid.Add(startCell);
        var neighbours = new MapPath(8);

        while (!OpenSet.IsEmpty)
        {
            var next = OrderedOpenSet.Pop();

            if (next == goalCell)
            {
                var result = ReconstructPath(CameFrom, CameFrom.GetCell(goalCell.X, goalCell.Y)!);
                result.Add(goalCell);
                return result;
            }
            
            OpenSet.Remove(next);
            ClosedSet.Add(next);
            StoreNeighbours(next, neighbours);
            
            foreach (var currentNeighbour in neighbours)
            {
                if (currentNeighbour == null || currentNeighbour.IsWall || ClosedSet.Contains(currentNeighbour))
                    continue;

                var tentativeG = RuntimeGrid.GetCell(next.X, next.Y)!.G + Map.GetDistance(next, currentNeighbour);
                
                if (!TentativeIsBetter(currentNeighbour, tentativeG, out var added))
                    continue;
                
                CameFrom.SetWithHistory(currentNeighbour.X, currentNeighbour.Y, next);
                
                if (!RuntimeGrid.Contains(currentNeighbour))
                    RuntimeGrid.Add(currentNeighbour);
                
                currentNeighbour.G = tentativeG;
                currentNeighbour.H = Map.GetDistance(currentNeighbour, goalCell);
                currentNeighbour.F = currentNeighbour.G + currentNeighbour.H;
                
                if (added)
                    OrderedOpenSet.Push(currentNeighbour);
                else
                    OrderedOpenSet.Update(currentNeighbour);
            }

        }

        return null;
    }

    private bool TentativeIsBetter(MapCell currentNeighbour, double tentativeG, out bool added)
    {
        if (!OpenSet.Contains(currentNeighbour))
        {
            OpenSet.Add(currentNeighbour);
            added = true;
            return true;
        }

        added = false;
        var cell = RuntimeGrid.GetCell(currentNeighbour.X, currentNeighbour.Y);

        if (cell == null)
            return false;

        return tentativeG < cell.G;
    }

    private void StoreNeighbours(MapCell c, MapPath neighbours)
    {
        neighbours[0] = Map.GetCell(c.X - 1, c.Y - 1);
        neighbours[1] = Map.GetCell(c.X, c.Y - 1);
        neighbours[2] = Map.GetCell(c.X + 1, c.Y - 1);
        neighbours[3] = Map.GetCell(c.X - 1, c.Y);
        neighbours[4] = Map.GetCell(c.X + 1, c.Y);
        neighbours[5] = Map.GetCell(c.X - 1, c.Y + 1);
        neighbours[6] = Map.GetCell(c.X, c.Y + 1);
        neighbours[7] = Map.GetCell(c.X + 1, c.Y + 1);
    }

    private static MapPath ReconstructPath(Map cameFrom, MapCell currentCell)
    {
        var path = new MapPath();
        ReconstructPathRecursive(cameFrom, currentCell, path);
        return path;

        void ReconstructPathRecursive(Map recursiveCameFrom, MapCell recursiveCurrentCell, MapPath result)
        {
            var item = recursiveCameFrom.GetCell(recursiveCurrentCell.X, recursiveCurrentCell.Y);

            if (item != null)
                ReconstructPathRecursive(recursiveCameFrom, item, result);
            
            result.Add(recursiveCurrentCell);
        }
    }
}
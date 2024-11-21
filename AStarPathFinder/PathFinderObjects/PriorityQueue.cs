using System.Collections.Generic;

namespace AStarPathFinder.PathFinderObjects;

public class PriorityQueue
{
    private List<MapCell> InnerList { get; } = [];
    private Map Map { get; }

    public PriorityQueue(Map map)
    {
        Map = map;
    }

    public void Clear() =>
        InnerList.Clear();

    public int Push(MapCell c)
    {
        var index1 = InnerList.Count;
        c.Index = index1;
        InnerList.Add(c);

        do
        {
            if (index1 <= 0)
                break;

            var index2 = (index1 - 1) / 2;
            
            if (Compare(index1, index2) < 0)
            {
                SwitchElements(index1, index2);
                index1 = index2;
                continue;
            }
            
            break;
        } while (true);

        return index1;
    }

    public MapCell Pop()
    {
        var result = InnerList[0];
        InnerList[0] = InnerList[^1];
        InnerList[0].Index = 0;
        InnerList.RemoveAt(InnerList.Count - 1);
        result.Index = -1;
        var p = 0;

        do
        {
            var pn = p;
            var p1 = 2 * p + 1;
            var p2 = 2 * p + 2;

            if (InnerList.Count > p1 && Compare(p, p1) > 0)
                p = p1;
            
            if (InnerList.Count > p2 && Compare(p, p2) > 0)
                p = p2;
            
            if (p == pn)
                break;
            
            SwitchElements(p, pn);
        } while (true);

        return result;
    }

    public void Update(MapCell cell)
    {
        var count = InnerList.Count;

        while (cell.Index - 1 > 0 && Compare(cell.Index - 1, cell.Index) > 0)
            SwitchElements(cell.Index - 1, cell.Index);
        
        while (cell.Index + 1 < count && Compare(cell.Index + 1, cell.Index) < 0)
            SwitchElements(cell.Index + 1, cell.Index);
    }

    private int Compare(int i1, int i2) =>
        Map.CompareCells(InnerList[i1], InnerList[i2]);

    private void SwitchElements(int i1, int i2)
    {
        (InnerList[i1], InnerList[i2]) = (InnerList[i2], InnerList[i1]);
        InnerList[i1].Index = i1;
        InnerList[i2].Index = i2;
    }
}
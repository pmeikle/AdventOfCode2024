using System.Numerics;
using System.Text;

namespace AdventOfCode2024.Day12;

public class Solution
{
    private static List<Tuple<int, int>> possibleDirections = new List<Tuple<int, int>>()
    {
        new Tuple<int, int>(1, 0),
        new Tuple<int, int>(0, 1),
        new Tuple<int, int>(-1, 0),
        new Tuple<int, int>(0, -1),
    };
    public static int FindTotalPriceOfFencing(string filename)
    {
        var grid = File.ReadAllLines(filename).Select(x => x.ToCharArray()).ToArray();
        var seen = new HashSet<Tuple<int, int>>();
        var cost = 0;
        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid[0].Length; j++)
            {
                if (seen.Contains(new Tuple<int, int>(i, j)))
                    continue;
                var plotsInRegion = GetPlotsInRegion(grid, i, j);
                seen.UnionWith(plotsInRegion);
                var costOfRegion = Perimeter(plotsInRegion, grid) * plotsInRegion.Count;
                cost += costOfRegion;
            }
        }
        return cost;
    }

    public static int FindTotalBulkPriceOfFencing(string filename)
    {
        var grid = File.ReadAllLines(filename).Select(x => x.ToCharArray()).ToArray();
        var seen = new HashSet<Tuple<int, int>>();
        var cost = 0;
        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid[0].Length; j++)
            {
                if (seen.Contains(new Tuple<int, int>(i, j)))
                    continue;
                var plotsInRegion = GetPlotsInRegion(grid, i, j);
                seen.UnionWith(plotsInRegion);
                var numberSidesInRegion = FindNumberOfSides(plotsInRegion, grid);
                var costOfRegion = numberSidesInRegion * plotsInRegion.Count;
                cost += costOfRegion;
            }
        }
        return cost;
    }

    public static int FindNumberOfSides(HashSet<Tuple<int, int>> plotsInRegion, char[][] grid)
    {
        var verticies = 0;
        foreach (var plot in plotsInRegion)
        {
            var spot = grid[plot.Item1][plot.Item2];
            for (var i = 0; i < possibleDirections.Count; i++)
            {
                var possibleDirection = possibleDirections[i];
                var otherDirection = possibleDirections[(i + 1) % 4];

                // Easy check, literal corner of the grid
                if (!IsNextSpotInGrid(grid, plot, possibleDirection) && !IsNextSpotInGrid(grid, plot, otherDirection))
                {
                    verticies++;
                } 
                // Check if one side isn't in grid and other is a different spot, corner
                else if (IsNextSpotInGrid(grid, plot, possibleDirection) && !IsNextSpotInGrid(grid, plot, otherDirection))
                {
                    var newSpot = grid[plot.Item1 + possibleDirection.Item1][plot.Item2 + possibleDirection.Item2];
                    if (newSpot != spot)
                    {
                        verticies++;
                    }
                } 
                // Check if other side isn't in grid and other is a different spot, corner
                else if (!IsNextSpotInGrid(grid, plot, possibleDirection) && IsNextSpotInGrid(grid, plot, otherDirection))
                {
                    var newSpot = grid[plot.Item1 + otherDirection.Item1][plot.Item2 + otherDirection.Item2];
                    if (newSpot != spot)
                    {
                        verticies++;
                    }
                } 
                // Both spots in grid, be smarter
                else
                {
                    var spot1 = grid[plot.Item1 + otherDirection.Item1][plot.Item2 + otherDirection.Item2];
                    var spot2 = grid[plot.Item1 + possibleDirection.Item1][plot.Item2 + possibleDirection.Item2];
                    var spot3 = grid[plot.Item1 + possibleDirection.Item1 + otherDirection.Item1][plot.Item2 + possibleDirection.Item2 + otherDirection.Item2];
                    if(spot1 == spot2 && spot == spot1 && spot != spot3) // Inward corner dawg
                        verticies++;
                    else if (spot != spot1 && spot != spot2) // Outward corder
                        verticies++;
                }
            }
        }
        return verticies;
    }

    private static int Perimeter(HashSet<Tuple<int,int>> plotsInRegion, char[][] grid)
    {
        var numPerimeter = 0;
        foreach (var plot in plotsInRegion)
        {
            var spot = grid[plot.Item1][plot.Item2];
            foreach (var possibleDirection in possibleDirections)
            {
                if (IsInGrid(grid, plot.Item1 + possibleDirection.Item1, plot.Item2 + possibleDirection.Item2))
                {
                    var newCoord = new Tuple<int, int>(plot.Item1 + possibleDirection.Item1, plot.Item2 + possibleDirection.Item2);
                    var newSpot = grid[newCoord.Item1][newCoord.Item2];
                    if (newSpot != spot)
                    {
                        numPerimeter++;
                    }
                }
                else
                {
                    numPerimeter++;
                }
            }
        }
        return numPerimeter;
    }

    private static HashSet<Tuple<int, int>> GetPlotsInRegion(char[][] grid, int i, int j)
    {
        var toReturn = new HashSet<Tuple<int, int>>();
        var spot = grid[i][j];
        var toProcess = new List<Tuple<int, int>>();
        toProcess.Add(Tuple.Create(i, j));
        toReturn.Add(Tuple.Create(i, j));
        while (toProcess.Any())
        {
            var plot = toProcess.First();
            toProcess.RemoveAt(0);
            foreach (var direction in possibleDirections)
            {
                if (IsInGrid(grid, plot.Item1 + direction.Item1, plot.Item2 + direction.Item2) && !toReturn.Contains(new Tuple<int, int>(plot.Item1 + direction.Item1, plot.Item2 + direction.Item2)))
                {
                    try
                    {
                        var newCoord = new Tuple<int, int>(plot.Item1 + direction.Item1, plot.Item2 + direction.Item2);
                        var newSpot = grid[newCoord.Item1][newCoord.Item2];
                        if (newSpot == spot)
                        {
                            toReturn.Add(newCoord);
                            toProcess.Add(newCoord);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }
        
        return toReturn;
    }

    public static bool IsNextSpotInGrid(char[][] grid, Tuple<int, int> plot, Tuple<int, int> direction)
    {
        return IsInGrid(grid, plot.Item1 + direction.Item1, plot.Item2 + direction.Item2);
    }
    private static bool IsInGrid(char[][] grid, int i, int j)
    {
        return i >= 0 && i < grid.Length && j >= 0 && j < grid[0].Length;
    }
}

public class Test
{
    [TestCase("Day12/simple.txt", 1930)]
    [TestCase("Day12/complex.txt", 1396298)]
    public void ShouldFindTotalPriceOfFences(string filename, int expected)
    {
        Assert.That(Solution.FindTotalPriceOfFencing(filename), Is.EqualTo(expected));
    }
    
    [TestCase("Day12/simple.txt", 1206)]
    [TestCase("Day12/complex.txt", 853588)]
    public void ShouldFindTotalBulkPriceOfFences(string filename, int expected)
    {
        Assert.That(Solution.FindTotalBulkPriceOfFencing(filename), Is.EqualTo(expected));
    }
}
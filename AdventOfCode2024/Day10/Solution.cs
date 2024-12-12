using System.Text;

namespace AdventOfCode2024.Day10;

public class Solution
{
    public static List<Tuple<int, int>> PossibleDirections = new List<Tuple<int, int>>()
    {
        new Tuple<int, int>(1,0),
        new Tuple<int, int>(0,1),
        new Tuple<int, int>(-1,0),
        new Tuple<int, int>(0,-1)
    };
    public static int FindSumTrailheadScores(string filename)
    {
        var mountain = File.ReadAllLines(filename).Select(x => x.ToCharArray().ToList().Select(y => int.Parse(y.ToString())).ToArray()).ToArray();
        var trailheads = FindTrailheads(mountain);
        var allTrailheadScore = 0;
        foreach (var trailhead in trailheads)
        {
            var reachable = FindReachablePeaks(trailhead, mountain);
            var trailheadScore = FindReachablePeaks(trailhead, mountain).Count();
            allTrailheadScore += trailheadScore;
        }

        return allTrailheadScore;
    }

    public static int FindSumTrailheadRatings(string filename)
    {
        var mountain = File.ReadAllLines(filename).Select(x => x.ToCharArray().ToList().Select(y => int.Parse(y.ToString())).ToArray()).ToArray();
        var trailheads = FindTrailheads(mountain);
        var allTrailheadRating = 0;
        foreach (var trailhead in trailheads)
        {
            allTrailheadRating += FindNumberOfWaysToTheTop(trailhead, mountain);
        }
        return allTrailheadRating;
    }

    private static int FindNumberOfWaysToTheTop(Tuple<int, int> current, int[][] mountain)
    {
        var numberOfWaysToTheTop = 0;
        if (mountain[current.Item1][current.Item2] == 9)
            return 1; // Found one way to the top!
        
        foreach (var direction in PossibleDirections)
        {
            var possibleNewY = direction.Item1 + current.Item1;
            var possibleNewX = direction.Item2 + current.Item2;
            if(possibleNewY < 0 || possibleNewX < 0 || possibleNewY > mountain.Length - 1 || possibleNewX > mountain.Length - 1)
                continue; // Out of bounds!
            var currentElevation = mountain[current.Item1][current.Item2];
            var elevationOfPossibleSpot = mountain[possibleNewY][possibleNewX];
            if(elevationOfPossibleSpot == currentElevation + 1)
                numberOfWaysToTheTop += FindNumberOfWaysToTheTop(new Tuple<int, int>(possibleNewY, possibleNewX), mountain);
        }
        
        return numberOfWaysToTheTop;
    }

    private static HashSet<Tuple<int, int>> FindReachablePeaks(Tuple<int, int> current, int[][] mountain, HashSet<Tuple<int, int>>? visited = null)
    {
        if (visited == null)
            visited = new HashSet<Tuple<int, int>>();
        
        var reachablePeaks = new HashSet<Tuple<int, int>>();
        if (mountain[current.Item1][current.Item2] == 9)
        {
            reachablePeaks.Add(new Tuple<int, int>(current.Item1, current.Item2));
            return reachablePeaks;
        }

        foreach (var direction in PossibleDirections)
        {
            var possibleNewY = direction.Item1 + current.Item1;
            var possibleNewX = direction.Item2 + current.Item2;
            if(possibleNewY < 0 || possibleNewX < 0 || possibleNewY > mountain.Length - 1 || possibleNewX > mountain.Length - 1)
                continue; // Out of bounds!
            if (visited.Contains(new Tuple<int, int>(possibleNewY, possibleNewX)))
                continue; // Already been here via another path, prune
            var currentElevation = mountain[current.Item1][current.Item2];
            var elevationOfPossibleSpot = mountain[possibleNewY][possibleNewX];
            if(elevationOfPossibleSpot == currentElevation + 1)
                reachablePeaks.UnionWith(FindReachablePeaks(new Tuple<int, int>(possibleNewY, possibleNewX), mountain, visited));
        }
        
        return reachablePeaks;
    }


    public static HashSet<Tuple<int, int>> FindTrailheads(int[][] grid)
    {
        var trailheads = new HashSet<Tuple<int, int>>();
        for (int row = 0; row < grid.Length; row++)
        {
            for (int col = 0; col < grid[row].Length; col++)
            {
                var check = grid[row][col];
                if(check == 0)
                    trailheads.Add(new Tuple<int, int>(row, col));
            }
        }
        return trailheads;
    }
}

public class Test
{
    [TestCase("Day10/simple.txt", 36)]
    [TestCase("Day10/complex.txt", 816)]
    public void ShouldFindSumTrailheadScores(string filename, int expected)
    {
        Assert.That(Solution.FindSumTrailheadScores(filename), Is.EqualTo(expected));
    }
    
    [TestCase("Day10/simple.txt", 81)]
    [TestCase("Day10/complex.txt", 1960)]
    public void ShouldFindSumTrailheadRatings(string filename, int expected)
    {
        Assert.That(Solution.FindSumTrailheadRatings(filename), Is.EqualTo(expected));
    }
}
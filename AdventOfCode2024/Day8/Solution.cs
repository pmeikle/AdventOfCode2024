namespace AdventOfCode2024.Day8;

public class Solution
{
    public static int FindNumberAntiNodes(string fileName, bool goDeep = false)
    {
        var lines = File.ReadAllLines(fileName);
        var xBound = lines[0].Length;
        var yBound = lines.Length;
        var signalLocations = new Dictionary<char, List<Tuple<int, int>>>();
        var antinodes = new HashSet<Tuple<int, int>>();
        
        // Parse junk
        for (var j = 0; j < yBound; j++)
        {
            var line = lines[j];
            for (var i = 0; i < line.Length; i++)
            {
                var spot = line[i];
                if (spot != '.')
                {
                    if (signalLocations.ContainsKey(spot))
                    {
                        signalLocations[spot].Add(new Tuple<int, int>(i, j));
                    }
                    else
                    {
                        signalLocations.Add(spot, new List<Tuple<int, int>>() { new Tuple<int, int>(i, j) });
                    }
                }
            }
        }
        
        // Process junk
        foreach (var key in signalLocations.Keys)
        {
            for (var i = 0; i < signalLocations[key].Count; i++)
            {
                var firstSignal = signalLocations[key][i];
                for (var j = i + 1; j < signalLocations[key].Count; j++)
                {
                    var secondSignal = signalLocations[key][j];
                    var xSlope = firstSignal.Item1 - secondSignal.Item1;
                    var ySlope = firstSignal.Item2 - secondSignal.Item2;
                    var firstAntiNodeX = xSlope + firstSignal.Item1;
                    var firstAntiNodeY = ySlope + firstSignal.Item2;
                    if (goDeep)
                    {
                        antinodes.Add(new Tuple<int, int>(firstSignal.Item1, firstSignal.Item2));
                        antinodes.Add(new Tuple<int, int>(secondSignal.Item1, secondSignal.Item2));
                    }
                    
                    if (firstAntiNodeX >= 0 && firstAntiNodeX < xBound && firstAntiNodeY >= 0 && firstAntiNodeY < yBound)
                    {
                        antinodes.Add(new Tuple<int, int>(firstAntiNodeX, firstAntiNodeY));
                        while (goDeep)
                        {
                            firstAntiNodeX += xSlope;
                            firstAntiNodeY += ySlope;
                            if (firstAntiNodeX >= 0 && firstAntiNodeX < xBound && firstAntiNodeY >= 0 && firstAntiNodeY < yBound)
                            {
                                antinodes.Add(new Tuple<int, int>(firstAntiNodeX, firstAntiNodeY));
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    
                    
                    var secondAntiNodeX = secondSignal.Item1 - xSlope;
                    var secondAntiNodeY = secondSignal.Item2 - ySlope;
                    if (secondAntiNodeX >= 0 && secondAntiNodeX < xBound && secondAntiNodeY >= 0 && secondAntiNodeY < yBound)
                    {
                        antinodes.Add(new Tuple<int, int>(secondAntiNodeX, secondAntiNodeY));
                        while (goDeep)
                        {
                            secondAntiNodeX -= xSlope;
                            secondAntiNodeY -= ySlope;
                            if (secondAntiNodeX >= 0 && secondAntiNodeX < xBound && secondAntiNodeY >= 0 && secondAntiNodeY < yBound)
                            {
                                antinodes.Add(new Tuple<int, int>(secondAntiNodeX, secondAntiNodeY));
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        return antinodes.Count;
    }

    public static int FindNumberAntiNodesPt2(string fileName)
    {
        return FindNumberAntiNodes(fileName, true);
    }
}

public class Test
{
    [Test]
    public void SimpleShouldFindProperSum()
    {
        Assert.That(Solution.FindNumberAntiNodes("Day8/Simple.txt"), Is.EqualTo(14));
    }

    [Test]
    public void ComplexFindNumberOfGuardMoves()
    {
        Assert.That(Solution.FindNumberAntiNodes("Day8/Complex.txt"), Is.EqualTo(371));
    }
    
    [Test]
    public void SimpleShouldFindProperSumPt2()
    {
        Assert.That(Solution.FindNumberAntiNodesPt2("Day8/Simple.txt"), Is.EqualTo(34));
    }
    
    [Test]
    public void SimpleEdgeShouldFindProperSumPt2()
    {
        Assert.That(Solution.FindNumberAntiNodesPt2("Day8/edge.txt"), Is.EqualTo(9));
    }

    [Test]
    public void ComplexFindNumberOfGuardMovesPt2()
    {
        Assert.That(Solution.FindNumberAntiNodesPt2("Day8/Complex.txt"), Is.EqualTo(1229));
    }
}
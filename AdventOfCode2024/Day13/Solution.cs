using System.Numerics;
using System.Text;

namespace AdventOfCode2024.Day13;

public class Solution
{
    public static long FindFewestTokensToGetAllPrizes(string filename)
    {
        var games = ParseGames(filename);
        long totalCost = 0;
        foreach (var game in games)
        {
            totalCost += game.CostToSolve();
        }
        return totalCost;
    }
    
    public static long FindFewestTokensToGetAllPrizesPt2(string filename)
    {
        var games = ParseGames(filename);
        long totalCost = 0;
        foreach (var game in games)
        {
            game.First.C += 10000000000000;
            game.Second.C += 10000000000000;
            totalCost += game.CostToSolve();
        }
        return totalCost;
    }
    public static List<ClawGame> ParseGames(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var toReturn = new List<ClawGame>();
        for (int i = 0; i < lines.Length; i += 4)
        {
            var buttonA = lines[i];
            var buttonB = lines[i + 1];
            var prizes = lines[i + 2];
            var aX = long.Parse(buttonA.Substring(buttonA.IndexOf('+')+1, buttonA.IndexOf(',')-buttonA.IndexOf('+')-1));
            var aY = long.Parse(buttonA.Substring(buttonA.LastIndexOf('+')+1).Trim());
            
            var bX = long.Parse(buttonB.Substring(buttonB.IndexOf('+')+1, buttonB.IndexOf(',')-buttonB.IndexOf('+')-1));
            var bY = long.Parse(buttonB.Substring(buttonB.LastIndexOf('+')+1).Trim());
            
            var prizeX = long.Parse(prizes.Substring(prizes.IndexOf('=')+1, prizes.IndexOf(',')-prizes.IndexOf('=')-1));
            var prizeY = long.Parse(prizes.Substring(prizes.LastIndexOf('=')+1).Trim());
            toReturn.Add(new ClawGame()
            {
                First = new Equation(){A = aX, B = bX, C = prizeX},
                Second = new Equation(){A = aY, B = bY, C = prizeY},
            });
        }
        return toReturn;
    }
}

public class ClawGame
{
    public Equation First { get; set; }
    public Equation Second { get; set; }

    public long CostToSolve()
    {
        var x = (First.C * Second.B - Second.C * First.B) / (First.A * Second.B - Second.A * First.B);
        var y = (Second.C - x * Second.A) / Second.B;
        if(First.Check(x, y) && Second.Check(x, y))
            return x * 3 + y;
        return 0;
    }
}

public class Equation
{
    public long A { get; set; }
    public long B { get; set; }
    public long C { get; set; }

    public bool Check(long x, long y)
    {
        return C == A * x + B * y;
    }
}

public class Test
{
    [TestCase("Day13/simple.txt", 480)]
    [TestCase("Day13/complex.txt", 26599)]
    public void ShouldFindFewestTokensToWinAll(string filename, long expected)
    {
        Assert.That(Solution.FindFewestTokensToGetAllPrizes(filename), Is.EqualTo(expected));
    }
    
    [TestCase("Day13/simple.txt", 875318608908)]
    [TestCase("Day13/complex.txt", 106228669504887)]
    public void ShouldFindFewestTokensToWinAllPt2(string filename, long expected)
    {
        Assert.That(Solution.FindFewestTokensToGetAllPrizesPt2(filename), Is.EqualTo(expected));
    }
}
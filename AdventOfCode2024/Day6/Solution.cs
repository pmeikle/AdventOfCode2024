using System.Text;
using NUnit.Framework.Constraints;

namespace AdventOfCode2024.Day6;

public class Solution
{
    
    public static int FindNumberOfGuardMoves(string filename)
    {
        // Read input, find guard starting position
        var board = File.ReadAllLines(filename).Select(x => new StringBuilder(x)).ToList();
        var visited = new HashSet<Tuple<int, int>>();
        var guardX = 0;
        var guardY = 0;
        foreach (var row in board)
        {
            if (row.ToString().Contains('^'))
            {
                guardY = board.IndexOf(row);
                guardX = row.ToString().IndexOf('^');
            }
        }
        
        
        // Move until hes out of range
        var velocityX = 0;
        var velocityY = -1;
        visited.Add(Tuple.Create(guardX, guardY));
        
        while (guardY + velocityY >= 0 && guardY + velocityY < board.Count && guardX + velocityX >= 0 && guardX + velocityX < board[0].Length)
        {
            board[guardY][guardX] = 'X';
            if (board[guardY + velocityY][guardX + velocityX] == '#')
            {
                Turn(ref velocityX, ref velocityY);
            }
            guardX += velocityX;
            guardY += velocityY;
            visited.Add(Tuple.Create(guardX, guardY));
        }

        // Add 1, off by 1 cause it didn't mark the last spot as X whatever
        return visited.Count();
    }

    public static int FindPossibleLoops(string filename)
    {
        // Read input, find guard starting position
        var board = File.ReadAllLines(filename).Select(x => new StringBuilder(x)).ToList();
        var guardX = 0;
        var guardY = 0;
        var visited = new HashSet<Tuple<int, int>>();
        var visitedWithVelocities = new HashSet<Tuple<int, int, int, int>>();
        foreach (var row in board)
        {
            if (row.ToString().Contains('^'))
            {
                guardY = board.IndexOf(row);
                guardX = row.ToString().IndexOf('^');
            }
        }
        visited.Add(Tuple.Create(guardX, guardY));
        // Move until hes out of range
        var velocityX = 0;
        var velocityY = -1;
        visitedWithVelocities.Add(Tuple.Create(guardX, guardY, velocityX, velocityY));
        
        var numPossibleLoops = 0;
        for (var y = 0; y < board.Count; y++)
        {
            for (var x = 0; x < board[y].Length; x++)
            {
                var spot = board[y][x];
                if (spot == '#')
                    continue;
                var cloneBoard = board.Select(x => new StringBuilder(x.ToString())).ToList();
                cloneBoard[y][x] = '#';
                if (CheckForLoop(cloneBoard, guardY, guardX, velocityX, velocityY))
                {
                    numPossibleLoops++;
                }
            }
        }

        foreach (var row in board)
        {
            Console.WriteLine(row);
        }
        return numPossibleLoops;
    }

    private static bool CheckForLoop(List<StringBuilder> board, int guardY, int guardX, int velocityX, int velocityY)
    {
        var plausibleVelocityX = velocityX;
        var plausibleVelocityY = velocityY;
        var visitedWithVelocity = new HashSet<Tuple<int, int, int, int>>();
        try
        {
            var currPosX = guardX;
            var currPosY = guardY;
            while (true)
            {
                if (visitedWithVelocity.Contains(new Tuple<int, int, int, int>(currPosX, currPosY, plausibleVelocityX, plausibleVelocityY)))
                {
                    return true;
                }
                visitedWithVelocity.Add(new Tuple<int, int, int, int>(currPosX, currPosY, plausibleVelocityX, plausibleVelocityY));

                var plausiblePosX = currPosX + plausibleVelocityX;
                var plausiblePosY = currPosY + plausibleVelocityY;
                
                var spot = board[plausiblePosY][plausiblePosX];
                if (spot == '#') // Going to hit a wall, check curr square. not when you're in the same spot tho
                {
                    Turn(ref plausibleVelocityX, ref plausibleVelocityY);
                }
                else
                {
                    currPosX = plausiblePosX;
                    currPosY = plausiblePosY;
                }
            }
        }
        catch (Exception e)
        {
            // Got out of the board, no loop
            return false;
        }
        
    }

    private static void Turn(ref int velocityX, ref int velocityY)
    {
        if (velocityX == 1)
        {
            velocityX = 0;
            velocityY = 1;
        } else if (velocityX == -1)
        {
            velocityX = 0;
            velocityY = -1;
        } else if (velocityY == 1)
        {
            velocityX = -1;
            velocityY = 0;
        } else if (velocityY == -1)
        {
            velocityX = 1;
            velocityY = 0;
        }
    }
}

public class Test
{
    [Test]
    public void SimpleFindNumberOfGuardMoves()
    {
        Assert.That(Solution.FindNumberOfGuardMoves("Day6/Simple.txt"), Is.EqualTo(41));
    }
    
    [Test]
    public void ComplexFindNumberOfGuardMoves()
    {
        Assert.That(Solution.FindNumberOfGuardMoves("Day6/Complex.txt"), Is.EqualTo(5199));
    }
    
    
    [Test]
    public void SimpleFindNumberOfGuardLoops()
    {
        Assert.That(Solution.FindPossibleLoops("Day6/Simple.txt"), Is.EqualTo(6));
    }
    
    [Test]
    public void ComplexFindNumberOfGuardLoops()
    {
        Assert.That(Solution.FindPossibleLoops("Day6/Complex.txt"), Is.EqualTo(1915));
    }
}
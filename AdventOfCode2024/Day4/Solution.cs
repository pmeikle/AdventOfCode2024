namespace AdventOfCode2024.Day4;

public class Solution
{
    public static int NumOfXmases(string filename)
    {
        var wordSearch = File.ReadAllLines(filename);
        var numFound = 0;
        for (var i = 0; i < wordSearch.Length; i++)
        {
            for (var k = 0; k < wordSearch[i].Length; k++)
            {
                numFound += NumOfXmases(wordSearch, k, i);
            }
        }
        return numFound;
    }
    
    public static int NumOfSams(string filename)
    {
        var wordSearch = File.ReadAllLines(filename);
        var numFound = 0;
        for (var i = 0; i < wordSearch.Length; i++)
        {
            for (var k = 0; k < wordSearch[i].Length; k++)
            {
                if (wordSearch[i][k] == 'A')
                {
                    // Too lazy to check bounds, just swallow index out of bounds and move on with life
                    try
                    {
                        var one = wordSearch[i-1][k-1] + "A" + wordSearch[i+1][k+1];
                        var two = wordSearch[i+1][k-1] + "A" + wordSearch[i-1][k+1];
                        if ((one == "MAS" || one == "SAM") && (two == "MAS" || two == "SAM"))
                        {
                            numFound++;
                        }
                    }
                    catch (Exception e)
                    {
                        
                    }
                }
            }
        }
        return numFound;
    }

    private static int NumOfXmases(string[] wordSearch, int x, int y)
    {
        var numToReturn = 0;
        // Right
        if (x + 4 <= wordSearch[0].Length)
        {
            var thing = wordSearch[y].Substring(x, 4);
            if (wordSearch[y].Substring(x, 4) == "XMAS")
            {
                numToReturn++;
            }
        }
        // Left
        if (x - 3 >= 0)
        {
            var thing = wordSearch[y].Substring(x-3, 4);
            if (wordSearch[y].Substring(x-3, 4) == "SAMX")
            {
                numToReturn++;
            }
        }
        // Up
        if (y - 3 >= 0)
        {
            var ok = wordSearch[y][x];
            var word = "" + wordSearch[y][x] + wordSearch[y-1][x] + wordSearch[y-2][x] + wordSearch[y-3][x];
            if(word == "XMAS")
                numToReturn++;
        }
        // Down
        if (y + 3 < wordSearch.Length)
        {
            var word = "" + wordSearch[y][x] + wordSearch[y + 1][x] + wordSearch[y + 2][x] + wordSearch[y + 3][x];
            if(word == "XMAS")
                numToReturn++;
        }
        // Up Right
        if (x + 4 <= wordSearch[0].Length && y - 3 >= 0)
        {
            var word = "" + wordSearch[y][x] + wordSearch[y - 1][x + 1] + wordSearch[y - 2][x + 2] + wordSearch[y - 3][x + 3];
            if(word == "XMAS")
                numToReturn++;
        }
        // Up Left
        if (x - 3 >= 0 && y - 3 >= 0)
        {
            var word = "" + wordSearch[y][x] + wordSearch[y - 1][x - 1] + wordSearch[y - 2][x - 2] + wordSearch[y - 3][x - 3];
            if(word == "XMAS")
                numToReturn++;
        }
        // Down Right
        if (x + 4 <= wordSearch[0].Length && y + 3 < wordSearch.Length)
        {
            var word = "" + wordSearch[y][x] + wordSearch[y + 1][x + 1] + wordSearch[y + 2][x + 2] + wordSearch[y + 3][x + 3];
            if(word == "XMAS")
                numToReturn++;
        }
        // Down Left
        if (x - 3 >= 0 && y + 3 < wordSearch.Length)
        {
            var word = "" + wordSearch[y][x] + wordSearch[y + 1][x - 1] + wordSearch[y + 2][x - 2] + wordSearch[y + 3][x - 3];
            if(word == "XMAS")
                numToReturn++;
        }
        return numToReturn;
    }
}

public class Test
{
    [Test]
    public void SimpleShouldCalcSumOfMultiplications()
    {
        var result = Solution.NumOfXmases("Day4/Simple.txt");
        Assert.That(result, Is.EqualTo(18));
    }

    [Test]
    public void ComplexShouldCalcSumOfMultiplications()
    {
        var result = Solution.NumOfXmases("Day4/Complex.txt");
        Assert.That(result, Is.EqualTo(2654));
    }


    [Test]
    public void SimpleShouldFindSams()
    {
        var result = Solution.NumOfSams("Day4/Simple.txt");
        Assert.That(result, Is.EqualTo(9));
    }
    
    [Test]
    public void ComplexShouldFindSams()
    {
        var result = Solution.NumOfSams("Day4/Complex.txt");
        Assert.That(result, Is.EqualTo(1990));
    }
}
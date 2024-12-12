using System.Numerics;
using System.Text;

namespace AdventOfCode2024.Day11;

public class Solution
{
    public static long FindNumStones(string filename)
    {
        var numbers = File.ReadAllText(filename).Trim().Split(' ').Select(x => new BigInteger(long.Parse(x))).ToList();
        var memoizedNumbers = new Dictionary<Tuple<BigInteger, int>, long>();
        var answer = numbers.Sum(x => BlinkV2(x, 25, memoizedNumbers));
            
        return answer;
    }
    
    public static long FindNumStonesPt2(string filename)
    {
        var numbers = File.ReadAllText(filename).Trim().Split(' ').Select(x => new BigInteger(long.Parse(x))).ToList();
        var memoizedNumbers = new Dictionary<Tuple<BigInteger, int>, long>();
        var answer = numbers.Sum(x => BlinkV2(x, 75, memoizedNumbers));
            
        return answer;
    }

    private static long BlinkV2(BigInteger number, int roundsLeft, Dictionary<Tuple<BigInteger, int>, long> memoizedScores)
    {
        long result = 0;
        if(memoizedScores.ContainsKey(new Tuple<BigInteger, int>(number, roundsLeft))) 
            return memoizedScores[new Tuple<BigInteger, int>(number, roundsLeft)];
        if (roundsLeft == 0)
            return 1;
        if (number == 0)
        {
            result = BlinkV2(1, roundsLeft - 1, memoizedScores);
            
        } else if (number.ToString().Length % 2 == 0)
        {
            var numAsString = number.ToString();
            var first = BlinkV2(BigInteger.Parse(numAsString.Substring(0, numAsString.Length/2)), roundsLeft - 1, memoizedScores);
            var second = BlinkV2(BigInteger.Parse(numAsString.Substring(numAsString.Length/2)), roundsLeft - 1, memoizedScores);
            
            result = first + second;
        }
        else
        {
            result = BlinkV2(number * 2024, roundsLeft - 1, memoizedScores);
        }

        memoizedScores[new Tuple<BigInteger, int>(number, roundsLeft)] = result;   

        return result;
    }

    private static List<BigInteger> Blink(List<BigInteger> numbers)
    {
        var toReturn = new List<BigInteger>();
        foreach (var number in numbers)
        {
            if (number == 0)
            {
                toReturn.Add(1);
            } else if (number.ToString().Length % 2 == 0)
            {
                var digits = number.ToString();
                var firstHalf = digits.Substring(0, digits.Length / 2);
                var secondHalf = digits.Substring(digits.Length / 2);
                toReturn.Add(BigInteger.Parse(firstHalf));
                toReturn.Add(BigInteger.Parse(secondHalf));
            } else
            {
                toReturn.Add(number * new BigInteger(2024));  
            }
            
            
        }
        return toReturn;
    }
}

public class Test
{
    [TestCase("Day11/simple.txt", 55312)]
    [TestCase("Day11/complex.txt", 186424)]
    public void ShouldFindNumStones25Blinks(string filename, int expected)
    {
        Assert.That(Solution.FindNumStones(filename), Is.EqualTo(expected));
    }
    
    [TestCase("Day11/complex.txt", 219838428124832)]
    public void ShouldFindNumStones75Blinks(string filename, long expected)
    {
        Assert.That(Solution.FindNumStonesPt2(filename), Is.EqualTo(expected));
    }
}
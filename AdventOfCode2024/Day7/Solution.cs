namespace AdventOfCode2024.Day7;

public class Solution
{
    public static long FindSumValidCalibrations(string filename, bool alsoConcat = false)
    {
        var lines = File.ReadAllLines(filename);
        long sum = 0;

        foreach (var line in lines)
        {
            var firstSplit = line.Split(":");
            var target = long.Parse(firstSplit[0]);
            var numbers = firstSplit[1].Trim().Split(" ").Select(long.Parse).ToList();
            var possibleResults = new List<long>();
            foreach (var number in numbers)
            {
                if (possibleResults.Count == 0)
                {
                    possibleResults.Add(number);
                    continue;
                }

                var updatedResults = new List<long>();
                foreach (var possibleResult in possibleResults)
                {
                    var added = number + possibleResult;
                    var multiplied = number * possibleResult;
                    if (alsoConcat)
                    {
                        var concat = long.Parse(possibleResult.ToString() + number.ToString());
                        updatedResults.Add(concat);
                    }
                    updatedResults.Add(multiplied);
                    updatedResults.Add(added);
                }
                possibleResults = updatedResults;
            }

            if (possibleResults.Contains(target))
                sum += target;
        }
        
        return sum;
    }

    public static long FindSumValidCalibrationsPt2(string filename)
    {
        return FindSumValidCalibrations(filename, true);
    }
}

public class Test
{
    [Test]
    public void SimpleShouldFindProperSum()
    {
        Assert.That(Solution.FindSumValidCalibrations("Day7/Simple.txt"), Is.EqualTo(3749));
    }

    [Test]
    public void ComplexFindNumberOfGuardMoves()
    {
        Assert.That(Solution.FindSumValidCalibrations("Day7/Complex.txt"), Is.EqualTo(6392012777720));
    }
    
    [Test]
    public void SimplePt2ShouldFindProperSum()
    {
        Assert.That(Solution.FindSumValidCalibrationsPt2("Day7/Simple.txt"), Is.EqualTo(11387));
    }

    [Test]
    public void ComplexPt2FindNumberOfGuardMoves()
    {
        Assert.That(Solution.FindSumValidCalibrationsPt2("Day7/Complex.txt"), Is.EqualTo(61561126043536));
    }
}
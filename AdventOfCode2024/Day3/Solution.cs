using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day3;

public class Solution
{
    public static int SumOfMultiplications(string filename) {
        var program = File.ReadAllText(filename);
        var matches = Regex.Matches(program, @"mul\(\d{1,3},\d{1,3}\)");
        var sum = 0;
        foreach (Match match in matches)
        {
            var pair = match.Value.Substring(4, match.Value.Length - 5);
            var split = pair.Split(',');
            sum += int.Parse(split[0]) * int.Parse(split[1]);
        }
        return sum;
    }
    
    public static int SumOfMultiplicationsDoAndDont(string filename) {
        var doWork = true;
        var program = File.ReadAllText(filename);
        var matches = Regex.Matches(program, @"mul\(\d{1,3},\d{1,3}\)|don\'t\(\)|do\(\)");
        var sum = 0;
        foreach (Match match in matches)
        {
            if (match.Value.Equals("don\'t()")) {
                doWork = false;
                continue;
            }
            if (match.Value.Equals("do()")) {
                doWork = true;
                continue;
            }

            if (doWork)
            {
                var pair = match.Value.Substring(4, match.Value.Length - 5);
                var split = pair.Split(',');
                sum += int.Parse(split[0]) * int.Parse(split[1]);
            }
        }
        return sum;
    }
}

public class Test {
    [Test]
    public void SimpleShouldCalcSumOfMultiplications() {
        var result = Solution.SumOfMultiplications("Day3/Simple.txt");
        Assert.That(result, Is.EqualTo(161));
    }

    [Test]
    public void ComplexShouldCalcSumOfMultiplications() {
        var result = Solution.SumOfMultiplications("Day3/Complex.txt");
        Assert.That(result, Is.EqualTo(192767529));
    }
    
    [Test]
    public void SimpleShouldCalcSumOfMultiplicationsDoAndDont() {
        var result = Solution.SumOfMultiplicationsDoAndDont("Day3/Simple2.txt");
        Assert.That(result, Is.EqualTo(48));
    }
    
    [Test]
    public void ComplexShouldCalcSumOfMultiplicationsDoAndDont() {
        var result = Solution.SumOfMultiplicationsDoAndDont("Day3/Complex2.txt");
        Assert.That(result, Is.EqualTo(104083373));
    }
}
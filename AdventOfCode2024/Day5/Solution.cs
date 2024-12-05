using NUnit.Framework.Constraints;

namespace AdventOfCode2024.Day5;

public class Solution
{
    public static int SumBadPages(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var indexOfSeparator = Array.IndexOf(lines, "");
        var pageOrderRuleStrings = lines.Take(indexOfSeparator);
        var pageUpdates = lines.TakeLast(lines.Count() - indexOfSeparator - 1);
        
        // Create dictionary of pages and list of pages they must come before
        var pageOrderRules = new Dictionary<int, List<int>>();
        foreach (var pageOrderRuleString in pageOrderRuleStrings)
        {
            var split = pageOrderRuleString.Split('|');
            var pageOne = int.Parse(split[0]);
            var pageTwo = int.Parse(split[1]);
            if (pageOrderRules.ContainsKey(pageOne))
            {
                pageOrderRules[pageOne].Add(pageTwo);
            }
            else
            {
                pageOrderRules.Add(pageOne, new List<int> { pageTwo });
            }
        }

        // Process page updates to determine validitiy
        var sum = 0;
        foreach (var pageUpdate in pageUpdates)
        {
            var pages = pageUpdate.Split(',').Select(int.Parse).ToList();
            var isValidPage = true;
            for (var i = 0; i < pages.Count; i++)
            {
                var current = pages[i];
                if (pageOrderRules.ContainsKey(current))
                {
                    var mustComeBefore = pageOrderRules[current];
                    var previousPages = pages.Take(i);
                    if (previousPages.Any(page => mustComeBefore.Contains(page)))
                    {
                        isValidPage = false;
                        break;
                    }
                }
            }
            if(isValidPage)
                sum+= pages[pages.Count / 2];
        }
        
        return sum;
    }

    public static int SumBadPagesAfterOrderingCorrectly(string filename)
    {
        var lines = File.ReadAllLines(filename);
        var indexOfSeparator = Array.IndexOf(lines, "");
        var pageOrderRuleStrings = lines.Take(indexOfSeparator);
        var pageUpdates = lines.TakeLast(lines.Count() - indexOfSeparator - 1);
        
        // Create dictionary of pages and list of pages they must come before
        var pageOrderRules = new Dictionary<int, List<int>>();
        foreach (var pageOrderRuleString in pageOrderRuleStrings)
        {
            var split = pageOrderRuleString.Split('|');
            var pageOne = int.Parse(split[0]);
            var pageTwo = int.Parse(split[1]);
            if (pageOrderRules.ContainsKey(pageOne))
            {
                pageOrderRules[pageOne].Add(pageTwo);
            }
            else
            {
                pageOrderRules.Add(pageOne, new List<int> { pageTwo });
            }
        }
        
        // Build list of bad updates
        var badUpdates = new List<List<int>>();
        foreach (var pageUpdate in pageUpdates)
        {
            var pages = pageUpdate.Split(',').Select(int.Parse).ToList();
            for (var i = 0; i < pages.Count; i++)
            {
                var current = pages[i];
                if (pageOrderRules.ContainsKey(current))
                {
                    var mustComeBefore = pageOrderRules[current];
                    var previousPages = pages.Take(i);
                    if (previousPages.Any(page => mustComeBefore.Contains(page)))
                    {
                        badUpdates.Add(pages);
                        break;
                    }
                }
            }
        }

        // Reorder bad updates and sum their median
        var sum = 0;
        foreach (var badUpdate in badUpdates)
        {
            var goodUpdate = new List<int>();
            for (var i = 0; i < badUpdate.Count; i++)
            {
                var page = badUpdate[i];
                if (!goodUpdate.Any())
                {
                    goodUpdate.Add(page);
                    continue;
                }

                if (pageOrderRules.ContainsKey(page))
                {
                    var mustComeBefore = pageOrderRules[page];
                    var indexPageMustPreceed = goodUpdate.FindIndex(x => mustComeBefore.Contains(x));
                    if (indexPageMustPreceed == -1)
                    {
                        goodUpdate.Add(page);
                    }
                    else
                    {
                        goodUpdate.Insert(indexPageMustPreceed, page);
                    }
                }
                else
                {
                    goodUpdate.Add(page);
                }
            }
            sum+= goodUpdate[goodUpdate.Count / 2];
        }
        
        return sum;
    }
}

public class Test
{
    [Test]
    public void SimpleSumOfBadPagesShouldWork()
    {
        Assert.That(Solution.SumBadPages("Day5/Simple.txt"), Is.EqualTo(143));
    }
    
    [Test]
    public void ComplexSumOfBadPagesShouldWork()
    {
        Assert.That(Solution.SumBadPages("Day5/Complex.txt"), Is.EqualTo(6612));
    }
    
    [Test]
    public void SimpleSumOfBadPagesAfterOrderingCorrectlyShouldWork()
    {
        Assert.That(Solution.SumBadPagesAfterOrderingCorrectly("Day5/Simple.txt"), Is.EqualTo(123));
    }
    
    [Test]
    public void ComplexSumOfBadPagesAfterOrderingCorrectlyShouldWork()
    {
        Assert.That(Solution.SumBadPagesAfterOrderingCorrectly("Day5/Complex.txt"), Is.EqualTo(4944));
    }
}
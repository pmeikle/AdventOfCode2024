using System.Text;

namespace AdventOfCode2024.Day9;

public class Solution
{
    public static long FindChecksum(string filename)
    {
        var line = File.ReadAllText(filename);
        var blocks = new List<int>();
        //Expand
        for (var i = 0; i < line.Length; i++)
        {
            var spot = line[i];
            var toAppendInt = -1;
            if (i % 2 == 0)
            {
                toAppendInt = i/2;
            }
            var numTimes = int.Parse(spot.ToString());
            blocks.AddRange(Enumerable.Repeat(toAppendInt, numTimes));
        }
        
        // Move back to first
        for (var i = 0; i < blocks.Count; i++)
        {
            var curr = blocks[i];
            if (curr != -1)
                continue;
            var lastJChecked = blocks.Count - 1;
            for (var j = lastJChecked; j >= 0 && j > i; j--)
            {
                var toCheck = blocks[j];
                if (toCheck == -1)
                    continue;
                blocks[j] = -1;
                blocks[i] = toCheck;
                break;
            }
        }
        
        // Calc checksum
        long checkSum = 0;
        for (var i = 0; i < blocks.Count; i++)
        {
            var id = blocks[i];
            if (id == -1)
                break;
            checkSum += id * i;
        }
        
        return checkSum;
    }
    
    public static long FindChecksumV2(string filename)
    {
        var line = File.ReadAllText(filename);
        var blocks = new List<int>();
        //Expand
        for (var i = 0; i < line.Length; i++)
        {
            var spot = line[i];
            var toAppendInt = -1;
            if (i % 2 == 0)
            {
                toAppendInt = i/2;
            }
            var numTimes = int.Parse(spot.ToString());
            blocks.AddRange(Enumerable.Repeat(toAppendInt, numTimes));
        }
        
        // Move back to first
        for (var j = blocks.Count - 1; j >= 0; j--)
        {
            var toCheck = blocks[j];
            if (toCheck == -1)
                continue;
            var firstIndex = blocks.FindIndex(x => x == toCheck);
            var needToFit = j - firstIndex + 1;
            
            for (var i = 0; i < blocks.Count && i < j; i++)
            {
                var curr = blocks[i];
                if (curr != -1)
                    continue;
                var nextIndex = blocks.FindIndex(i, x => x != -1);
                if (nextIndex == -1)
                    break;
                var canFit = nextIndex - i;
                if (needToFit <= canFit)
                {
                    for (var k = i; k < i + needToFit; k++)
                    {
                        blocks[k] = toCheck;
                    }

                    for (var k = firstIndex; k <= j; k++)
                    {
                        blocks[k] = -1;
                    }

                    break;
                }
                i = nextIndex - 1;
            }
            j = firstIndex;
        }
        
        
        // Calc checksum
        long checkSum = 0;
        for (var i = 0; i < blocks.Count; i++)
        {
            var id = blocks[i];
            if (id == -1)
                continue;
            checkSum += id * i;
        }
        
        return checkSum;
    }
}

public class Test
{
    [Test]
    public void SimpleShouldFindChecksum()
    {
        Assert.That(Solution.FindChecksum("Day9/Simple.txt"), Is.EqualTo(1928));
    }
    
    [Test]
    public void ComplexShouldFindChecksum()
    {
        Assert.That(Solution.FindChecksum("Day9/Complex.txt"), Is.EqualTo(6331212425418));
    }
    
    [Test]
    public void SimpleShouldFindChecksumV2()
    {
        Assert.That(Solution.FindChecksumV2("Day9/Simple.txt"), Is.EqualTo(2858));
    }
    
    [Test]
    public void ComplexShouldFindChecksumV2()
    {
        Assert.That(Solution.FindChecksumV2("Day9/Complex.txt"), Is.EqualTo(6363268339304));
    }
}
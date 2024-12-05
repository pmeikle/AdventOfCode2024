using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024.Day2 {
	public class Test {
		[Test]
		public void SimpleShouldCalcSimilarity() {
			var result = Solution.NumberOfSafeReports("Day2/Simple.txt");
			Assert.That(result, Is.EqualTo(2));
		}

		[Test]
		public void ComplexShouldCalcSimilarity() {
			var result = Solution.NumberOfSafeReports("Day2/Complex.txt");
			Assert.That(result, Is.EqualTo(670));
		}

		[Test]
		public void SimpleShouldCalcSimilarityTolerateBadLevel() {
			var result = Solution.NumberOfSafeReportsTolerateOneBadLevel("Day2/Simple.txt");
			Assert.That(result, Is.EqualTo(4));
		}

		[Test]
		public void ComplexShouldCalcSimilarityTolerateBadLevel() {
			var result = Solution.NumberOfSafeReportsTolerateOneBadLevel("Day2/Complex.txt");
			Assert.That(result, Is.EqualTo(700));
		}
	}
	public class Solution {
		public static int NumberOfSafeReports(string filename) {
			var lines = File.ReadLines(filename);
			return lines.Where(IsReportSafe).Count();
		}

		public static int NumberOfSafeReportsTolerateOneBadLevel(string filename) {
			var lines = File.ReadLines(filename);
			return lines.Where(IsReportSafeTolerateOneBadLevel).Count();
		}

		private static bool IsReportSafeTolerateOneBadLevel(string report) {
			if (IsReportSafe(report))
				return true;
			else {
				var numbers = report.Split(' ').Select(x => int.Parse(x)).ToList();
				for (var i = 0; i < numbers.Count; i++) {
					var copy = numbers.GetRange(0, numbers.Count);
					copy.RemoveAt(i);
					if (IsReportSafe(String.Join(' ', copy)))
						return true;
				}
				return false;
			}
		}

		private static bool IsReportSafe(string report) {
			var haveRemovedLevel = false;
			var numbers = report.Split(' ').Select(x => int.Parse(x)).ToList();
			var isListIncreasing = false;
			for (var i = 0; i < numbers.Count() - 1; i++) {
				var difference = numbers[i] - numbers[i + 1];
				if (i == 0) {
					isListIncreasing = difference < 0;
				} else if (isListIncreasing && difference > 0 || !isListIncreasing && difference < 0) {
					return false;
				}
				var absValue = Math.Abs(difference);
				if (absValue == 0 || absValue > 3) {
					return false;
				}
			}
			return true;
		}
	}
}

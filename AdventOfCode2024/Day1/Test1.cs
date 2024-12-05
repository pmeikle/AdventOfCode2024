namespace AdventOfCode2024.Day1 {
	public class Test1 {

		[Test]
		public void SimpleShouldCalcDistance() {
			var result = Solution.CalcDistance("Day1/Simple.txt");
			Assert.That(result, Is.EqualTo(11));
		}

		[Test]
		public void ComplexShouldCalcDistance() {
			var result = Solution.CalcDistance("Day1/Complex.txt");
			Assert.That(result, Is.EqualTo(2769675));
		}


		[Test]
		public void SimpleShouldCalcSimilarity() {
			var result = Solution.CalcSimularity("Day1/Simple.txt");
			Assert.That(result, Is.EqualTo(31));
		}

		[Test]
		public void ComplexShouldCalcSimilarity() {
			var result = Solution.CalcSimularity("Day1/Complex.txt");
			Assert.That(result, Is.EqualTo(24643097));
		}
	}

	public class Solution {
		private const string Separator = "   ";
		public static int CalcDistance(string filename) {
			// Parse Input
			var firstList = new List<int>();
			var secondList = new List<int>();
			var lines = File.ReadLines(filename);
			foreach(var line in lines) {
				var numbers = line.Split(Separator);
				firstList.Add(Int32.Parse(numbers[0]));
				secondList.Add(Int32.Parse(numbers[1]));
			}
			firstList.Sort();
			secondList.Sort();

			// Process
			var distance = 0;
			for(var i = 0; i < lines.Count(); i++) {
				var first = firstList[i];
				var second = secondList[i];
				distance += Math.Abs(first - second);
			}
			return distance;
		}

		public static int CalcSimularity(string filename) {
			// Parse Input
			var firstList = new List<int>();
			var numberMap = new Dictionary<int, int>();
			var lines = File.ReadLines(filename);
			foreach (var line in lines) {
				var numbers = line.Split(Separator);
				var first = Int32.Parse(numbers[0]);
				var second = Int32.Parse(numbers[1]);
				firstList.Add(first);
				if (numberMap.ContainsKey(second)) {
					numberMap[second]++;
				} else {
					numberMap[second] = 1;
				}
			}

			// Process
			var similarity = 0;
			for (var i = 0; i < lines.Count(); i++) {
				var first = firstList[i];
				var second = numberMap.ContainsKey(first) ? numberMap[first] : 0;
				similarity += first * second;
			}

			return similarity;
		}
	}
}

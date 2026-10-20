using System.Text;

namespace LongestIncreasingSubsequence.Tests;

public class LongestIncreasingSubsequenceFinderTests
{
    public static TheoryData<int> ProvidedCaseNumbers => [.. Enumerable.Range(1, 11)];

    [Theory]
    [MemberData(nameof(ProvidedCaseNumbers))]
    public void FindLongestIncreasingSubsequence_WithProvidedTestCase_ReturnsExpectedSequence(int caseNumber)
    {
        var input = ReadTestData($"case{caseNumber:D2}.input.txt");
        var expected = ReadTestData($"case{caseNumber:D2}.expected.txt");

        var result = LongestIncreasingSubsequenceFinder.FindLongestIncreasingSubsequence(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("6 1 5 9 2", "1 5 9")]
    [InlineData("7", "7")]
    [InlineData("", "")]
    [InlineData("   ", "")]
    [InlineData("9 7 5 3 1", "9")]
    [InlineData("1 2 3 4 5", "1 2 3 4 5")]
    [InlineData("-5 -2 -10 -1", "-5 -2")]
    [InlineData("0 -1 1", "-1 1")]
    [InlineData("6 2 4 3 1 5 9", "1 5 9")]
    public void FindLongestIncreasingSubsequence_WithBasicInputs_ReturnsExpectedSequence(string input, string expected)
    {
        Assert.Equal(expected, LongestIncreasingSubsequenceFinder.FindLongestIncreasingSubsequence(input));
    }

    [Theory]
    [InlineData("1 2 2 3", "1 2")]
    [InlineData("2 2 2", "2")]
    [InlineData("1 2 2 3 4", "2 3 4")]
    public void FindLongestIncreasingSubsequence_WithDuplicateValues_RequiresStrictlyIncreasingValues(string input, string expected)
    {
        Assert.Equal(expected, LongestIncreasingSubsequenceFinder.FindLongestIncreasingSubsequence(input));
    }

    [Theory]
    [InlineData("3 4 1 2", "3 4")]
    [InlineData("1 2 5 3 4", "1 2 5")]
    [InlineData("1 3 2 4", "1 3")]
    [InlineData("2 1 3 4", "1 3 4")]
    [InlineData("5 6 1 2 3 7 8 9", "1 2 3 7 8 9")]
    public void FindLongestIncreasingSubsequence_WithMultipleEqualLengthSequences_ReturnsEarliestSequence(string input, string expected)
    {
        Assert.Equal(expected, LongestIncreasingSubsequenceFinder.FindLongestIncreasingSubsequence(input));
    }

    [Theory]
    [InlineData("  1 5   9 2  ", "1 5 9")]
    [InlineData("\t1 5\n9 2\r\n", "1 5 9")]
    public void FindLongestIncreasingSubsequence_WithExtraWhitespace_IgnoresIt(string input, string expected)
    {
        Assert.Equal(expected, LongestIncreasingSubsequenceFinder.FindLongestIncreasingSubsequence(input));
    }

    [Theory]
    [InlineData("1 two 3")]
    [InlineData("1.5 2")]
    [InlineData("1,2")]
    [InlineData("1 - 2")]
    public void FindLongestIncreasingSubsequence_WithNonNumericToken_ThrowsArgumentException(string input)
    {
        Assert.Throws<ArgumentException>(() => LongestIncreasingSubsequenceFinder.FindLongestIncreasingSubsequence(input));
    }

    [Theory]
    [InlineData("1 2147483648")]
    [InlineData("-2147483649 1")]
    public void FindLongestIncreasingSubsequence_WithIntegerOverflow_ThrowsArgumentException(string input)
    {
        Assert.Throws<ArgumentException>(() => LongestIncreasingSubsequenceFinder.FindLongestIncreasingSubsequence(input));
    }

    [Fact]
    public void FindLongestIncreasingSubsequence_WithIntegerLimits_ReturnsFullRange()
    {
        var result = LongestIncreasingSubsequenceFinder.FindLongestIncreasingSubsequence("2147483647 -2147483648 2147483647");

        Assert.Equal("-2147483648 2147483647", result);
    }

    [Fact]
    public void FindLongestIncreasingSubsequence_WithNullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => LongestIncreasingSubsequenceFinder.FindLongestIncreasingSubsequence(null!));
    }

    [Fact]
    public void FindLongestIncreasingSubsequence_WithLargeInput_ReturnsKnownSequence()
    {
        // A long descending run followed by an ascending run: the answer is the whole ascending run.
        var descending = Enumerable.Range(0, 5000).Select(i => 10_000 - i);
        var ascending = Enumerable.Range(0, 5000);
        var input = string.Join(' ', descending.Concat(ascending));
        var expected = string.Join(' ', ascending);

        Assert.Equal(expected, LongestIncreasingSubsequenceFinder.FindLongestIncreasingSubsequence(input));
    }

    [Fact]
    public void FindLongestIncreasingSubsequence_WithRandomInputs_MatchesBruteForce()
    {
        var random = new Random(12345);

        for (var run = 0; run < 1000; run++)
        {
            var numbers = Enumerable.Range(0, random.Next(0, 15)).Select(_ => random.Next(-5, 6)).ToArray();
            var input = string.Join(' ', numbers);
            var expected = BruteForce(numbers);

            var result = LongestIncreasingSubsequenceFinder.FindLongestIncreasingSubsequence(input);

            Assert.True(expected == result, $"Input '{input}': expected '{expected}' but got '{result}'.");
        }
    }

    // Checks every start/end pair. Longest strictly increasing run wins; the smallest start wins ties.
    private static string BruteForce(int[] numbers)
    {
        var bestStart = 0;
        var bestLength = 0;

        for (var start = 0; start < numbers.Length; start++)
        {
            for (var end = start; end < numbers.Length; end++)
            {
                var isIncreasing = Enumerable.Range(start + 1, end - start).All(i => numbers[i] > numbers[i - 1]);
                if (isIncreasing && end - start + 1 > bestLength)
                {
                    bestStart = start;
                    bestLength = end - start + 1;
                }
            }
        }

        return string.Join(' ', numbers.Skip(bestStart).Take(bestLength));
    }

    private static string ReadTestData(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "TestData", fileName);
        return File.ReadAllText(path, Encoding.UTF8).Trim();
    }
}

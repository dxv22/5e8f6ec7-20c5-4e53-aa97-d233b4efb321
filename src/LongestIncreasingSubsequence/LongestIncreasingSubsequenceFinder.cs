using System.Globalization;

namespace LongestIncreasingSubsequence;

public static class LongestIncreasingSubsequenceFinder
{
    /// <summary>
    /// Finds the longest run of strictly increasing, consecutive integers in a whitespace-separated list.
    /// When several runs share the longest length, the earliest one is returned.
    /// </summary>
    /// <param name="input">Integers separated by whitespace. Empty or whitespace-only input is allowed.</param>
    /// <returns>The run as space-separated integers, or an empty string for empty input.</returns>
    /// <exception cref="ArgumentException">A token is not a valid 32-bit integer.</exception>
    public static string FindLongestIncreasingSubsequence(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var numbers = ParseIntegers(input);
        var (start, length) = FindEarliestLongestIncreasingRun(numbers);

        return string.Join(' ', numbers.Skip(start).Take(length));
    }

    private static int[] ParseIntegers(string input)
    {
        var tokens = input.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        var numbers = new int[tokens.Length];

        for (var i = 0; i < tokens.Length; i++)
        {
            if (!int.TryParse(tokens[i], NumberStyles.Integer, CultureInfo.InvariantCulture, out numbers[i]))
            {
                throw new ArgumentException($"'{tokens[i]}' is not a valid 32-bit integer.");
            }
        }

        return numbers;
    }

    private static (int Start, int Length) FindEarliestLongestIncreasingRun(int[] numbers)
    {
        var bestStart = 0;
        var bestLength = 0;
        var runStart = 0;

        for (var i = 0; i < numbers.Length; i++)
        {
            if (i > 0 && numbers[i] <= numbers[i - 1])
            {
                runStart = i;
            }

            var runLength = i - runStart + 1;

            // Strictly greater, so an equally long later run never replaces the earliest one.
            if (runLength > bestLength)
            {
                bestStart = runStart;
                bestLength = runLength;
            }
        }

        return (bestStart, bestLength);
    }
}

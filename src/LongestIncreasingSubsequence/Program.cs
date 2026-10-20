using LongestIncreasingSubsequence;

if (args.Length > 1)
{
    Console.Error.WriteLine("Usage: LongestIncreasingSubsequence \"<integers separated by whitespace>\"");
    return 2;
}

try
{
    Console.WriteLine(LongestIncreasingSubsequenceFinder.FindLongestIncreasingSubsequence(args.Length == 0 ? string.Empty : args[0]));
    return 0;
}
catch (ArgumentException ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    return 1;
}

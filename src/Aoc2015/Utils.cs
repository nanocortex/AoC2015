namespace Aoc2015;

public static class Utils
{
    public static int[] GetIntegers(string filePath)
    {
        return File.ReadLines(filePath).Select(int.Parse).ToArray();
    }

    public static double[] GetDoubles(string filePath)
    {
        return File.ReadLines(filePath).Select(double.Parse).ToArray();
    }

    public static string[] GetLines(string filePath)
    {
        return File.ReadLines(filePath).ToArray();
    }
}
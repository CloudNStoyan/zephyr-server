namespace Zephyr.Infrastructure;

public static class CustomUtils
{
    /// <summary>
    /// Tries to convert numbers in string to an array of ints
    /// </summary>
    /// <returns>The numbers int parsed in an array</returns>
    public static int[] ConvertToArray(string numbersInString, string separator = " ")
    {
        string[] stringNumbers = numbersInString.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var numbers = new List<int>();

        foreach (string stringNumber in stringNumbers)
        {
            if (int.TryParse(stringNumber, out int number))
            {
                numbers.Add(number);
            }
        }

        return numbers.ToArray();
    }
}
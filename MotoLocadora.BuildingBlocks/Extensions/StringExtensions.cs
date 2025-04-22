namespace MotoLocadora.BuildingBlocks.Extensions;

public static class DateValidationExtensions
{
    public static bool IsValidDateFormat(this string input, out DateTime result)
    {
        return DateTime.TryParseExact(input, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out result);
    }
}


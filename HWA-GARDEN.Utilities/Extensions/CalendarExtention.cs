namespace HWA.GARDEN.Utilities.Extensions
{
    public static class CalendarExtention
    {
        private const int DayOfFeb28 = 59;

        public static DateOnly ToDate(this int dayOfYear, int year)
        {
            var result = new DateOnly(year, 1, 1).AddDays(dayOfYear - 1);
            if(DateTime.IsLeapYear(year) && dayOfYear > DayOfFeb28)
            {
                result = result.AddDays(1);
            }
            return result;
        }

        public static int ToDayOfYear(this DateOnly date)
        {
            return date.ToDateTime(new TimeOnly()).ToDayOfYear();
        }

        public static int ToDayOfYear(this DateTime date)
        {
            var result = (date - new DateTime(date.Year, 1, 1)).Days + 1;
            if(DateTime.IsLeapYear(date.Year) && result > DayOfFeb28)
            {
                result--;
            }
            return result;
        }

        public static DateOnly ToDateOnly(this string value)
        {
            DateOnly result;
            if (!DateOnly.TryParse(value, out result))
            {
                throw new InvalidOperationException($"The '{value}' cannot be converted to DateOnly type.");
            }
            return result;
        }
    }
}

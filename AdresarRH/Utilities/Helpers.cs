namespace CroHoliCityAPI.Utilities
{
    public static class Helpers
    {
        //calculates easter date for a given year
        public static DateTime CalculateEasterDate(int year)
        {
            int a = year % 19;
            int b = year / 100;
            int c = year % 100;
            int d = b / 4;
            int e = b % 4;
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            int h = (19 * a + b - d - g + 15) % 30;
            int i = c / 4;
            int k = c % 4;
            int l = (32 + 2 * e + 2 * i - h - k) % 7;
            int m = (a + 11 * h + 22 * l) / 451;
            int month = (h + l - 7 * m + 114) / 31;
            int day = ((h + l - 7 * m + 114) % 31) + 1;

            return new DateTime(year, month, day);
        }
        public static DateTime CalculateTjelovoDate(int year)
        {
            DateTime easterDate = CalculateEasterDate(year);
            DateTime tjelovoDate = easterDate.AddDays(60); // Add 60 days to Easter date for Tjelovo

            return tjelovoDate;
        }

        //extracts all weekends from a given year. Saturday and Sunday
        static DateTime[] GetWeekends(int year)
        {
            var weekends = new List<DateTime>();

            for (int month = 1; month <= 12; month++) {
                for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++) {
                    DateTime date = new DateTime(year, month, day);
                    if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) {
                        weekends.Add(date);
                    }
                }
            }

            return weekends.ToArray();
        }

        //extract non working days for Croatia
        static DateTime[] GetNonWorkingDaysCroatia(int year)
        {
            var weekends = new List<DateTime>();

           //holidays

            return weekends.ToArray();
        }

    }
}

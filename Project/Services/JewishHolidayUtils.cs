using System;
using System.Collections.Generic;
using System.Globalization;

namespace Project.Services
{
    public static class JewishHolidayUtils
    {
        private static readonly HebrewCalendar HebrewCalendar = new HebrewCalendar();

        public static bool IsJewishHoliday(DateTime date)
        {
            int hebrewYear = HebrewCalendar.GetYear(date);
            int hebrewMonth = HebrewCalendar.GetMonth(date);
            int hebrewDay = HebrewCalendar.GetDayOfMonth(date);

            string hebrewDateStr = GetHebrewDateString(date);
            string holidayName = GetHolidayName(hebrewMonth, hebrewDay, hebrewYear);

            Console.WriteLine($"תאריך לועזי: {date:yyyy-MM-dd}");
            Console.WriteLine($"תאריך עברי: {hebrewDateStr}");

            if (!string.IsNullOrEmpty(holidayName))
            {
                Console.WriteLine($"חג/חופש: {holidayName}");
                return true;
            }

            Console.WriteLine("לא חג");
            return false;
        }

        public static List<(DateTime gregorian, string hebrew, string name)> GetHolidaysOfHebrewYear(int hebrewYear)
        {
            var holidays = new List<(DateTime, string, string)>();

            for (int month = 1; month <= 12; month++)
            {
                int daysInMonth = HebrewCalendar.GetDaysInMonth(hebrewYear, month);

                for (int day = 1; day <= daysInMonth; day++)
                {
                    DateTime gregorianDate;
                    try
                    {
                        gregorianDate = HebrewCalendar.ToDateTime(hebrewYear, month, day, 0, 0, 0, 0);
                    }
                    catch
                    {
                        continue;
                    }

                    string holidayName = GetHolidayName(month, day, hebrewYear);
                    if (!string.IsNullOrEmpty(holidayName))
                    {
                        holidays.Add((gregorianDate, GetHebrewDateString(gregorianDate), holidayName));
                    }
                }
            }

            return holidays;
        }

        private static string GetHolidayName(int month, int day, int year)
        {
            // תשרי
            if (month == 1 && day == 3) return "צום גדליה";
            if (month == 1 && (day == 9 || day == 10)) return "צום יום כיפור";
            if (month == 1 && day >= 11 && day <= 14) return "בין יום כיפור לסוכות";
            if (month == 1 && day >= 15 && day <= 23) return "סוכות + אסרו חג";
            if (month == 13 && day == 29) return "ראש השנה (כ״ט אלול – ג׳ תשרי)";
            if (month == 1 && (day == 1 || day == 2)) return "ראש השנה";

            // כסלו–טבת
            if (month == 3 && day >= 24) return "חנוכה";
            if (month == 4 && day <= 2) return "חנוכה";
            if (month == 4 && day == 10) return "צום עשרה בטבת";

            // פורים
            // שנה רגילה: י"ג–ט"ו באדר (חודש 12)
            // שנה מעוברת: י"ג–ט"ו באדר ב' (חודש 13 בלבד)
            int adarMonth = HebrewCalendar.IsLeapYear(year) ? 13 : 12;
            if (month == adarMonth && day >= 13 && day <= 15)
                return "פורים";

            // ניסן
            if (month == 8 && day >= 8 && day <= 22) return "פסח";

            // סיון
            if (month == 10 && day >= 5 && day <= 7) return "שבועות";

            // תמוז
            if (month == 11 && day == 17) return "צום י״ז בתמוז";

            // אב
            if (month == 12 && day >= 8 && day <= 29) return "בין הזמנים";

            return null;
        }

        private static string GetHebrewDateString(DateTime date)
        {
            int hebrewYear = HebrewCalendar.GetYear(date);
            int hebrewMonth = HebrewCalendar.GetMonth(date);
            int hebrewDay = HebrewCalendar.GetDayOfMonth(date);

            string[] hebrewMonths = {
                "",
                "תשרי",
                "חשון",
                "כסלו",
                "טבת",
                "שבט",
                "אדר א׳",
                "אדר ב׳",
                "ניסן",
                "אייר",
                "סיון",
                "תמוז",
                "אב",
                "אלול"
            };

            return $"{hebrewDay} {hebrewMonths[hebrewMonth]} {hebrewYear}";
        }
    }
}

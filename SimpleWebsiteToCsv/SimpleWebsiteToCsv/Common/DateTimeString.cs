using System;

namespace SimpleWebsiteToCsv.Common
{
    public class DateTimeString
    {
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }

        public DateTimeString() { }
        public DateTimeString(string year, string month, string day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public DateTimeString(string[] array, int yindx, int mindx, int dindx)
        {
            string year = array[yindx].Trim();
            string month = array[mindx].Trim();
            string day = array[dindx].Trim();

            if (!string.IsNullOrEmpty(year))
            {
                int iyear = Convert.ToInt32(year);
                if (iyear > 0)
                    Year = iyear.ToString();
            }

            if (!string.IsNullOrEmpty(month))
            {
                int imonth = Convert.ToInt32(month);
                if (imonth > 0)
                    Month = imonth.ToString();
            }

            if (!string.IsNullOrEmpty(day))
            {
                int iday = Convert.ToInt32(day);
                if (iday > 0)
                    Day = iday.ToString();
            }
        }

        public DateTimeString(string year)
        {
            Year = year;
        }

        public DateTimeString(DateTime dated)
        {
            Year = dated.Year.ToString();
            Month = dated.Month.ToString();
            Day = dated.Day.ToString();
        }
    }
}

using System;

namespace TemporalExpressions
{
    public class RangeEachInterval : TemporalExpression
    {
        private const int DaysInWeek = 7;
        private const int MonthsInYear = 12;

        public DateTime Date { get; set; }

        public int Count { get; set; }

        public UnitOfTime Unit { get; set; }

        public RangeEachInterval(int year, int month, int day, int count, UnitOfTime unit)
        {
            this.Date = new DateTime(year, month, day);
            this.Count = count;
            this.Unit = unit;
        }

        public override bool Includes(DateTime date)
        {
            if (date < Date)
            {
                return false;
            }

            int? unitsApart = null;

            var dayDifference = date - Date;

            switch (Unit)
            {
                case UnitOfTime.Day:
                    unitsApart = (int)Math.Round(dayDifference.TotalDays);
                    break;
                case UnitOfTime.Week:
                    unitsApart = (int)Math.Floor(dayDifference.TotalDays / DaysInWeek);
                    break;
                case UnitOfTime.Month:
                    var startMonth = MonthOrdinal(Date);
                    var endMonth = MonthOrdinal(date);

                    unitsApart = endMonth - startMonth;
                    break;
                case UnitOfTime.Year:
                    unitsApart = date.Year - Date.Year;
                    break;
            }

            if (!unitsApart.HasValue)
            {
                return false;
            }

            return unitsApart.Value % Count == 0;
        }

        private static int MonthOrdinal(DateTime date)
        {
            return (date.Year * MonthsInYear) + date.Month;
        }
    }
}

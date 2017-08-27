using System;

namespace TemporalExpressions
{
    public class RegularInterval : TemporalExpression
    {
        private const int DaysInWeek = 7;
        private const int MonthsInYear = 12;

        public DateTime StartDate { get; set; }

        public int Count { get; set; }

        public UnitOfTime Unit { get; set; }

        public RegularInterval(int year, int month, int day, int count, UnitOfTime unit)
        {
            this.StartDate = new DateTime(year, month, day);
            this.Count = count;
            this.Unit = unit;
        }

        public override bool Includes(DateTime date)
        {
            if (date < StartDate)
            {
                return false;
            }

            int? unitsApart = null;

            var dayDifference = date - StartDate;

            switch (Unit)
            {
                case UnitOfTime.Day:
                    unitsApart = (int)Math.Round(dayDifference.TotalDays);
                    break;
                case UnitOfTime.Week:
                    unitsApart = (int)Math.Floor(dayDifference.TotalDays / DaysInWeek);
                    break;
                case UnitOfTime.Month:
                    var startMonth = MonthOrdinal(StartDate);
                    var endMonth = MonthOrdinal(date);

                    unitsApart = endMonth - startMonth;
                    break;
                case UnitOfTime.Year:
                    unitsApart = date.Year - StartDate.Year;
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

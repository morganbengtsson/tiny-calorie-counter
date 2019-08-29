using System;
using System.Linq;
using System.Collections.Generic;
using Diet.Models;
using System.Globalization;

namespace Diet.ViewModels
{
    public class StatisticsView
    {
        public IEnumerable<double?> Weights { get; set; }
        public IEnumerable<double> Calories { get; set; }
    }

    public class StatisticsViewYear : StatisticsView
    {
        public StatisticsViewYear(int year, IEnumerable<double?> weightList, IEnumerable<double> calorieList)
        {
            Year = year;
            Weights = weightList;
            Calories = calorieList;
        }
        public int Year { get; set; }
    }

    public class StatisticsViewWeek : StatisticsViewYear
    {
        public StatisticsViewWeek(int year, int week, IEnumerable<double?> weightList, IEnumerable<double> calorieList) : base(year, weightList, calorieList)
        {
            Week = week;
        }
        public int Week { get; set; }
    }

    public class StatisticsViewMonth : StatisticsViewYear
    {
        public StatisticsViewMonth(int year, int month, IEnumerable<double?> weightList, IEnumerable<double> calorieList) : base(year, weightList, calorieList)
        {
            Month = month;
        }
        public int Month { get; set; }
    }


}
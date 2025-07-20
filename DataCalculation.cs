using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using Test_Task_Datetime.Scheme;

namespace Test_Task_Datetime
{
    public class DataCalculation
    {
        public static (double DeltaTime, DateTimeOffset MinDate, double AvgExTime, decimal AvgValue, decimal MdnValue, decimal MinValue, decimal MaxValue) MakeAllCalculations(DbSet<Values> context)
        {
            return (DeltaTime(context), MinDate(context), AverageExecutionTime(context), AverageValue(context), MedianValue(context), MinValue(context), MaxValue(context));
        }
        public static double DeltaTime(DbSet<Values> context)
        {
            return (context.Max(p => p.Date) - context.Min(p => p.Date)).TotalSeconds;
        }
        public static DateTimeOffset MinDate(DbSet<Values> context)
        {
            return context.Min(p => p.Date);
        }
        public static double AverageExecutionTime(DbSet<Values> context)
        {
            return context.Average(p => p.ExecutionTime);
        }
        public static decimal AverageValue(DbSet<Values> context)
        {
            return context.Average(p=>p.Value);
        }
        public static decimal MedianValue(DbSet<Values> context)
        {
            int count = context.Count();
            return count % 2 == 0 ? context.Select(x => x.Value).OrderBy(x => x).Skip((count / 2) - 1).Take(2).Average() : context.Select(x => x.Value).OrderBy(x => x).ElementAt(count / 2);
        }
        public static decimal MinValue(DbSet<Values> context)
        {
            return context.Min(p => p.Value);
        }
        public static decimal MaxValue(DbSet<Values> context)
        {
            return context.Max(p => p.Value);
        }
    }
}

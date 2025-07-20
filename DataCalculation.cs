using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using Test_Task_Datetime.Scheme;

namespace Test_Task_Datetime
{
    public class DataCalculation
    {
        public static (double DeltaTime, DateTimeOffset MinDate, double AvgExTime, decimal AvgValue, decimal MdnValue, decimal MinValue, decimal MaxValue) MakeAllCalculations(DbSet<Values> context, FileNames f)
        {
            /*Get Data By File Name*/
            var data = context
                .Select(x => x)
                .Where(x => x.FileNamesFileNameId == f.FileNameId);

            return (DeltaTime(data), MinDate(data), AverageExecutionTime(data), AverageValue(data), MedianValue(data), MinValue(data), MaxValue(data));
        }
        public static double DeltaTime(IQueryable<Values> context)
        {
            return (context.Max(p => p.Date) - context.Min(p => p.Date)).TotalSeconds;
        }
        public static DateTimeOffset MinDate(IQueryable<Values> context)
        {
            return context.Min(p => p.Date);
        }
        public static double AverageExecutionTime(IQueryable<Values> context)
        {
            return context.Average(p => p.ExecutionTime);
        }
        public static decimal AverageValue(IQueryable<Values> context)
        {
            return context.Average(p=>p.Value);
        }
        public static decimal MedianValue(IQueryable<Values> context)
        {
            int count = context.Count();
            return count % 2 == 0 ? context.Select(x => x.Value).OrderBy(x => x).Skip((count / 2) - 1).Take(2).Average() : context.Select(x => x.Value).OrderBy(x => x).ElementAt(count / 2);
        }
        public static decimal MinValue(IQueryable<Values> context)
        {
            return context.Min(p => p.Value);
        }
        public static decimal MaxValue(IQueryable<Values> context)
        {
            return context.Max(p => p.Value);
        }
    }
}

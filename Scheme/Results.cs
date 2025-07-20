using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test_Task_Datetime.Scheme
{
    public class Results
    {
        [Key]
        public int ResultId { get; set; }
        public double DeltaTime { get; set; }
        public DateTimeOffset MinDate { get; set; }
        public double AverageExecutionTime { get; set; }
        public decimal AverageValue { get; set; }
        public decimal MedianValue { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }

        public int FileNamesFileNameId { get; set; }
        public FileNames Filename { get; set; }
    }
}

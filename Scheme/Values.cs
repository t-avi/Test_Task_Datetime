using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test_Task_Datetime.Scheme
{
    public class Values
    {     
        [Key]       
        public int ValueId { get; set; }
        public DateTimeOffset Date { get; set; }
        public int ExecutionTime { get; set; }
        public decimal Value { get; set; }

        public int FileNamesFileNameId { get; set; }
        public FileNames Filename { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Test_Task_Datetime.Scheme
{
    public class Values
    {
        [Key]       
        public Guid id { get; set; }
        public DateTimeOffset date { get; set; }
        public int executiontime { get; set; }
        public decimal value { get; set; }
    }
}

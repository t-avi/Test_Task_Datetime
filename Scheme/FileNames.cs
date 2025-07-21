using System.ComponentModel.DataAnnotations;

namespace Test_Task_Datetime.Scheme
{
    public class FileNames
    {
        [Key]
        public int FileNameId { get; set; }
        public string Name {  get; set; }
        public List<Values> Values { get; set; } = new List<Values>();
        public List<Results> Results { get; set; } = new List<Results>();
    }
}

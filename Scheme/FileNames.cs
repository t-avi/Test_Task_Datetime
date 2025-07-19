using System.ComponentModel.DataAnnotations;

namespace Test_Task_Datetime.Scheme
{
    public class FileNames
    {
        [Key]
        public int FileNameId { get; set; }
        public string Name {  get; set; }
        public List<Values> Users { get; set; } = new List<Values>();
    }
}

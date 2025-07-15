using Microsoft.AspNetCore.Mvc;

namespace Test_Task_Datetime.Controllers
{
    public class DateTimeController : Controller
    {
        private static List<DateOnly> _dateTime = new List<DateOnly>();
        private readonly ILogger<DateTimeController> _logger;
        public DateTimeController(ILogger<DateTimeController> logger)
        {
            _logger = logger;
        }

        [HttpPost("upload-file")]
        public async Task Upload(IFormFile file)
        {
            var directory = @"C:\temp";
            string filePath = Path.Combine(directory, file.FileName);
            await using Stream fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            _logger.LogDebug(id.ToString());
            return _dateTime[id].ToString();
        }

        [HttpPost("{value}")]
        public void Post([FromBody] string value)        
          => _dateTime.Add(DateOnly.FromDateTime(DateTime.Now).AddDays(_dateTime.Count));
        

        [HttpPut("{id}, {value}")]
        public void Put(int id, [FromBody] string value)
        {            
            //...
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //...
        }
    }
}

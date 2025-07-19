using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using Test_Task_Datetime.Scheme;

namespace Test_Task_Datetime.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DateTimeController : Controller
    {
        private readonly ILogger<DateTimeController> _logger;
        public DateTimeController(ILogger<DateTimeController> logger)
        {
            _logger = logger;
        }

        [HttpPost("upload-file")]
        public async Task Upload(IFormFile file)
        {
            try{ DataValidation.TryPutData(file); } catch (Exception e){ Console.WriteLine(e);
                                                                        _logger.LogDebug(e.ToString()); }
        }

        [HttpPut]
        public void Put(string date, int time, decimal value, string f)
        {

        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            _logger.LogDebug(id.ToString());
            return "";
        }
    }
}

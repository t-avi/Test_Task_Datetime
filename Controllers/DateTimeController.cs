using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Xml.Linq;
using Test_Task_Datetime.Scheme;

namespace Test_Task_Datetime.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpPut]
        public void Put(int age, string name)
        {
            //DBQuery.InsertData(value, id);

            using (ApplicationContext db = new ApplicationContext())
            {
                Values d = new Values { date = DateTimeOffset.Parse("2009-05-07 08:17:25Z"), executiontime = 12, value = 0.3M };

                db.Values.Add(d);
                db.SaveChanges();

                /*
                var values = db.Values.ToList();
                foreach (v u in values)
                {
                //...
                }*/
            }
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            _logger.LogDebug(id.ToString());
            return _dateTime[id].ToString();
        }
    }
}

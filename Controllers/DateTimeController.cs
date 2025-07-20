using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using System.Globalization;
using Test_Task_Datetime.Scheme;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace Test_Task_Datetime.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DateTimeController : Controller
    {
        /*Stopwatch stopwatch = new Stopwatch();*/
        private readonly ILogger<DateTimeController> _logger;
        public DateTimeController(ILogger<DateTimeController> logger)
        {
            _logger = logger;
        }

        [HttpPost("UploadFile")]
        public async Task Upload(IFormFile file)
        {
            try { DataValidation.TryPutData(file); } catch (Exception e){ Console.WriteLine(e);
                                                                        _logger.LogDebug(e.ToString()); }            

        }

        [HttpGet]
        [Route("GetByName")]
        public IEnumerable<Scheme.Results> GetByName(string name)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureCreated();

                int id = db.FileNames.First(a => a.Name == name).FileNameId; //try catch

                return db.Results
                    .Select(x=>x)
                    .Where(x=>x.FileNamesFileNameId == id) //Reference Loop Exception
                    .ToArray();    
            }             
        }

        [HttpGet]
        [Route("GetByMinDates")]        
        public IEnumerable<Scheme.Results> GetByMinDates(DateTime start, DateTime end)
        {
            //'2017-07-21T17:32:28Z'
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureCreated();

                return db.Results
                    .Select(x => x)
                    .Where(x => x.MinDate > start && x.MinDate < end)
                    .ToArray();
            }
        }

        [HttpGet]
        [Route("GetByAvgTimes")]
        public IEnumerable<Scheme.Results> GetByAvgTimes(double start, double end)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureCreated();

                return db.Results
                    .Select(x => x)
                    .Where(x => x.AverageExecutionTime > start && x.AverageExecutionTime < end)
                    .ToArray();
            }
        }

        [HttpGet]
        [Route("GetByAvgValues")]
        public IEnumerable<Scheme.Results> GetByAvgValues(decimal start, decimal end)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureCreated();

                return db.Results
                    .Select(x => x)
                    .Where(x => x.AverageValue > (decimal)start && x.AverageValue < (decimal)end)
                    .ToArray();
            }
        }        

    }
}

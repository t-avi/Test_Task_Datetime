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

        /// <summary>
        /// Upload file and add data to DB
        /// </summary>
        /// <param name="file">IFormFile object</param>
        /// <returns>Returns nothing</returns>
        [HttpPost("UploadFile")]
        public async Task Upload(IFormFile file)
        {
            try { DataValidation.TryPutData(file); } catch (Exception e){ Console.WriteLine(e);
                                                                        _logger.LogDebug(e.ToString()); }            

        }

        /// <summary>
        /// Applying several filters
        /// </summary>
        /// <param name="d_start">Use YYYY-MM-DDThh:mm:ssZ template</param>
        /// <param name="d_end">Use YYYY-MM-DDThh:mm:ssZ template</param>
        /// <returns>Returns IEnumerable result</returns>
        [HttpGet]
        [Route("GetByMultiplyFilters")]
        public IEnumerable<Scheme.Results> GetByMultiplyFilters(string? name, DateTime? d_start, DateTime? d_end, double? t_start, double? t_end, decimal? v_start, decimal? v_end)
        {   
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureCreated();
                IQueryable<Scheme.Results> res = db.Results;

                if (name is not null) res = res.Select(x => x).Where(x => x.FileNamesFileNameId == db.FileNames.First(a => a.Name == name).FileNameId);
                if (d_start is not null && d_end is not null) res = res.Select(x => x).Where(x => x.MinDate > d_start && x.MinDate < d_end);
                if (t_start is not null && t_end is not null) res = res.Select(x => x).Where(x => x.AverageExecutionTime > t_start && x.AverageExecutionTime < t_end);
                if (v_start is not null && v_end is not null) res = res.Select(x => x).Where(x => x.AverageValue > v_start && x.AverageValue < v_end);

                return res.ToArray();
            }
        }

        /// <summary>
        /// Sotring the last 10 records from file by date 
        /// </summary>
        /// <returns>Returns IEnumerable result</returns>
        [HttpGet]
        [Route("GetLastTenValues")]
        public IEnumerable<Values> GetLastTenValues(string name)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureCreated();

                int id = db.FileNames.First(a => a.Name == name).FileNameId; //try catch

                return db.Values
                    .Select(x => x)
                    .Where(x => x.FileNamesFileNameId == id) //Reference Loop Exception
                    .OrderBy(x => x.ValueId)
                    .Take(10)
                    .OrderBy (x => x.Date)
                    .ToArray();
            }
        }

    }
}

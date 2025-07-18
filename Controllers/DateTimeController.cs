using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Globalization;
using System.Text;
using System.Xml.Linq;
using Test_Task_Datetime.Scheme;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var directory = @"C:\data";
            string filePath = Path.Combine(directory, file.FileName);
            /*using (FileStream fstream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                byte[] buffer = new byte[fstream.Length];
                await fstream.ReadAsync(buffer, 0, buffer.Length);
                string textFromFile = Encoding.Default.GetString(buffer);
                Console.WriteLine($"Текст из файла: {textFromFile}");
            }*/
            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters("/n");

                while (!parser.EndOfData)
                {
                    try 
                    { 
                        string[] fields = parser.ReadFields();  

                        foreach (string field in fields) 
                        {
                            string[] args = field.Split(";");
                            try
                            {
                                this.Put(args[0], Convert.ToInt32(args[1]), Decimal.Round(Decimal.Parse(args[2], CultureInfo.InvariantCulture), 4));
                            }
                            catch (Exception e) { Console.WriteLine(e); }
                        }
                    }
                    catch (Exception e) { Console.WriteLine(e); } //catch null reference if empty file
                }

            }
        }

        [HttpPut]
        public void Put(string date, int time, decimal value)
        {
            //DBQuery.InsertData(value, id);

            using (ApplicationContext db = new ApplicationContext())
            {

                Values d = new Values { date = DateTime.ParseExact(date,
                                       "yyyy-MM-dd'T'HH-mm-ss.ffff'Z'",
                                       CultureInfo.InvariantCulture,
                                       DateTimeStyles.AssumeUniversal |
                                       DateTimeStyles.AdjustToUniversal).ToUniversalTime(), executiontime = time, value = value
                }; //some useless parsing tho 

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

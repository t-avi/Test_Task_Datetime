using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using System.Runtime.Intrinsics.X86;
using Test_Task_Datetime.Scheme;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Test_Task_Datetime
{
    public class DataValidation
    {
        public DataValidation() { }

        public static DateTimeOffset TryParseValidateDate(string d) 
        {
            /*parsing date with template*/
            /*this one can throwin several exceptions*/
            string dateTemplate = "yyyy-MM-dd'T'HH-mm-ss.ffff'Z'";

            var result = DateTime.ParseExact(d,
                                dateTemplate,
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.AssumeUniversal |
                                DateTimeStyles.AdjustToUniversal);

            if (DateTime.Compare(new DateTime(2000,1,1), result) > 0) 
                throw new Exception("Date is earlier than 01.01.2000");

            if (DateTime.Compare(DateTime.Now, result) < 0) 
                throw new Exception($"Date is later than now: {result}");
            return result;
        }
        public static int TryParseValidateExecutionTime(string t) 
        {
            var result = int.Parse(t);
            if (result < 0)
                throw new Exception($"Execution Time less than Zero: {result}");
            return result;
        }
        public static decimal TryParseValidateValue(string v) 
        {
            var result = Decimal.Round(Decimal.Parse(v, CultureInfo.InvariantCulture), 4);
            if (result < 0)
                throw new Exception($"Value Less Than Zero: {result}");
            return result;
        }
        public static void ValidateFileMeta(IFormFile f) //string instead of ifromfile lol
        {
            var directory = @"C:\data";
            string filePath = Path.Combine(directory, f.FileName);

            if (File.Exists(filePath))
            {
                var i = File.ReadAllLines(filePath).Count();
                if (i < 1)
                    throw new Exception($"Empty File: {f.FileName}");
                if (i > 10000)
                    throw new Exception($"File Data Is More Than 10k Lines: {f.FileName}");
            }
            else
            {
                throw new Exception($"File {f.FileName} Does Not Exists");
            }
        }
        public static (DateTimeOffset Date, int ExTime, decimal Value) ValidateParsing(string data) 
        {
            string[] content = data.Split(";");
            return (TryParseValidateDate(content[0]), TryParseValidateExecutionTime(content[1]), TryParseValidateValue(content[2]));
        }
        public static void TryPutData(IFormFile file)
        {
            var directory = @"C:\data";
            string filePath = Path.Combine(directory, file.FileName);

            FileNames f = new FileNames { Name = file.FileName };

            ValidateFileMeta(file);

            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters("/n");                

                using (ApplicationContext db = new ApplicationContext())
                {
                    db.Database.EnsureCreated();                    
                    /*if new file with old name will have invalid data 
                         * then old valid data will be deleted because of one thread methods
                         * to solve this one shuold using async but i have no enough time*/

                    if (db.FileNames.FirstOrDefault(a => a.Name == file.FileName) != null)
                    {
                        var filenameid = db.FileNames.First(a => a.Name == file.FileName).FileNameId;
                        db.Values
                            .Where(a => a.FileNamesFileNameId == filenameid)
                            .ExecuteDelete();
                        db.SaveChanges();

                        db.Results
                            .Where(a => a.FileNamesFileNameId == filenameid)
                            .ExecuteDelete();
                        db.SaveChanges();

                        db.FileNames
                            .Where(a => a.Name == file.FileName)
                            .ExecuteDelete();
                        db.SaveChanges();
                        //db.FileNames.Remove(f); //debug ex later.....cant cascade delete here
                    }

                    db.SaveChanges(); 
                    
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();                        

                        foreach (string field in fields!)
                        {
                            try
                            {
                                /*check all field for valid values then add*/                           
                                var result = ValidateParsing(field);

                                Values d = new Values
                                {
                                    Date = result.Date,
                                    ExecutionTime = result.ExTime,
                                    Value = result.Value,
                                    Filename = f
                                };

                                db.Values.Add(d);
                                db.SaveChanges();
                            }
                            catch (Exception)
                            {
                                /*useing cascade delete and throwin ex*/
                                db.FileNames.Remove(f);
                                db.SaveChanges();
                                throw;
                            }

                        }
                    }
                                        
                    var calcs = DataCalculation.MakeAllCalculations(db.Values, f);
                    Scheme.Results r = new Scheme.Results 
                    {
                        DeltaTime = calcs.DeltaTime,
                        MinDate = calcs.MinDate,
                        AverageExecutionTime = calcs.AvgExTime,
                        AverageValue = calcs.AvgValue,
                        MedianValue = calcs.MdnValue,
                        MinValue = calcs.MinValue,  
                        MaxValue = calcs.MaxValue,
                        Filename = f
                    };
                    db.Results.Add(r);
                    db.SaveChanges();
                }

            }

        }

    }
}

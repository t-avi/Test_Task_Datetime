namespace Test_Task_Datetime
{
    interface IDataValidation
    {
        public void ValidateDate(string d);
        public void ValidateExecutionTime(int t);
        public void ValidateValue(decimal value);
        public void ValidateParsing(string data); 
        public void TryPutData();

    }
}

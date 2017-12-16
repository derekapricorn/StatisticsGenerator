using System;
namespace StatisticsGenerator
{
    public class MinValueCalculation : ICalculation
    {
        public string VarName { get; set; }
        public string PeriodChoice { get; set; }
        public string StatCalc { get; set; }
        public ResultType Result { get; set; } = new ResultType(double.MaxValue, 0);
        public void HandleLine(double[] values)
        {

            var value = OperationDictionary.PeriodOperation[PeriodChoice](values);
            if (value < Result.Value)
            {
                Result.Value = value;
            }
        }
        public string ReturnFinal()
        {
            string finalResult = Result.Value.ToString();
            Console.WriteLine($"{VarName}\t{StatCalc}\t{finalResult}");
            return $"{VarName}\t{StatCalc}\t{finalResult}";
        }
    }
}

using System;
namespace StatisticsGenerator
{
    public class AverageCalculation : ICalculation
    {
        public string VarName { get; set; }
        public string PeriodChoice { get; set; }
        public string StatCalc { get; set; }
        public ResultType Result { get; set; } = new ResultType(0, 0);
        public void HandleLine(double[] values)
        {
            var value = OperationDictionary.PeriodOperation[PeriodChoice](values);
            Result.Value += value;
            Result.Count++;
        }
        public string ReturnFinal()
        {
            string finalResult = (Result.Value / Result.Count).ToString();
            Console.WriteLine($"{VarName}\t{StatCalc}\t{finalResult}");
            return $"{VarName}\t{StatCalc}\t{finalResult}";
        }
    }
}

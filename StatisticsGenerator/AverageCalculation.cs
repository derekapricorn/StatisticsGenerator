using System;
namespace StatisticsGenerator
{
    /// <summary>
    /// Calculate the task where statistic selector is Average
    /// </summary>
    public class AverageCalculation : ICalculation
    {
        public string VarName { get; set; }
        public string PeriodChoice { get; set; }
        public string StatCalc { get; set; }
        public ResultType Result { get; set; } = new ResultType();

        public void HandleLine(double[] values)
        {
            var value = OperationDictionary.PeriodOperation[PeriodChoice](values);
            Result.Value += value;
            Result.Count++;
        }
        public string ReturnFinal()
        {
            string finalResult = (Result.Value / Result.Count).ToString();
            string outputString = $"{VarName}\t{StatCalc}\t{finalResult}";
            Console.WriteLine(outputString);
            return outputString;
        }
    }
}

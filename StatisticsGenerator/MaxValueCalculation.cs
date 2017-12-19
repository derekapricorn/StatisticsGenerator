using System;
namespace StatisticsGenerator
{
    /// <summary>
    /// Calculate the task where statistic selector is MaxValue
    /// </summary>
    public class MaxValueCalculation : ICalculation
    {
        public string VarName { get; set; }
        public string PeriodChoice { get; set; }
        public string StatCalc { get; set; }
        public ResultType Result { get; set; } = new ResultType(double.MinValue, 0);
        public void HandleLine(double[] values)
        {
            var value = OperationDictionary.PeriodOperation[PeriodChoice](values);
            if (value > Result.Value)
            {
                Result.Value = value;
            }
        }
        public string ReturnFinal()
        {
            string finalResult = Result.Value.ToString();
            string outputString = $"{VarName}\t{StatCalc}\t{finalResult}";
            Console.WriteLine(outputString);
            return outputString;
        }
    }
}

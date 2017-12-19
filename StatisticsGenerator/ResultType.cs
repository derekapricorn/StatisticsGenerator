using System;
namespace StatisticsGenerator
{
    /// <summary>
    /// Define the result type for stat calculation
    /// </summary>
    public class ResultType
    {
        public double Value { get; set; }
        public int Count { get; set; }
        public ResultType()
        {
            Value = 0;
            Count = 0;
        }
        public ResultType(double value, int count)
        {
            Value = value;
            Count = count;
        }
    }
}

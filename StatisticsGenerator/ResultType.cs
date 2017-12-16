using System;
namespace StatisticsGenerator
{
    public class ResultType
    {
        public double Value { get; set; }
        public int Count { get; set; }
        public ResultType(double value, int count)
        {
            Value = value;
            Count = count;
        }
        public ResultType()
        {
            Value = 0;
            Count = 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
namespace StatisticsGenerator
{
    public class OperationDictionary
    {
        public static IDictionary<string, Func<double[], double>> PeriodOperation { get; set; } = new Dictionary<string, Func<double[], double>>()
        {
            {"MinValue", Min},
            {"MaxValue", Max},
            {"FirstValue", First},
            {"LastValue", Last},
        };

        public static double Min(double[] arr)
        {
            return arr.Min();
        }
        public static double Max(double[] arr)
        {
            return arr.Max();
        }
        public static double First(double[] arr)
        {
            return arr[0];
        }
        public static double Last(double[] arr)
        {
            return arr[arr.Count() - 1];
        }
    }
}

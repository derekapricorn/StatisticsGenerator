using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
namespace StatisticsGenerator
{
    public interface ICalculation
    {
        string VarName { get; set; }
        string PeriodChoice { get; set; }
        string StatCalc { get; set; }
        ResultType Result { get; set; }
        void HandleLine(double[] values);
        string ReturnFinal();
    }
}

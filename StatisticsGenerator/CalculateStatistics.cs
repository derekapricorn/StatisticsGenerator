using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
namespace StatisticsGenerator
{
    public class CalculateStatistics
    {
        //public List<ICalculation> taskList = new List<ICalculation>();
        public string DataPath { get; set; }
        public string Configuration { get; set; }
        public string TotalTemp { get; set; }

        static readonly string[] varNameSet = {"CashPrem", "AvePolLoanYield",
            "ResvAssumed"};
        static readonly string[] statSet = { "MinValue", "MaxValue", "Average" };
        static readonly string[] periodSet = {"FirstValue", "LastValue",
            "MinValue", "MaxValue"};

        public static Dictionary<string, Type> GetCalculationTypes()
        {
            string calcName;
            Dictionary<string, Type> dict = new Dictionary<string, Type>();
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type mytype in types
                     .Where(mytype => mytype.GetInterfaces()
                            .Contains(typeof(ICalculation)))) //assume the interface is known to client
            {
                calcName = mytype.ToString().Replace("Calculation", "").Replace(mytype.Namespace, "").Replace(".", "");
                dict[calcName] = mytype;
            }
            return dict;
        }

        public CalculateStatistics(string[] inputArr)
        {
            DataPath = inputArr[0];
            TotalTemp = inputArr[1];
            Configuration = inputArr[2];
        }

        //Parse configuration commands. 
        //Return true if commands are successfully parsed, false otherwise.
        public List<ICalculation> ParseConfig(Dictionary<string, Type> Operations)
        {
            Console.WriteLine("Parse config file");
            string varName;
            string statCalc;
            string period;
            var taskList = new List<ICalculation>();
            string[] configLines = System.IO.File.ReadAllLines(Path.Combine(DataPath, Configuration));
            configLines = configLines.Where(x => !string.IsNullOrEmpty(x)).ToArray(); // remove empty strings
            foreach (string line in configLines)
            {
                string[] strArr = line.Trim().Split();
                if (ValidateArguments(strArr))
                {
                    varName = strArr[0];
                    statCalc = strArr[1];
                    period = strArr[2];

                    //see if the name matches with statCalc
                    //if so instanitiate with implementation
                    if (Operations.ContainsKey(statCalc))
                    {
                        ICalculation newOp = (StatisticsGenerator.ICalculation)Operations[statCalc].GetConstructor(new Type[] { }).Invoke(new object[] { });
                        newOp.PeriodChoice = period;
                        newOp.VarName = varName;
                        newOp.StatCalc = statCalc;
                        taskList.Add(newOp);
                    }
                }
            }
            return taskList;
        }

        //validate if config arguments exist in the sets.
        private bool ValidateArguments(string[] strArr)
        {
            return varNameSet.Contains(strArr[0]) && statSet.Contains(strArr[1])
                             && periodSet.Contains(strArr[2]);
        }

        //Process data by traversing through the table row by row.
        //summarize across columns if varName matches.
        public void ProcessData(List<ICalculation> taskList)
        {
            //read data line by line 
            Console.WriteLine("Process Data");
            try 
            {
                using (StreamReader sr = new StreamReader(Path.Combine(DataPath, TotalTemp))) {
                    string line;
                    while ((line = sr.ReadLine()) != null) {
                        string[] lineEntries = line.Trim().Split();
                        lineEntries = lineEntries.Where(x => !string.IsNullOrEmpty(x)).ToArray(); // remove empty strings
                        if (lineEntries[0] == "ScenId") //skip first row
                        {
                            continue;
                        }
                        string lineVarName = lineEntries[1];
                        int size = lineEntries.Count();
                        var dataEntries= lineEntries.Skip(2).Take(size - 2).Select(double.Parse).ToArray();
                        foreach (ICalculation operation in taskList) {
                            if (operation.VarName == lineVarName) 
                            {
                                operation.HandleLine(dataEntries);
                            }
                        }
                    }
                } 
            }
            catch (Exception e)
            {
                Console.WriteLine("Data cannot be processed:");
                Console.WriteLine(e.Message);
            }
        }

        public void SaveResult(List<ICalculation> taskList)
        {
            Console.WriteLine("save result");
            string[] output = new string[taskList.Count()];
            int index = 0;
            foreach (ICalculation operation in taskList)
            {
                output[index++] = operation.ReturnFinal();
            }
            //output result to file
            string outputPath = System.IO.Path.Combine(DataPath, "Results.txt");
            System.IO.File.WriteAllLines(outputPath, output);
        }
    }
}

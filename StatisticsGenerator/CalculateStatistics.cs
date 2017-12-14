using System;
using System.Collections.Generic;
using System.Linq;
namespace StatisticsGenerator
{
    public class CalculateStatistics
    {
        string _input;
        string _configuration;
        string _totalTemp;
        List<Task> taskList = new List<Task>();

        public string Input { get { return _input; } set { _input = value; } }
        public string Configuration { get { return _configuration; } set { _configuration = value; } }
        public string TotalTemp { get { return _totalTemp; } set { _totalTemp = value; } }

        public CalculateStatistics(string _input)
        {
            this._input = _input;
        }

        //Cascading strucutre to process data only if all satisfactory conditions are met
        public void Run()
        {
            if (this.ValidateInput())
            {
                if (this.ValidateFiles())
                {
                    if (this.ParseConfig())
                    {
                        this.ProcessData();
                        this.SaveResult();
                    }
                }
            }
        }

        //valid input command. Return true if input is valid, false otherwise.
        public bool ValidateInput()
        {
            var strArr = this.Input.Trim().Split();
            if (strArr.Length != 2)
            {
                Console.WriteLine("Invalid input: check number of files");
                return false;
            }
            //update properties to reflect the pathnames
            var dir = System.IO.Directory.GetCurrentDirectory().Replace("bin/Debug", "data");
            TotalTemp = System.IO.Path.Combine(dir, strArr[0]);
            Configuration = System.IO.Path.Combine(dir, strArr[1]);
            return true;
        }

        //Validate if the files exist in the path specified either implicitly or explicitly
        //Return true if files exist, false otherwise
        public bool ValidateFiles(params string[] list)
        {
            Console.WriteLine("Validate Files");

            //account for the unit test case where path was not established earlier
            if (list.Length == 1)
            {
                TotalTemp = list[0] + TotalTemp;
                Configuration = list[0] + Configuration;
            }

            if (System.IO.File.Exists(TotalTemp) && System.IO.File.Exists(Configuration))
            {

                return true;
            }
            else
            {
                Console.WriteLine(TotalTemp);
                Console.WriteLine(Configuration);
                Console.WriteLine("file(s) couldn't be found");
                return false;
            }
        }

        //Parse configuration commands. 
        //Return true if commands are successfully parsed, false otherwise.
        public bool ParseConfig()
        {
            Console.WriteLine("Parse config file");
            string[] configLines = System.IO.File.ReadAllLines(Configuration);
            foreach (string line in configLines)
            {
                string[] strArr = line.Trim().Split();
                if (ValidateArguments(strArr))
                {
                    taskList.Add(new Task(strArr));
                }
            }
            return (taskList.Any());
        }

        static readonly string[] varNameSet = {"CashPrem", "AvePolLoanYield",
            "ResvAssumed"};
        static readonly string[] statSet = { "MinValue", "MaxValue", "Average" };
        static readonly string[] periodSet = {"FirstValue", "LastValue",
            "MinValue", "MaxValue"};

        //validate if config arguments exist in the sets.
        private bool ValidateArguments(string[] strArr)
        {
            return varNameSet.Contains(strArr[0]) && statSet.Contains(strArr[1])
                             && periodSet.Contains(strArr[2]);
        }

        //Process data by traversing through the table row by row.
        //summarize across columns if varName matches.
        public void ProcessData()
        {
            //read data line by line 
            Console.WriteLine("Process Data");

            foreach (string line in System.IO.File.ReadLines(TotalTemp))
            {
                string[] lineEntries = line.Trim().Split();
                lineEntries = lineEntries.Where(x => !string.IsNullOrEmpty(x)).ToArray(); // remove empty strings
                if (lineEntries[0] == "ScenId")
                {
                    continue;
                }
                string lineVarName = lineEntries[1];
                foreach (Task task in taskList)
                {
                    if (task.varName == lineVarName)
                    {
                        task.ResultList.Add(Calculate(lineEntries, task.periodChoice));
                    }
                }
            }
        }

        private double Calculate(string[] lineEntries, string periodChoice)
        {
            //calculate from entry 2 to end according to periodChoice
            int size = lineEntries.Count();
            var list = lineEntries.Skip(2).Take(size - 2).Select(double.Parse).ToArray();
            Console.WriteLine("we are calculating");
            switch (periodChoice)
            {
                case "FirstValue":
                    return list[0];
                case "LastValue":
                    return list[list.Count() - 1];
                case "MinValue":
                    return list.Min();
                case "MaxValue":
                    return list.Max();
            }
            return 0d;
        }

        ////Calculate aggregate results from tast objects
        public void SaveResult()
        {
            Console.WriteLine("save result");
            string[] output = new string[taskList.Count()];
            int index = 0;
            foreach (Task task in taskList)
            {
                string name = task.varName;
                string stat = task.statCalc;
                switch (task.statCalc)
                {
                    case "MinValue":
                        string minValue = task.ResultList.Min().ToString();
                        Console.WriteLine($"{name}\t{stat}\t{minValue}");
                        output[index++] = $"{name}\t{stat}\t{minValue}";
                        break;
                    case "MaxValue":
                        string maxValue = task.ResultList.Max().ToString();
                        Console.WriteLine($"{name}\t{stat}\t{maxValue}");
                        output[index++] = $"{name}\t{stat}\t{maxValue}";
                        break;
                    case "Average":
                        string avgValue = task.ResultList.Average().ToString();
                        Console.WriteLine($"{name}\t{stat}\t{avgValue}");
                        output[index++] = $"{name}\t{stat}\t{avgValue}";
                        break;
                }
            }

            //output result to file
            var dir = System.IO.Directory.GetCurrentDirectory().Replace("bin/Debug", "data");
            string outputPath = System.IO.Path.Combine(dir, "Results.txt");
            System.IO.File.WriteAllLines(outputPath, output);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
namespace StatisticsGenerator
{
    public class CalculateStatistics
    {
        public List<ICalculation> taskList = new List<ICalculation>();
        public string Input { get; set; }
        public string DataPath { get; set; }
        public string Configuration { get; set; }
        public string TotalTemp { get; set; }

        public CalculateStatistics(string _input)
        {
            Input = _input;
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
            if (strArr.Length != 3)
            {
                Console.WriteLine("Invalid input: check number of files");
                return false;
            }
            //check if path is valid
            //update properties to reflect the pathnames
            if (Directory.Exists(strArr[0]))
            {
                DataPath = strArr[0];
                TotalTemp = System.IO.Path.Combine(DataPath, strArr[1]);
                Configuration = System.IO.Path.Combine(DataPath, strArr[2]);
                return true;
            }
            Console.WriteLine("Invalid path");
            return false;            
        }

        //Validate if the files exist in the path specified either implicitly or explicitly
        //Return true if files exist, false otherwise
        public bool ValidateFiles()
        {
            Console.WriteLine("Validate Files");
            Console.WriteLine("Totaltemp is at " + TotalTemp);
            Console.WriteLine("configuration is at" + Configuration);

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

        public Dictionary<string, Type> FindAvaialbleTypes ()
        {
            string calcName;
            //Dictionary<string, ICalculation> dict = new Dictionary<string, ICalculation>(StringComparer.InvariantCultureIgnoreCase);
            Dictionary<string, Type> dict = new Dictionary<string, Type>();
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type mytype in types
                     .Where(mytype => mytype.GetInterfaces()
                            .Contains(typeof(ICalculation)))) //assume the interface is known to client
            {
                calcName = mytype.ToString().Replace("Calculation", "").Replace(mytype.Namespace, "").Replace(".","");
                dict[calcName] = mytype;
            }
            return dict;
        }

        //Parse configuration commands. 
        //Return true if commands are successfully parsed, false otherwise.
        public bool ParseConfig()
        {
            Console.WriteLine("Parse config file");

            string varName;
            string statCalc;
            string period;
            string[] configLines = System.IO.File.ReadAllLines(Configuration);
            var Operations = FindAvaialbleTypes();
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
            try 
            {
                using (StreamReader sr = new StreamReader(TotalTemp)) {
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
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        public void SaveResult()
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StatisticsGenerator
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.Title = typeof(MainClass).Name;
            Run();
        }

        /* The Run() function makes the console works like a interactive terminal
         * It will wait for proper input and create an object which will process statistics
         * It will exit if user types "exit" 
         */
        static void Run()
        {
            string consoleInput = "";
            while (!consoleInput.Equals("exit"))
            {
                consoleInput = ReadFromConsole();
                if (string.IsNullOrWhiteSpace(consoleInput))
                {
                    continue;
                }
                try
                {
                    var inputArr = InputParser.Parse(consoleInput);
                    if (inputArr != null)
                    {
                        //create calculation classes

                        var calculationDict = CalculateStatistics.GetCalculationTypes();

                        if (calculationDict.Count != 0)
                        {
                            //calculate statistics and save to file
                            var statCalculator = new CalculateStatistics(inputArr);
                            var taskList = statCalculator.ParseConfig(calculationDict);
                            if (taskList.Any())
                            {
                                statCalculator.ProcessData(taskList);
                                statCalculator.SaveResult(taskList);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    WriteToConsole(e.Message);
                }
            }
        }

        public static void WriteToConsole(string message = "")
        {
            if (message.Length > 0)
            {
                Console.WriteLine(message);
            }
        }

        const string _readPrompt = "user> ";
        public static string ReadFromConsole()
        {
            Console.Write(_readPrompt);
            return Console.ReadLine();
        }
    }
}

using System;
using System.IO;
namespace StatisticsGenerator
{
    /// <summary>
    ///  Parse the user input and output errors if necessary
    /// </summary>
    public static class InputParser
    {
        public static string[] Parse(string input)
        {
            var inputArr = ValidateInput(input);
            if (inputArr != null)
            {
                return ValidateFiles(inputArr);
            }
            return null;
        }

        public static string[] ValidateInput(string input)
        {
            if (input == null) 
            {
                return null;
            }
            Console.WriteLine("validate input");
            var strArr = input.Trim().Split();
            if (strArr.Length != 3)
            {
                Console.WriteLine("Invalid input");
                return null;
            }
            return strArr;
        }

        public static string[] ValidateFiles(string[] strArr)
        {
            //check if path is valid
            //update properties to reflect the pathnames
            Console.WriteLine("validate if files exist");
            string path = strArr[0];
            if (Directory.Exists(path))
            {
                string dataName = Path.Combine(path, strArr[1]);
                string configName = Path.Combine(path, strArr[2]);
                if (File.Exists(dataName) && File.Exists(configName))
                {
                    Console.WriteLine("files found");
                    return strArr;
                }
                Console.WriteLine("file(s) couldn't be found");
            }
            return null;
        }
    }
}

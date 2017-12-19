using System;
using NUnit.Framework;
using StatisticsGenerator;
using System.Linq;
namespace StatisticsGeneratorTests
{
    [TestFixture]
    public class TestInputFromUser
    {
        [Test]
        public void ValidateInput_InvalidArgumentNumber_ReturnNull()
        {
            //test invalid number of arguments
            var testObject = InputParser.ValidateInput("invalidFilename");
            Assert.IsNull(testObject);
        }

        [Test]
        public void ValidateInput_ValidArguments_ReturnNull()
        {
            //test right number of arguments
            var testObject = InputParser.ValidateInput("/Users/Derek/Projects/StatisticsGenerator/data filename1 filename2");
            Assert.IsNotNull(testObject);
        }
    }

    [TestFixture]
    public class TestFileNameValidation
    {
        [Test]
        public void ValidateFiles_InvalidFilename_ReturnNull()
        {
            //test filenames that don't exist
            string[] strArr = InputParser.ValidateInput("/Users/Derek/Projects/StatisticsGenerator/data TotalTemp.txt randomfile.txt");
            Assert.IsNull(InputParser.ValidateFiles(strArr));
        }

        [Test]
        public void ValidateFiles_ValidFilenames_ReturnStringArray()
        {
            //test filenames that exist
            string[] strArr = InputParser.ValidateInput("/Users/Derek/Projects/StatisticsGenerator/data TotalTemp.txt Configuration.txt");
            Console.WriteLine(InputParser.ValidateFiles(strArr)[1]);
            Assert.AreEqual(InputParser.ValidateFiles(strArr)[0], "/Users/Derek/Projects/StatisticsGenerator/data");
            Assert.AreEqual(InputParser.ValidateFiles(strArr)[1], "TotalTemp.txt");
            Assert.AreEqual(InputParser.ValidateFiles(strArr)[2], "Configuration.txt");
        }
    }

    [TestFixture]
    public class TestParseConfig
    {
        [Test]
        public void ParseConfig_InvalidConfig_ReturnNull()
        {
            //test if at least one row can be matched
            var strArr = InputParser.ValidateInput("/Users/Derek/Projects/StatisticsGenerator/data TotalTemp.txt ConfigTest1.txt");
            var testObj = new CalculateStatistics(strArr);
            Assert.AreEqual(testObj.ParseConfig(CalculateStatistics.GetCalculationTypes()).Count(), 0);
        }

        [Test]
        public void ParseConfig_ValidConfig_ReturnObject()
        {
            //test if at least one row can be matched
            var strArr = InputParser.ValidateInput("/Users/Derek/Projects/StatisticsGenerator/data TotalTemp.txt ConfigTest2.txt");
            var testObj = new CalculateStatistics(strArr);
            var dict = CalculateStatistics.GetCalculationTypes();
            var taskList = testObj.ParseConfig(dict);
            Assert.AreEqual(testObj.ParseConfig(dict).Count, 3);
        }
    }

    [TestFixture]
    public class TestSingleCalculation
    {
        [Test]
        public void ProcessData_CashPremMinMin_ReturnCorrectCalculation()
        {
            var strArr = InputParser.ValidateInput("/Users/Derek/Projects/StatisticsGenerator/data TotalTemp.txt CashPremMinMin.txt");
            var testObj = new CalculateStatistics(strArr);
            var dict = CalculateStatistics.GetCalculationTypes();
            var taskList = testObj.ParseConfig(dict);
            testObj.ProcessData(taskList);
            string actual = taskList[0].ReturnFinal().Split('\t')[2];
            double calculated = new[] { 4655947.13, 0, 0 }.Min();
            Console.WriteLine("acutal is {0}, calculated is {1}", actual, calculated.ToString());
            Assert.AreEqual(actual, calculated.ToString());
        }

        [Test]
        public void ProcessData_ResvAssumedMaxMax_ReturnCorrectCalculation()
        {
            var strArr = InputParser.ValidateInput("/Users/Derek/Projects/StatisticsGenerator/data TotalTemp.txt ResvAssumedMaxMax.txt");
            var testObj = new CalculateStatistics(strArr);
            var dict = CalculateStatistics.GetCalculationTypes();
            var taskList = testObj.ParseConfig(dict);
            testObj.ProcessData(taskList);
            string actual = taskList[0].ReturnFinal().Split('\t')[2];
            double calculated = new[] { -27923645.44, -27923645.44, -27923645.44 }.Max();
            Console.WriteLine("acutal is {0}, calculated is {1}", actual, calculated.ToString());
            Assert.AreEqual(actual, calculated.ToString());
        }
        [Test]
        public void ProcessData_AvePolLoanYieldAverageFirst_ReturnCorrectCalculation()
        {
            var strArr = InputParser.ValidateInput("/Users/Derek/Projects/StatisticsGenerator/data TotalTemp.txt AvePolLoanYieldAverageFirst.txt");
            var testObj = new CalculateStatistics(strArr);
            var dict = CalculateStatistics.GetCalculationTypes();
            var taskList = testObj.ParseConfig(dict);
            testObj.ProcessData(taskList);
            string actual = taskList[0].ReturnFinal().Split('\t')[2];
            double calculated = new[] { 0.00, 0.00, 0.00 }.Average();
            Console.WriteLine("acutal is {0}, calculated is {1}", actual, calculated.ToString());
            Assert.AreEqual(actual, calculated.ToString());
        }

        [Test]
        public void ProcessData_CashPremAverageLast_ReturnCorrectCalculation()
        {
            var strArr = InputParser.ValidateInput("/Users/Derek/Projects/StatisticsGenerator/data TotalTemp.txt CashPremAverageLast.txt");
            var testObj = new CalculateStatistics(strArr);
            var dict = CalculateStatistics.GetCalculationTypes();
            var taskList = testObj.ParseConfig(dict);
            testObj.ProcessData(taskList);
            string actual = taskList[0].ReturnFinal().Split('\t')[2];
            double calculated = new[] { 84655947.13, 84655914.86, 84655947.13 }.Average();
            Console.WriteLine("acutal is {0}, calculated is {1}", actual, calculated.ToString());
            Assert.AreEqual(actual, calculated.ToString());
        }
    }

    [TestFixture]
    public class TestMultipleCalculations
    {
        [Test]
        public void ProcessData_AvePolLoanYieldAverageFirstCashPremAverageLast_ReturnCorrectCalculation()
        {
            var strArr = InputParser.ValidateInput("/Users/Derek/Projects/StatisticsGenerator/data TotalTemp.txt AvePolLoanYieldAverageFirstCashPremAverageLast.txt");
            var testObj = new CalculateStatistics(strArr);
            var dict = CalculateStatistics.GetCalculationTypes();
            var taskList = testObj.ParseConfig(dict);
            testObj.ProcessData(taskList);
            string actual = taskList[0].ReturnFinal().Split('\t')[2];
            double calculated = new[] { 0.00, 0.00, 0.00 }.Average();
            string actual2 = taskList[1].ReturnFinal().Split('\t')[2];
            double calculated2 = new[] { 84655947.13, 84655914.86, 84655947.13 }.Average();
            Assert.AreEqual(actual + actual2, calculated.ToString() + calculated2.ToString());
        }
    }
}

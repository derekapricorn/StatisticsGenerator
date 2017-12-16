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
        public void InvalidNumberOfArguments()
        {
            //test invalid number of arguments
            var testObject = new CalculateStatistics("invalidFilename");
            Assert.IsFalse(testObject.ValidateInput());
        }

        [Test]
        public void ValidNumberOfArguments()
        {
            //test right number of arguments
            var testObject = new CalculateStatistics("/Users/Derek/Projects/StatisticsGenerator/data filename1 filename2");
            Assert.IsTrue(testObject.ValidateInput());
        }
    }

    [TestFixture]
    public class TestFileNameValidation 
    {
        [Test]
        public void InvalidFilename() {
            //test filenames that don't exist
            var testObject = new CalculateStatistics("/Users/Derek/Projects/StatisticsGenerator/data TotalTemp.txt randomfile.txt");
            testObject.ValidateInput();
            Assert.IsFalse(testObject.ValidateFiles());
        }

        [Test]
        public void ValidFilenames() {
            //test filenames that exist
            var testObject = new CalculateStatistics("/Users/Derek/Projects/StatisticsGenerator/data TotalTemp.txt Configuration.txt");
            testObject.ValidateInput();
            //need to pass in the absolute path for test
            Assert.IsTrue(testObject.ValidateFiles());
        }
    }

    [TestFixture]
    public class TestParseConfig 
    {
        [Test]
        public void InvalidConfig() {
            //test if at least one row can be matched
            var testObject = new CalculateStatistics("/Users/Derek/Projects/StatisticsGenerator/data TotalTemp.txt ConfigTest1.txt");
            testObject.ValidateInput();
            testObject.ValidateFiles();
            Assert.IsFalse(testObject.ParseConfig());
        }

        [Test]
        public void ValidConfig()
        {
            //test if at least one row can be matched
            var testObject = new CalculateStatistics("/Users/Derek/Projects/StatisticsGenerator/data TotalTemp.txt ConfigTest2.txt");
            testObject.ValidateInput();
            testObject.ValidateFiles();
            Assert.IsTrue(testObject.ParseConfig());
        }
    }

    [TestFixture]
    public class TestSingleCalculation 
    {
        [Test]
        public void CashPremMinMin()
        {
            var testObject = new CalculateStatistics("/Users/Derek/Projects/StatisticsGenerator/data TotalTemp.txt CashPremMinMin.txt");
            testObject.ValidateInput();
            testObject.ValidateFiles();
            testObject.ParseConfig();
            testObject.ProcessData();
            string actual = testObject.taskList[0].ReturnFinal().Split('\t')[2];
            double calculated = new[] {4655947.13, 0, 0}.Min();
            Console.WriteLine("acutal is {0}, calculated is {1}",actual, calculated.ToString());
            Assert.AreEqual(actual, calculated.ToString());
        }
        [Test]
        public void ResvAssumedMaxMax()
        {
            var testObject = new CalculateStatistics("/Users/Derek/Projects/StatisticsGenerator/data TotalTemp.txt ResvAssumedMaxMax.txt");
            testObject.ValidateInput();
            testObject.ValidateFiles();
            testObject.ParseConfig();
            testObject.ProcessData();
            string actual = testObject.taskList[0].ReturnFinal().Split('\t')[2];
            double calculated = new[] { -27923645.44, -27923645.44, -27923645.44 }.Max();
            Console.WriteLine("acutal is {0}, calculated is {1}", actual, calculated.ToString());
            Assert.AreEqual(actual, calculated.ToString());
        }
        [Test]
        public void AvePolLoanYieldAverageFirst()
        {
            var testObject = new CalculateStatistics("/Users/Derek/Projects/StatisticsGenerator/data TotalTemp.txt AvePolLoanYieldAverageFirst.txt");
            testObject.ValidateInput();
            testObject.ValidateFiles();
            testObject.ParseConfig();
            testObject.ProcessData();
            string actual = testObject.taskList[0].ReturnFinal().Split('\t')[2];
            double calculated = new[] { 0.00, 0.00, 0.00 }.Average();
            Console.WriteLine("acutal is {0}, calculated is {1}", actual, calculated.ToString());
            Assert.AreEqual(actual, calculated.ToString());
        }

        [Test]
        public void CashPremAverageLast()
        {
            var testObject = new CalculateStatistics("/Users/Derek/Projects/StatisticsGenerator/data TotalTemp.txt CashPremAverageLast.txt");
            testObject.ValidateInput();
            testObject.ValidateFiles();
            testObject.ParseConfig();
            testObject.ProcessData();
            string actual = testObject.taskList[0].ReturnFinal().Split('\t')[2];
            double calculated = new[] { 84655947.13, 84655914.86, 84655947.13 }.Average();
            Console.WriteLine("acutal is {0}, calculated is {1}", actual, calculated.ToString());
            Assert.AreEqual(actual, calculated.ToString());
        }
    }

    [TestFixture]
    public class TestMultipleCalculations
    {
        [Test]
        public void AvePolLoanYieldAverageFirstCashPremAverageLast() 
        {
            var testObject = new CalculateStatistics("/Users/Derek/Projects/StatisticsGenerator/data TotalTemp.txt AvePolLoanYieldAverageFirstCashPremAverageLast.txt");
            testObject.ValidateInput();
            testObject.ValidateFiles();
            testObject.ParseConfig();
            testObject.ProcessData();
            string actual = testObject.taskList[0].ReturnFinal().Split('\t')[2];
            double calculated = new[] { 0.00, 0.00, 0.00 }.Average();
            string actual2 = testObject.taskList[1].ReturnFinal().Split('\t')[2];
            double calculated2 = new[] { 84655947.13, 84655914.86, 84655947.13 }.Average();
            //Console.WriteLine("acutal is {0}, calculated is {1}", actual, calculated.ToString());
            Assert.AreEqual(actual+actual2, calculated.ToString()+calculated2.ToString());
        }
    }
}

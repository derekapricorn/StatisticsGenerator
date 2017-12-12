using System;
using NUnit.Framework;

namespace StatisticsGenerator
{
    [TestFixture]
    public class StatisticsGeneratorTest
    {
        const string dataPath = "/Users/Derek/Projects/StatisticsGenerator/StatisticsGenerator/data";    
        [TestFixture]
        public class TestInputFromUser
        {
            [Test]
            public void RandomInput()
            {
                //test an input line without "sg" at the beginning
                var testObject = new CalculateStatistics("random input");
                
                Assert.IsFalse(testObject.ValidateInput());
            }

            [Test]
            public void InvalidNumberOfArguments()
            {
                //test invalid number of arguments
                var testObject = new CalculateStatistics("sg invalidFilename");
                Assert.IsFalse(testObject.ValidateInput());
            }

            [Test]
            public void ValidNumberOfArguments()
            {
                //test right number of arguments
                var testObject = new CalculateStatistics("sg filename1 filename2");
                Assert.IsTrue(testObject.ValidateInput());
            }
        }

        [TestFixture]
        public class TestFileNameValidation 
        {
            [Test]
            public void InvalidFilename() {
                //test filenames that don't exist
                var testObject = new CalculateStatistics("sg TotalTemp.txt randomfile.txt");
                Assert.IsFalse(testObject.ValidateFiles());
            }

            [Test]
            public void ValidFilenames() {
                //test filenames that exist
                var testObject = new CalculateStatistics("sg TotalTemp.txt Configuration.txt");
                testObject.ValidateInput();
                //need to pass in the absolute path for test
                Assert.IsTrue(testObject.ValidateFiles(dataPath));
            }
        }
        [TestFixture]
        public class TestParseConfig {
            [Test]
            public void InvalidConfig() {
                //test if at least one row can be matched
                var testObject = new CalculateStatistics("sg whatever ConfigTest1.txt");
                testObject.ValidateInput();
                testObject.ValidateFiles(dataPath);
                Assert.IsFalse(testObject.ParseConfig());
            }

            [Test]
            public void ValidConfig()
            {
                //test if at least one row can be matched
                var testObject = new CalculateStatistics("sg whatever ConfigTest2.txt");
                testObject.ValidateInput();
                testObject.ValidateFiles(dataPath);
                Assert.IsTrue(testObject.ParseConfig());
            }
        }
    }
}

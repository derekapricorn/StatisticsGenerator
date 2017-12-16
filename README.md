# Statistics Generator
---
## Description
This C# program is a command line tool that calculates simple statistics from a tab delimited text file.

- Important files
	- _/StatisticsGenerator.sln_: the project entry.
	- _/StatisticsGenerator/data/TotalTemp.txt_: data file
	- _/StatisticsGenerator/data/Configuration.txt_: configuration file where all the statistics commands are.
	- _/StatisticsGenerator/StatisticsGenerator/MainClass.cs_: Program entry.
	- _/StatisticsGenerator/StatisticsGeneratorTests/StatisticsGeneratorTest.cs_: NUnit test cases.
	- _/StatisticsGenerator/StatisticsGenerator/ICalculation.cs_: Calculation interface
	- _/StatisticsGenerator/StatisticsGenerator/MaxValueCalculation.cs_: Max calculation derived from ICalculation
	- _/StatisticsGenerator/StatisticsGenerator/MinValueCalculation.cs_: Min calculation derived from ICalculation
	- _/StatisticsGenerator/StatisticsGenerator/AverageCalculation.cs_: Average calculation derived from ICalculation
---

## System Requirements
Any compiler that can operate on .NET framework, although Visual Studio is preferred to run the assembly.

---

## How to Run the Program
1. Clone the project.
2. Open project **StatisticsGenerator** in Visual Studio.
3. Run project in Visual Studio.
4. To run analysis, type `$datapath TotalTemp.txt Configuration.txt`. Please replace the $datapth to the path of your own. Currently the data folder is stored right under the project solution folder.
5. To exit the program, type `exit`.

---

## Additional Info
The unit test cases were used to examine how the program handles invalid inputs. I have verified that the program can handle:
- inappropriate number of command line arguments
- invalid file names
- invalid configuration lines in the configuration.txt file.
- single Min/Max/Average calculations
- single First/Last/Min/Max phase calculations
- multiple calculations

## Updates
- Each class is in its own file.
- Data streaming was implemented to accommodate large file loading.
- Reflection was used to adhere to "Factory Pattern".
- Tests are in a separate project which references the main project.

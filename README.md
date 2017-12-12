# Statistics Generator
---
## Description
This C# program is a command line tool that calculates simple statistics from a tab deliminted text file.

- Important files
	- _/StatisticsGenerator.sln_: the project entry.
	- _/StatisticsGenerator/StatisticsGenerator/data/TotalTemp.txt_: data file
	- _/StatisticsGenerator/StatisticsGenerator/data/Configuration.txt_: configuration file where all the statistics commands are.
	- _/StatisticsGenerator/StatisticsGenerator/Program.cs_: C# script for the project.
	- _/StatisticsGenerator/StatisticsGenerator/StatisticsGeneratorTest.cs_: NUnit test cases.
---

## System Requirements
Any compiler that can operate on .NET framework, although Visual Studio is preferred to run the assembly.

---

## How to Run the Program
1. Clone the project.
2. Open project **StatisticsGenerator** in Visual Studio.
3. Run project in Visual Studio.
4. To run analysis, type `sg TotalTemp.txt Configuration.txt`. **sg** stands for StatisticsGenerator.
5. To exit the program, type `exit`.

---

## Additional Info
The unit test cases were used to examine how the program handles invalid inputs. Currently they don't support the exmination of the accuracy of statistics. To verify the statistics result, you will have to manually calculate the results and match entries.


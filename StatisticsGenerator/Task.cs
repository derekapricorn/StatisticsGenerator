using System;
using System.Collections.Generic;
namespace StatisticsGenerator
{
        /* The Task Class describes the task specified by the commands in Configuration.txt.
     * varName describes which type of variable the task is dealing
     * statCalc describes which type of statistic calculation it is
     * periodChoice describes which period we are selecting
     * ResultList is a list of results from all the rows that meet the varName requirement
     * From ResultList the aggregate result can be calculated
     */
    class Task
    {
        public string varName;
        public string statCalc; //min, max, average
        public string periodChoice; //first, last, min, max
        public List<double> ResultList;

        public Task() { }

        public Task(string[] input)
        {
            this.varName = input[0];
            this.statCalc = input[1];
            this.periodChoice = input[2];
            ResultList = new List<double>();
        }
    }
}

using System;
using System.Collections.Generic;

namespace ProductPlatformAnalyzer
{

    public partial class OutputExp
    {
        public OutputExp(string lname, string lvalue)
        {
            name = lname;
            value = lvalue;
            state = FindState(lname);
        }

        public int state { get; set; }

        public string name { get; set; }

        public string value { get; set; }

        public override string ToString()
        {
            return  name + " = " + value;
        }

        private int FindState(string name)
        {
            int tempState = -1;
            if (name.Contains("_"))
            {
                String[] lOperationNameParts = name.Split('_');
                tempState = Convert.ToInt32(lOperationNameParts[3]);
            }

            return tempState;
        }
    }

    public class OutputHandler
    {
        private List<OutputExp> outputResult;

        public OutputHandler()
        {
            outputResult = new List<OutputExp>();
        }

        public void addExp(string name, string value)
        {
            OutputExp temp = new OutputExp(name, value);
            outputResult.Add(temp);
        }

        public void Print()
        {
            foreach(OutputExp exp in outputResult)
            {
                Console.WriteLine(exp.ToString());
            }
        }

        public void SortAfterState()
        {
            outputResult.Sort(delegate(OutputExp exp1, OutputExp exp2)
                {
                    return exp1.state.CompareTo(exp2.state);
                });

        }

        public void printTrue()
        {
            foreach (OutputExp exp in outputResult)
            {
                if(exp.value == "true")
                    Console.WriteLine(exp.ToString());
            }

        }

    }
}
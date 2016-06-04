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
            setValues(lname);
        }

        public int state { get; set; }

        public string name { get; set; }

        public string operation { get; set; }

        public string value { get; set; }

        public string opState { get; set; }

        public int variant { get; set; }

        public override string ToString()
        {
            return  name + " = " + value;
        }

        private void setValues(string name)
        {
            if (name.Contains("_"))
            {
                String[] lOperationNameParts = name.Split('_');
                operation = lOperationNameParts[0];
                opState = lOperationNameParts[1];
                variant = Convert.ToInt32(lOperationNameParts[2]);
                state = Convert.ToInt32(lOperationNameParts[3]);
            }
            else
            {
                operation = null;
                opState = null;
                variant = -1;
                state = -1;
            }

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

        public void printVariants()
        {
            foreach (OutputExp exp in outputResult)
            {
                if (exp.state == -1)
                    Console.WriteLine(exp.ToString());
            }

        }


        public void printOpTransformations()
        {
            foreach (OutputExp exp in outputResult)
            {
                OutputExp nextOp = findNextOp(exp);
                if (nextOp != null)
                    if (String.Equals(exp.value, "true") && String.Equals(nextOp.value, "true"))
                    Console.WriteLine(exp.ToString() + " -> " + nextOp.ToString());
            }

        }

        private OutputExp findNextOp(OutputExp first)
        {
            OutputExp next = null;
            if (first.name.Contains("_"))
            {
                foreach (OutputExp exp in outputResult)
                {
                    if (exp.name.Contains("_"))
                    {
                        if (first.state == (exp.state + 1) &&
                            String.Equals(first.operation, exp.operation) &&
                            String.Equals(nextOpState(first.opState), exp.opState) &&
                            first.variant == exp.variant &&
                            !String.Equals(first.opState, "U") &&
                            !String.Equals(first.opState, "F"))
                        {
                            next = exp;
                        }
                    }
                }
            }

            return next;

        }

        public string nextOpState(string lstate)
        {
            string next = null;

            switch (lstate)
            {
                case "I": next = "E"; break;
                case "E": next = "F"; break;
                case "F": next = "F"; break;
                case "U": next = "U"; break;
            }
            return next;
        }


        internal void printCounterExample()
        {
            int lastState = getLastState();

            Console.WriteLine("\nVariants:");
            printVariants();

            Console.WriteLine("\nOperation states:");
            printOpState(lastState);

            Console.WriteLine("\nFalse pre/post-conditions:");
            printConditionsState(0);
        }

        private void printConditionsState(int lstate)
        {
            foreach (OutputExp exp in outputResult)
            {
                if (exp.state == lstate &&
                    (String.Equals(exp.opState, "PostCondition") || 
                    (String.Equals(exp.opState, "PreCondition")))&&
                    String.Equals(exp.value, "false"))
                    Console.WriteLine(exp.ToString());
            }
        }

        private void printOpState(int lstate)
        {
            foreach (OutputExp exp in outputResult)
            {
                if (exp.state == lstate &&
                   !String.Equals(exp.opState, "U") &&
                   !String.Equals(exp.opState, "PostCondition") &&
                   !String.Equals(exp.opState, "PreCondition"))
                {
                    Console.WriteLine(exp.ToString());
                }
            }
        }

        private int getLastState()
        {
            int lastState = 0;
            foreach (OutputExp exp in outputResult)
            {
                if (exp.state > lastState)
                {
                    lastState = exp.state;
                }
            }
            return lastState-1;
        }
    }
}
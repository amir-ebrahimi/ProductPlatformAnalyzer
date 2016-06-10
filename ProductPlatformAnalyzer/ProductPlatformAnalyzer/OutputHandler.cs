using System;
using System.IO;
using System.Web.UI;
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

        public void addExp(string name, string value, int pState)
        {
            if (!goalState(name, pState))
            {
                OutputExp temp = new OutputExp(name, value);
                outputResult.Add(temp);
            }
        }

        public void SortAfterState()
        {

            outputResult.Sort(delegate(OutputExp exp1, OutputExp exp2)
            {
                return exp1.state.CompareTo(exp2.state);
            });

        }

        public void SortAfterValue()
        {
            outputResult.Sort(delegate(OutputExp exp1, OutputExp exp2)
            {
                return exp2.value.CompareTo(exp1.value);
            });

        }


        public void printFinished()
        {
            SortAfterValue();
            Console.WriteLine("\nVariants: ");
            printVariants();

            SortAfterState();
            Console.WriteLine("\nOperations in order: ");
            printOpTransformations();


        }

        public void writeFinished()
        {
            StringWriter stringwriter = new StringWriter();
            List<String> variants = getChosenVariants();
            SortAfterState();
            List<String[]> transformations = getOpTransformations();
            String[] transPair;


            HtmlTextWriter writer = new HtmlTextWriter(stringwriter);

            writer.WriteBeginTag("p");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Chosen variants:");
            writer.WriteEndTag("p");


            writer.WriteBeginTag("ul style=\"list-style-type:none\"");
            writer.Write(HtmlTextWriter.TagRightChar);

            foreach (String var in variants)
            {
                writer.WriteBeginTag("li");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(var);
                writer.WriteEndTag("li");
            }
            writer.WriteEndTag("ul");

            writer.WriteBeginTag("p");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Operation sequence:");
            writer.WriteEndTag("p");


            writer.WriteBeginTag("table border=\"1\" cellpadding='5' cellspacing='0' Gridlines=\"both\"  ");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteBeginTag("tr");
            writer.Write(HtmlTextWriter.TagRightChar);
             
            
                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("Operation"); 
                writer.WriteEndTag("td");

                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("I->E");
                writer.WriteEndTag("td");


                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("E->F");
                writer.WriteEndTag("td");


            writer.WriteEndTag("tr");

            foreach (String[] trans in transformations)
            {
                if (String.Equals("E", trans[2]))
                {
                    writer.WriteBeginTag("tr");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    transPair = transformations.Find(x => String.Equals(x[0], trans[0]) &&
                                                          String.Equals(x[3], trans[3]) &&
                                                          !String.Equals(x[2], trans[2]));

                    writer.WriteBeginTag("td");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Write(trans[0]);
                    writer.WriteEndTag("td");


                    writer.WriteBeginTag("td");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    if (String.Equals("E", trans[2]))
                        writer.Write(trans[1]);
                    else if (transPair != null)
                    {
                        if (String.Equals("E", transPair[2]))
                            writer.Write(transPair[1]);
                    }

                    writer.WriteEndTag("td");

                    writer.WriteBeginTag("td");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    if (String.Equals("F", trans[2]))
                        writer.Write(trans[1]);
                    else if (transPair != null)
                    {
                        if (String.Equals("F", transPair[2]))
                            writer.Write(transPair[1]);
                    }
                    writer.WriteEndTag("td");

                    writer.WriteEndTag("tr");
                }
            }
            writer.WriteEndTag("table");


            File.WriteAllText("myfile.htm", stringwriter.ToString());

        }

        public void printCounterExample()
        {
            int lastState = getLastState();

            SortAfterValue();
            Console.WriteLine("\nVariants:");
            printVariants();
            
            Console.WriteLine("\nOperation in last state:");
            printOpState(lastState);

            SortAfterState();
            Console.WriteLine("\nOperations in order: ");
            printOpTransformations(lastState);

            Console.WriteLine("\nFalse pre/post-conditions:");
            printConditionsState(0);
        }

        public void Print()
        {
            foreach (OutputExp exp in outputResult)
            {
                Console.WriteLine(exp.ToString());
            }
        }

        public void printTrue()
        {
            foreach (OutputExp exp in outputResult)
            {
                if (exp.value == "true")
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

        public List<String> getChosenVariants()
        {
            List<String> vars = new List<String>();
            foreach (OutputExp exp in outputResult)
            {
                if (exp.state == -1 && String.Equals(exp.value, "true"))
                    vars.Add(exp.name);
            }

            return vars;
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

        private List<String[]> getOpTransformations()
        {
            List<String[]> transforms = new List<String[]>();
            String [] item = new String[4];

            foreach (OutputExp exp in outputResult)
            {
                OutputExp nextOp = findNextOp(exp);
                if (nextOp != null)
                    if (String.Equals(exp.value, "true") && String.Equals(nextOp.value, "true"))
                    {
                        item = new String[4];
                        item[0] = exp.operation;
                        item[1] = nextOp.state.ToString();
                        item[2] = nextOp.opState;
                        item[3] = nextOp.variant.ToString();
                        transforms.Add(item);
                    }
            }

            return transforms;
        }

        public void printOpTransformations(int max)
        {
            foreach (OutputExp exp in outputResult)
            {
                OutputExp nextOp = findNextOp(exp);
                if (nextOp != null && nextOp.state <= max)
                    if (String.Equals(exp.value, "true") && String.Equals(nextOp.value, "true"))
                        Console.WriteLine(exp.ToString() + " -> " + nextOp.ToString());
            }

        }


        private bool goalState(string name, int pState)
        {
            for (int i = 0; i <= pState; i++)
            {
                if (String.Equals(name, ("P" + i)))
                    return true;
            }
            return false;
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
                        if ((first.state +1) == exp.state &&
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

        private string nextOpState(string lstate)
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
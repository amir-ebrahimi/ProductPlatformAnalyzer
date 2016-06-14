using System;
using System.IO;
using System.Web.UI;
using System.Collections.Generic;

namespace ProductPlatformAnalyzer
{
    //Helping class for storing ouput expressions
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

    //Class for storing and writing results
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

        //Prints values for showing a finished analysis to console
        public void printFinished()
        {
            SortAfterValue();
            Console.WriteLine("\nVariants: ");
            printVariants();

            SortAfterState();
            Console.WriteLine("\nOperations in order: ");
            printOpTransformations();
        }

        //Writes values for showing a finished analysis to HTML-file
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
            writer.Write("Operations progressing at state:");
            writer.WriteEndTag("p");


            writer.WriteBeginTag("table border=\"1\" cellpadding='5' cellspacing='0' Gridlines=\"both\" style=\"margin-left:40px\" ");
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


            File.WriteAllText("result.htm", stringwriter.ToString());

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


        public void writeCounterExample()
        {
            StringWriter stringwriter = new StringWriter();
            SortAfterValue();
            List<String> variants = getChosenVariants();
            List<String[]> operationState = getOpState(getLastState());
            SortAfterState();
            List<String[]> transformations = getOpTransformations();
            List<String> conditions = getConditionsState(0);
            String[] transPair;
            String[] transF;
            String[] transI;


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
            writer.Write("Operation values in last state:");
            writer.WriteEndTag("p");



            writer.WriteBeginTag("table border=\"1\" cellpadding='5' cellspacing='0' Gridlines=\"both\"  style=\"margin-left:40px\"");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteBeginTag("tr");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteBeginTag("td");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Operation");
            writer.WriteEndTag("td");

            writer.WriteBeginTag("td");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("I");
            writer.WriteEndTag("td");

            writer.WriteBeginTag("td");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("E");
            writer.WriteEndTag("td");

            writer.WriteBeginTag("td");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("F");
            writer.WriteEndTag("td");


            writer.WriteEndTag("tr");

            foreach (String[] trans in operationState)
            {
                if (String.Equals("E", trans[2]))
                {
                    writer.WriteBeginTag("tr");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    transF = operationState.Find(x => String.Equals(x[0], trans[0]) &&
                                                       String.Equals(x[2], "F"));

                    transI = operationState.Find(x => String.Equals(x[0], trans[0]) &&
                                                       String.Equals(x[2], "I"));

                    if (String.Equals(trans[3], "false") && String.Equals(transI[3], "false") &&
                                                            String.Equals(transF[3], "false"))
                    {
                        transI[3] = "-";
                        trans[3] = "-";
                        transF [3] = "-";
                    }

                    writer.WriteBeginTag("td");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Write(trans[0]);
                    writer.WriteEndTag("td");


                    writer.WriteBeginTag("td");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    if (transI != null)
                        writer.Write(transI[3]);

                    writer.WriteEndTag("td");

                    writer.WriteBeginTag("td");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    if (String.Equals("E", trans[2]))
                        writer.Write(trans[3]);

                    writer.WriteEndTag("td");

                    writer.WriteBeginTag("td");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    if (transF != null)
                        writer.Write(transF[3]);

                    writer.WriteEndTag("td");

                    writer.WriteEndTag("tr");
                }
            }
            writer.WriteEndTag("table");


            writer.WriteBeginTag("p");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Operations progressing at state:");
            writer.WriteEndTag("p");


            writer.WriteBeginTag("table border=\"1\" cellpadding='5' cellspacing='0' Gridlines=\"both\"  style=\"margin-left:40px\"");
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

            if (transformations.Count == 0)
            {
                writer.WriteBeginTag("tr");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("-");
                writer.WriteEndTag("td");

                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("-");
                writer.WriteEndTag("td");

                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("-");
                writer.WriteEndTag("td");

                writer.WriteEndTag("tr");
            }

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

            writer.WriteBeginTag("p");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Post/precondition state:");
            writer.WriteEndTag("p");

            writer.WriteBeginTag("ul style=\"list-style-type:none\"");
            writer.Write(HtmlTextWriter.TagRightChar);

            foreach (String con in conditions)
            {
                writer.WriteBeginTag("li");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(con);
                writer.WriteEndTag("li");
            }
            writer.WriteEndTag("ul");


            File.WriteAllText("counterEx.htm", stringwriter.ToString());

        }

        //Prints all output expressions
        public void Print()
        {
            foreach (OutputExp exp in outputResult)
            {
                Console.WriteLine(exp.ToString());
            }
        }

        //Print all true outputexpressions
        public void printTrue()
        {
            foreach (OutputExp exp in outputResult)
            {
                if (exp.value == "true")
                    Console.WriteLine(exp.ToString());
            }
        }

        //Print all variants
        public void printVariants()
        {
            foreach (OutputExp exp in outputResult)
            {
                if (exp.state == -1)
                    Console.WriteLine(exp.ToString());
            }

        }

        //Returns all chosen variants
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

        //Print all operation transformations
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

        //Returns all operation transformations
        private List<String[]> getOpTransformations()
        {
            List<String[]> transforms = new List<String[]>();
            String [] item = new String[4];
            int lastState = getLastState(); 

            foreach (OutputExp exp in outputResult)
            {
                OutputExp nextOp = findNextOp(exp);
                if (nextOp != null)
                    if (String.Equals(exp.value, "true") && String.Equals(nextOp.value, "true"))
                    {
                        item = new String[4];
                        item[0] = exp.operation; //Name of operation
                        item[1] = nextOp.state.ToString(); //State after finished transition
                        item[2] = nextOp.opState; //Operation status (I/E/F) after transition
                        item[3] = nextOp.variant.ToString(); //Operation variant
                        transforms.Add(item);
                    }
            }

            return transforms;
        }

        //Print all operation transformations up to a state
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

        //Returns true is name is representing a goal selection varable
        private bool goalState(string name, int pState)
        {
            for (int i = 0; i <= pState; i++)
            {
                if (String.Equals(name, ("P" + i)))
                    return true;
            }
            return false;
        }

        //Returns operation in next state and with the following operation status
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

        //Returns the following operation status
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

        //Prints false pre and post conditions for lstate
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

        //Returns false pre and post conditions for lstate
        private List<String> getConditionsState(int lstate)
        {
            List<String> con = new List<String>();
            foreach (OutputExp exp in outputResult)
            {
                if (exp.state == lstate &&
                    (String.Equals(exp.opState, "PostCondition") ||
                    (String.Equals(exp.opState, "PreCondition"))) &&
                    String.Equals(exp.value, "false"))
                    con.Add(exp.ToString());
            }
            return con;
        }

        //Prints operations in lstate
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

        //Returns operations inlstate
        private List<String[]> getOpState(int lstate)
        {
            List<String[]> ops = new List<String[]>();
            String[] item;
            foreach (OutputExp exp in outputResult)
            {
                if (exp.state == lstate &&
                   !String.Equals(exp.opState, "U") &&
                   !String.Equals(exp.opState, "PostCondition") &&
                   !String.Equals(exp.opState, "PreCondition"))
                {
                    item = new String[4];
                    item[0] = exp.operation;
                    item[1] = exp.state.ToString();
                    item[2] = exp.opState;
                    item[3] = exp.value;
                    ops.Add(item);
                }
            }
            return ops;
        }

        //Returns the last state of analysis
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
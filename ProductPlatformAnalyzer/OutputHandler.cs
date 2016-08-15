using System;
using System.IO;
using System.Web.UI;
using System.Collections.Generic;
using System.Collections;

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
            return name + " = " + value;
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
        private string path = "../../Output/";
        private FrameworkWrapper fwrapper;

        public OutputHandler(FrameworkWrapper wrapper)
        {
            outputResult = new List<OutputExp>();
            fwrapper = wrapper;
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
            HtmlTextWriter writer = new HtmlTextWriter(stringwriter);

            writeDocStart(writer);
            writeTabList(writer);

            writer.WriteFullBeginTag("div id=\"tabs-1\"");
            writeInput(writer);
            writer.WriteEndTag("div");


            writer.WriteFullBeginTag("div id=\"tabs-2\"");
            writer.WriteBeginTag("p style=\"font-size:22px\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("The analysis was successful, all operations can be perfomed in the presented order.");
            writer.WriteEndTag("p");

            writeChosenVariants(writer);
            writeTransitionTableState(writer);
            writeOpOrder(writer);
            writeTransitionDiagram(writer);

            writer.WriteEndTag("div");
            writeDocEnd(writer);

            File.WriteAllText(path + "result.htm", stringwriter.ToString());

        }

        public void writeInputFile()
        {
            StringWriter stringwriter = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(stringwriter);

            writeInput(writer);

            File.WriteAllText(path + "input.htm", stringwriter.ToString());

        }

        private void writeInput(HtmlTextWriter writer)
        {

            writer.WriteBeginTag("p id=\"inF\" class=\"title\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Feature Model");

            writer.WriteBeginTag("span id=\"titleFArr\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("&#x25BC");
            //upp &#x25B2
            writer.WriteEndTag("span");
            writer.WriteEndTag("p");

            writer.WriteBeginTag("div id=\"inFContent\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writeVariants(writer);
            writeConstraints(writer);
            writer.WriteEndTag("div");

            writeOperationsWithPrePostCon(writer);
            writeVariantOperationMappings(writer);

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
            printConditionsState(lastState);
        }


        public void writeCounterExample()
        {
            StringWriter stringwriter = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(stringwriter);

            writeDocStart(writer);
            writeTabList(writer);

            writer.WriteFullBeginTag("div id=\"tabs-1\"");
            writeInput(writer);
            writer.WriteEndTag("div");

            writer.WriteFullBeginTag("div id=\"tabs-2\"");
            writer.WriteBeginTag("p style=\"font-size:22px\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Counterexample found, all operations needed could not be performed.");
            writer.WriteEndTag("p");

            writeChosenVariants(writer);
            //writeOpStateTable(writer);
            writeTransitionTableState(writer);
            writeOpOrder(writer);
            writeTransitionDiagram(writer);
            writeFalsePrePost(writer);

            writer.WriteEndTag("div");
            writeDocEnd(writer);

            File.WriteAllText(path + "counterEx.htm", stringwriter.ToString());

        }

        public void writeDebugFile()
        {
            StringWriter stringwriter = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(stringwriter);
            SortAfterState();

            foreach (OutputExp exp in outputResult)
            {
                writer.WriteBeginTag("p");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(exp);
                writer.WriteEndTag("p");
            }

            File.WriteAllText(path + "debug.htm", stringwriter.ToString());

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
            string var, vg;
            foreach (OutputExp exp in outputResult)
            {
                if (exp.state == -1)
                {
                    var = exp.ToString();
                    vg = fwrapper.getVariantGroup(var.Split(' ')[0]);
                    Console.WriteLine(vg + "." + var);
                }
            }

        }

        private void writeDocStart(HtmlTextWriter writer)
        {
            writer.WriteLine("<!doctype html> <html lang=\"en\"> <head><meta charset=\"utf-8\">"
                                + "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">"
                                + "<title>Product Platform Analyser</title>"
                                + "<link rel=\"stylesheet\" href=\"../css/jquery-ui.css\">"
                                + "<link rel=\"stylesheet\" href=\"/resources/demos/style.css\">"
                                + "<link rel=\"stylesheet\" href=\"../css/style.css\">"
                                + "<script src=\"../js/jquery-1.12.4.js\"></script> "
                                + "<script src=\"../js/jquery-ui.js\"></script><script>"
                                + "$( function() {$( \"#tabs\" ).tabs();} );</script>"
                                + "<script>$(document).ready(function(){$(\"#inFContent\").hide();"
                                + " $(\"#inOContent\").hide(); $(\"#inMContent\").hide();"
                                + "});</script></head><body>");
        }

        private void writeDocEnd(HtmlTextWriter writer)
        {
            writer.WriteLine("</body> </html>");
        }

        private void writeTabList(HtmlTextWriter writer)
        {
            writer.WriteLine("<div id=\"tabs\">" +
                             "<ul>" +
                             "<li><a href=\"#tabs-1\">Input</a></li>" +
                             "<li><a href=\"#tabs-2\">Result</a></li>" +
                             "</ul>");
        }

        private void writeTransitionDiagram(HtmlTextWriter writer)
        {
            List<string[]> transforms = getOpTransformations();
            writer.WriteBeginTag("p style=\"font-size:18px\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Order of operation transitions");
            writer.WriteEndTag("p");

            writer.WriteBeginTag("table style=\"margin-left:40px\"");
            //writer.WriteBeginTag("ol style=\"margin-left:1em;\" ");
            writer.Write(HtmlTextWriter.TagRightChar);


            writer.WriteBeginTag("tr");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Operation");
            writer.WriteEndTag("th");
            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Transition");
            writer.WriteEndTag("th");


            foreach (String[] trans in transforms)
            {
                writer.WriteBeginTag("tr");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);

                //Pil ner
                //writer.Write("&darr; ");
                writer.Write(trans[0]);

                writer.WriteEndTag("td");
                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);
                if (String.Equals(trans[2], "E"))
                    //pil upp
                    writer.Write(" &uarr;");
                else
                    //pil ner
                    writer.Write(" &darr;");

                writer.WriteEndTag("td");
                writer.WriteEndTag("tr");
            }
            writer.WriteEndTag("table");
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

        //Returns all chosen variants
        public List<string[]> getChosenVariantsWithGroup()
        {
            string var, vg;
            string[] list;
            List<string[]> vars = new List<string[]>();
            foreach (OutputExp exp in outputResult)
            {
                if (exp.state == -1 && String.Equals(exp.value, "true"))
                {

                    var = exp.name;
                    vg = fwrapper.getVariantGroup(var);
                    if (!String.Equals(vg, "Virtual-VG"))
                    {
                        list = new string[2] { vg, var };
                        vars.Add(list);
                    }
                }
            }
            return vars;
        }


        //Returns all chosen variant groups
        public List<string> getChosenVariantGroups()
        {
            string var, vg;
            List<string> vars = new List<string>();
            foreach (OutputExp exp in outputResult)
            {
                if (exp.state == -1 && String.Equals(exp.value, "true"))
                {

                    var = exp.name;
                    vg = fwrapper.getVariantGroup(var);
                    if (!String.Equals(vg, "Virtual-VG") && !vars.Contains(vg))
                    {
                        vars.Add(vg);
                    }
                }
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

        private void writeChosenVariants(HtmlTextWriter writer)
        {
            List<String[]> variants = getChosenVariantsWithGroup();
            List<String> groups = getChosenVariantGroups();

            writer.WriteBeginTag("p style=\"font-size:18px\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Chosen variants");
            writer.WriteEndTag("p");


            writer.WriteBeginTag("table style=\"margin-left:40px\"");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteBeginTag("tr");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Variants");
            writer.WriteEndTag("th");
            writer.WriteEndTag("tr");

            foreach (String vg in groups)
            {
                writer.WriteBeginTag("tr");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.WriteBeginTag("th class=\"vg\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(vg);
                writer.WriteEndTag("th");
                writer.WriteEndTag("tr");
                foreach (string[] var in variants)
                    if (String.Equals(var[0], vg))
                    {
                        writer.WriteBeginTag("tr");
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.WriteBeginTag("td");
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Write(var[1]);
                        writer.WriteEndTag("td");
                        writer.WriteEndTag("tr");
                    }
            }
            writer.WriteEndTag("table");
        }

        private void writeVariants(HtmlTextWriter writer)
        {
            List<variantGroup> variants = fwrapper.getVariantGroupList();


            writer.WriteBeginTag("table style=\"margin-left:40px\"");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteBeginTag("tr");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Variant groups");
            writer.WriteEndTag("th");
            writer.WriteEndTag("tr");


            foreach (variantGroup group in variants)
            {
                if (!String.Equals(group.names, "Virtual-VG"))
                {
                    writer.WriteBeginTag("tr");
                    writer.Write(HtmlTextWriter.TagRightChar);


                    writer.WriteBeginTag("th class =\"vg\"");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    writer.WriteBeginTag("b");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Write(group.names);
                    writer.WriteEndTag("b");
                    writer.Write(" - " + group.gCardinality);

                    writer.WriteEndTag("th");

                    writer.WriteEndTag("tr");

                    foreach (variant var in group.variant)
                    {
                        writer.WriteFullBeginTag("tr");
                        writer.WriteBeginTag("td");
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Write(var.displayName);
                        writer.WriteEndTag("td");

                        writer.WriteEndTag("tr");
                    }

                }
            }
            writer.WriteEndTag("table");
        }

        private void writeConstraints(HtmlTextWriter writer)
        {
            ArrayList constraints = new ArrayList(fwrapper.getConstraintList());

            writer.WriteBeginTag("p style=\"font-size:18px\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write(" ");
            writer.WriteEndTag("p");


            writer.WriteBeginTag("table style=\"margin-left:40px\"");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteBeginTag("tr");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Constraints");
            writer.WriteEndTag("th");
            writer.WriteEndTag("tr");

            foreach (String con in constraints)
            {
                if (checkCondition(con))
                {
                    writer.WriteBeginTag("tr");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.WriteBeginTag("td");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Write(modCondition(con));
                    writer.WriteEndTag("td");
                    writer.WriteEndTag("tr");
                }
            }
            writer.WriteEndTag("table");
        }

        private void writeOperationsWithPrePostCon(HtmlTextWriter writer)
        {
            List<operation> operations = new List<operation>(fwrapper.OperationList);

            writer.WriteBeginTag("p id=\"inO\" class=\"title\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Operations");

            writer.WriteBeginTag("span id=\"titleOArr\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("&#x25BC");
            //upp &#x25B2
            writer.WriteEndTag("span");
            writer.WriteEndTag("p");

            writer.WriteBeginTag("div id=\"inOContent\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteBeginTag("table  style=\"margin-left:40px\"");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteBeginTag("tr");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.Write("Operation");

            writer.WriteEndTag("th");

            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.Write("Precondition");

            writer.WriteEndTag("th");


            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.Write("Postcondition");

            writer.WriteEndTag("th");
            foreach (operation op in operations)
            {

                writer.WriteBeginTag("tr");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.Write(op.names);

                writer.WriteEndTag("td");


                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.WriteBeginTag("ul style=\"list-style-type:none\"");
                writer.Write(HtmlTextWriter.TagRightChar);

                foreach (string pre in op.precondition)
                {
                    writer.WriteBeginTag("li");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Write(modCondition(pre));
                    writer.WriteEndTag("li");
                }
                writer.WriteEndTag("ul");

                writer.WriteEndTag("td");

                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.WriteBeginTag("ul style=\"list-style-type:none\"");
                writer.Write(HtmlTextWriter.TagRightChar);

                foreach (string post in op.postcondition)
                {
                    writer.WriteBeginTag("li");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Write(modCondition(post));
                    writer.WriteEndTag("li");
                }
                writer.WriteEndTag("ul");

                writer.WriteEndTag("td");

                writer.WriteEndTag("tr");
            }

            writer.WriteEndTag("table");
            writer.WriteEndTag("div");

        }
        private void writeVariantOperationMappings(HtmlTextWriter writer)
        {
            List<variantOperations> operations = new List<variantOperations>(fwrapper.getVariantsOperations());

            writer.WriteBeginTag("p id=\"inM\" class=\"title\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Variant operation mappings");

            writer.WriteBeginTag("span id=\"titleMArr\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("&#x25BC");
            writer.WriteEndTag("span");
            writer.WriteEndTag("p");

            writer.WriteBeginTag("div id=\"inMContent\"");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteBeginTag("table  style=\"margin-left:40px\"");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteBeginTag("tr");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.Write("Variants");

            writer.WriteEndTag("th");

            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.Write("Operations");

            writer.WriteEndTag("th");
            writer.WriteEndTag("tr");

            foreach (variantOperations vop in operations)
            {

                writer.WriteBeginTag("tr");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.Write(replaceVirtual(vop.getVariant().names));

                writer.WriteEndTag("td");


                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.WriteBeginTag("ul style=\"list-style-type:none\"");
                writer.Write(HtmlTextWriter.TagRightChar);

                foreach (operation op in vop.getOperations())
                {
                    writer.WriteBeginTag("li");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Write(op.names);
                    writer.WriteEndTag("li");
                }
                writer.WriteEndTag("ul");

                writer.WriteEndTag("td");

                writer.WriteEndTag("tr");
            }

            writer.WriteEndTag("table");
            writer.WriteEndTag("div");

        }

        private void writeFalsePrePost(HtmlTextWriter writer)
        {
            List<String[]> conditions = getConditionsStateWithValues(getLastState());
            writer.WriteBeginTag("p style=\"font-size:18px\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("False post/preconditions in last state");
            writer.WriteEndTag("p");

            writer.WriteBeginTag("table style=\"margin-left:40px\"");
            writer.Write(HtmlTextWriter.TagRightChar);


            writer.WriteBeginTag("tr");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Name");
            writer.WriteEndTag("th");
            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Condition");
            writer.WriteEndTag("th");
            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Value");
            writer.WriteEndTag("th");
            writer.WriteEndTag("tr");

            foreach (String[] con in conditions)
            {
                writer.WriteBeginTag("tr");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(con[0]);
                writer.WriteEndTag("td");
                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(con[1]);
                writer.WriteEndTag("td");
                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(con[2]);
                writer.WriteEndTag("td");
                writer.WriteEndTag("tr");
            }
            writer.WriteEndTag("table");
        }

        private void writeOpOrder(HtmlTextWriter writer)
        {
            Boolean first = true;
            writer.WriteBeginTag("p style=\"font-size:18px;\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Order of operations");
            writer.WriteEndTag("p");

            writer.WriteBeginTag("p style=\"margin-left:2.5em;\" ");
            writer.Write(HtmlTextWriter.TagRightChar);

            foreach (OutputExp exp in outputResult)
            {
                OutputExp nextOp = findNextOp(exp);
                if (nextOp != null)
                    if (String.Equals(exp.value, "true") && String.Equals(nextOp.value, "true") && String.Equals(exp.opState, "E"))
                    {
                        if (!first)
                        {
                            writer.Write(" &rarr; ");
                        }
                        else
                            first = false;
                        writer.Write(exp.operation);
                    }
            }

            writer.WriteEndTag("p");
        }

        private void writeOpStateTable(HtmlTextWriter writer)
        {
            SortAfterValue();
            List<String[]> operationState = getOpState(getLastState());
            String[] transF;
            String[] transI;

            writer.WriteBeginTag("p style=\"font-size:18px\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Operation statuses in last state:");
            writer.WriteEndTag("p");

            writer.WriteBeginTag("table style=\"margin-left:40px\"");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteBeginTag("tr");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Operation");
            writer.WriteEndTag("th");

            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("I");
            writer.WriteEndTag("th");

            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("E");
            writer.WriteEndTag("th");

            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("F");
            writer.WriteEndTag("th");


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
                        transF[3] = "-";
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
        }

        private void writeTransitionTableStatus(HtmlTextWriter writer)
        {
            SortAfterState();
            String[] transPair;
            List<String[]> transformations = getOpTransformations();
            writer.WriteBeginTag("p style=\"font-size:18px\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Operations progressing at state:");
            writer.WriteEndTag("p");


            writer.WriteBeginTag("table style=\"margin-left:40px\"");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteBeginTag("tr");
            writer.Write(HtmlTextWriter.TagRightChar);


            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Operation");
            writer.WriteEndTag("th");

            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("I->E");
            writer.WriteEndTag("th");


            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("E->F");
            writer.WriteEndTag("th");


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

        }


        private void writeTransitionTableState(HtmlTextWriter writer)
        {
            List<String> activeOps = getActiveOps();
            List<String[]> opTransitions;
            int lastState = getLastState();

            writer.WriteBeginTag("p style=\"font-size:18px\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Operation statuses in different states");
            writer.WriteEndTag("p");

            writer.WriteBeginTag("table style=\"margin-left:40px\"");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteBeginTag("tr");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Operation");
            writer.WriteEndTag("th");

            for (int i = 0; i <= lastState + 1; i++)
            {
                writer.WriteBeginTag("th");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("" + i);
                writer.WriteEndTag("th");
            }

            writer.WriteEndTag("tr");

            foreach (String op in activeOps)
            {
                opTransitions = getOpTransformations(op);

                writer.WriteBeginTag("tr");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(op);
                writer.WriteEndTag("td");

                for (int i = 0; i <= lastState + 1; i++)
                {
                    writer.WriteBeginTag("td");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    writer.Write(TransitionStateAt(opTransitions, i));
                    writer.WriteEndTag("td");

                }

                writer.WriteEndTag("tr");

            }
            writer.WriteEndTag("table");
        }


        //Returns all operation transformations
        private List<String[]> getOpTransformations()
        {
            List<String[]> transforms = new List<String[]>();
            String[] item = new String[4];
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

        private string TransitionStateAt(List<String[]> transformations, int state)
        {
            string currentState = "I";
            foreach (String[] trans in transformations)
            {
                int transState = Convert.ToInt32(trans[1]);
                if (transState <= state)
                {
                    if (String.Equals(trans[2], "F"))
                        return trans[2];
                    else
                        currentState = trans[2];
                }
            }
            return currentState;
        }


        //Returns all operation transformations
        private List<String[]> getOpTransformations(string operation)
        {
            List<String[]> transforms = new List<String[]>();
            String[] item = new String[4];
            int lastState = getLastState();

            foreach (OutputExp exp in outputResult)
            {
                if (String.Equals(exp.operation, operation))
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
                        if ((first.state + 1) == exp.state &&
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
                    (String.Equals(exp.opState, "PreCondition"))) &&
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

        //Returns false pre and post conditions for lstate with the pre/post condition
        private List<String[]> getConditionsStateWithValues(int lstate)
        {
            List<String[]> conditions = new List<String[]>();
            List<String> conValue = new List<String>();
            string[] list;
            foreach (OutputExp exp in outputResult)
            {
                if (exp.state == lstate &&
                    (String.Equals(exp.opState, "PostCondition") ||
                    (String.Equals(exp.opState, "PreCondition"))) &&
                    String.Equals(exp.value, "false"))
                {
                    if (String.Equals(exp.opState, "PostCondition"))
                        conValue = fwrapper.getPostconditionForOperation(exp.operation);
                    else
                        conValue = fwrapper.getPreconditionForOperation(exp.operation);
                    list = new string[3] { exp.operation + "_" + exp.opState, consToString(conValue), exp.value };
                    conditions.Add(list);
                }
            }
            return conditions;
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

        private string consToString(List<string> pcons)
        {
            string exp = modCondition(pcons[0]);
            pcons.RemoveAt(0);
            foreach (string con in pcons)
            {
                exp = exp + "and" + modCondition(con);
            }

            return exp;
        }

        //Returns operations inlstate
        private List<String> getActiveOps()
        {
            List<String> ops = new List<String>();
            foreach (OutputExp exp in outputResult)
            {
                if (exp.state == 0 &&
                   String.Equals(exp.opState, "I") &&
                   String.Equals(exp.value, "true") &&
                   !String.Equals(exp.opState, "PostCondition") &&
                   !String.Equals(exp.opState, "PreCondition"))
                {
                    ops.Add(exp.operation);
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
            return lastState - 1;
        }

        private string replaceVirtual(string p)
        {
            string newP = "";
            if (p.StartsWith("Virtual"))
            {
                virtualConnection con = fwrapper.findVirtualConnectionWithName(p);
                foreach (variant var in con.getVariants())
                {
                    newP = newP + var.names + "<br>";
                }
                return newP;
            }
            else
                return p;

        }


        private string modCondition(string con)
        {
            string newCon = null;

            //For each condition first we have to build its coresponding tree
            Parser lConditionParser = new Parser();
            Node<string> lCnstExprTree = new Node<string>("root");

            lConditionParser.AddChild(lCnstExprTree, con);

            foreach (Node<string> item in lCnstExprTree)
            {
                //Then we have to traverse the tree
                newCon = ParseCondition(item);
            }

            return newCon;
        }

        private bool checkCondition(string con)
        {
            //For each condition first we have to build its coresponding tree
            Parser lConditionParser = new Parser();
            Node<string> lCnstExprTree = new Node<string>("root");

            lConditionParser.AddChild(lCnstExprTree, con);

            foreach (Node<string> item in lCnstExprTree)
            {
                //Then we have to traverse the tree
                return ParseConditionVirtual(item);
            }

            return true;
        }


        private string ParseCondition(Node<string> pNode)
        {
            try
            {
                List<Node<string>> lChildren = new List<Node<string>>();
                string newCon = null;
                if ((pNode.Data != "and") && (pNode.Data != "or") && (pNode.Data != "not"))
                {
                    //We have one operator
                    newCon = printStatus(pNode.Data);
                }
                else
                {
                    foreach (Node<string> lChild in pNode.Children)
                    {
                        lChildren.Add(lChild);
                    }
                    switch (pNode.Data)
                    {
                        case "and":
                            {
                                newCon = "(" + ParseCondition(lChildren[0]) + " and " + ParseCondition(lChildren[1]) + ")";
                                break;
                            }
                        case "or":
                            {
                                newCon = "(" + ParseCondition(lChildren[0]) + " or " + ParseCondition(lChildren[1]) + ")";
                                break;
                            }
                        case "not":
                            {
                                ////lResult = lZ3Solver.NotOperator(ParseConstraint(lChildren[0])).ToString();
                                newCon = "(not " + ParseCondition(lChildren[0]) + ")";
                                break;
                            }
                        default:
                            break;
                    }
                }
                return newCon;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private bool ParseConditionVirtual(Node<string> pNode)
        {
            try
            {
                List<Node<string>> lChildren = new List<Node<string>>();
                if ((pNode.Data != "and") && (pNode.Data != "or") && (pNode.Data != "not"))
                {
                    //We have one operator
                    if (pNode.Data.StartsWith("Virtual"))
                    {
                        return false;
                    }
                    else
                        return true;
                }
                else
                {
                    foreach (Node<string> lChild in pNode.Children)
                    {
                        lChildren.Add(lChild);
                    }
                    switch (pNode.Data)
                    {
                        case "and":
                            {
                                return ParseConditionVirtual(lChildren[0]) && ParseConditionVirtual(lChildren[1]);

                            }
                        case "or":
                            {
                                return ParseConditionVirtual(lChildren[0]) && ParseConditionVirtual(lChildren[1]);
                            }
                        case "not":
                            {
                                ////lResult = lZ3Solver.NotOperator(ParseConstraint(lChildren[0])).ToString();
                                return ParseConditionVirtual(lChildren[0]);
                            }
                        default:
                            break;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private string printStatus(string p)
        {
            string[] condition = new string[3];

            condition = p.Split('_');

            if (condition.Length == 1)
                return p;
            else
                switch (condition[1])
                {
                    case "F":
                        return condition[0] + ".Finished";
                    case "I":
                        return condition[0] + ".Initial";
                    default:
                        return condition[0] + ".Executing";
                }


        }
    }
}
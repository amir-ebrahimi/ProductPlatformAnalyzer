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
            try
            {
                if (name.Contains("_"))
                {
                    String[] lOperationNameParts = name.Split('_');
                    if (string.Equals(lOperationNameParts[0],"Possible"))
                    {
                    
                        operation = "";
                        foreach (string str in lOperationNameParts)
                        {
                            operation = operation + str + " ";
                        }
                        opState = "possible";
                        variant = -1;
                        state = -1;
                    }
                    else if(string.Equals(lOperationNameParts[0],"Use"))
                    {

                        operation = "";
                        foreach (string str in lOperationNameParts)
                        {
                            operation = operation + str + " ";
                        }
                        opState = "use";
                        variant = -1;
                        state = -1;
                    }
                    else
                    {
                        operation = lOperationNameParts[0];
                        opState = lOperationNameParts[1];
                        variant = Convert.ToInt32(lOperationNameParts[2]);
                        state = Convert.ToInt32(lOperationNameParts[3]);
                    }
                }
                else
                {
                    operation = null;
                    opState = null;
                    variant = -1;
                    state = -1;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in setValue in OutputHandler");
                Console.WriteLine(ex.Message);
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
            try
            {
                if (!goalState(name, pState))
                {
                    OutputExp temp = new OutputExp(name, value);
                    outputResult.Add(temp);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in addExp in outputHandler");
                Console.WriteLine(ex.Message);
            }
        }

        public void SortAfterState()
        {
            try
            {
                outputResult.Sort(delegate(OutputExp exp1, OutputExp exp2)
                {
                    return exp1.state.CompareTo(exp2.state);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in sortAfterState in outputHandler");
                Console.WriteLine(ex.Message);
            }

        }

        public void SortAfterValue()
        {
            try
            {
                outputResult.Sort(delegate(OutputExp exp1, OutputExp exp2)
                {
                    return exp2.value.CompareTo(exp1.value);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in sortAfterValue in outputHander");
                Console.WriteLine(ex.Message);
            }

        }

        //Prints values for showing a finished analysis to console
        public void printFinished()
        {
            try
            {
                SortAfterValue();
                Console.WriteLine("\nVariants: ");
                printVariants();

                SortAfterState();
                Console.WriteLine("\nOperations in order: ");
                printOpTransformations();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in printFinished");
                Console.WriteLine(ex.Message);
            }
        }



        //Writes values for showing a finished analysis to HTML-file
        public void writeFinished()
        {
            try
            {
                StringWriter stringwriter = new StringWriter();
                HtmlTextWriter writer = new HtmlTextWriter(stringwriter);

                writeDocStart(writer);
                writeTabList(writer);

                writer.WriteFullBeginTag("div id=\"tabs-1\"");
                writeInput(writer);
                writer.WriteEndTag("div");

                writer.WriteFullBeginTag("div id=\"tabs-2\"");
                writer.WriteBeginTag("p class=\"resultHeading\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("Analysis result");
                writer.WriteEndTag("p");

                writer.WriteBeginTag("p class=\"discription\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("The analysis was successful, all operations can be perfomed in the presented order.");
                writer.WriteEndTag("p");

                writeChosenVariants(writer);
                writeTransitionTableState(writer);
                writeOpOrder(writer);
                //writeTransitionDiagram(writer);
                writeAvailableResources(writer);

                writer.WriteEndTag("div");
                writeDocEnd(writer);

                File.WriteAllText(path + "result.htm", stringwriter.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in writeFinished");
                Console.WriteLine(ex.Message);
            }

        }


        //Writes values for showing a finished analysis to HTML-file
        public void writeFinishedNoPost()
        {
            try
            {
                StringWriter stringwriter = new StringWriter();
                HtmlTextWriter writer = new HtmlTextWriter(stringwriter);

                writeDocStart(writer);
                writeTabList(writer);

                writer.WriteFullBeginTag("div id=\"tabs-1\"");
                writeInputNoPost(writer);
                writer.WriteEndTag("div");


                writer.WriteFullBeginTag("div id=\"tabs-2\"");
                writer.WriteBeginTag("p style=\"font-size:22px\"");
                writer.WriteBeginTag("p class=\"resultHeading\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("Analysis result");
                writer.WriteEndTag("p");

                writer.WriteBeginTag("p class=\"discription\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("The analysis was successful, all operations can be perfomed in the presented order.");
                writer.WriteEndTag("p");

                writeChosenVariants(writer);
                writeTransitionTableState(writer);
                writeOpOrder(writer);
                //writeTransitionDiagram(writer);
                writeAvailableResources(writer);

                writer.WriteEndTag("div");
                writeDocEnd(writer);

                File.WriteAllText(path + "resultNoPost.htm", stringwriter.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in writeFinishedNoPost");
                Console.WriteLine(ex.Message);
            }

        }



        public void writeInputFile()
        {
            try
            {
                StringWriter stringwriter = new StringWriter();
                HtmlTextWriter writer = new HtmlTextWriter(stringwriter);

                writeInput(writer);

                File.WriteAllText(path + "input.htm", stringwriter.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in writeInputFile");
                Console.WriteLine(ex.Message);
            }

        }

        private void writeInput(HtmlTextWriter writer)
        {
            try
            {
                writer.WriteBeginTag("p class=\"resultHeading\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("Input");
                writer.WriteEndTag("p");


                writer.WriteBeginTag("p id=\"inF\" class=\"title\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(" Feature Model");

                writer.WriteBeginTag("span id=\"titleFArr\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("&#x25BC");
                //upp &#x25B2
                writer.WriteEndTag("span");
                writer.WriteEndTag("p");

                writer.WriteBeginTag("div id=\"inFContent\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writeVariants(writer);

                writer.WriteBeginTag("p style=\"font-size:18px\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(" ");
                writer.WriteEndTag("p");


                writeConstraints(writer);
                writer.WriteEndTag("div");

                writeResourcesAndTraits(writer);

                writeOperationsWithPrePostCon(writer);
                writeVariantOperationMappings(writer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in writeInput");
                Console.WriteLine(ex.Message);
            }

        }


        private void writeInputNoPost(HtmlTextWriter writer)
        {
            try
            {
                writer.WriteBeginTag("p class=\"resultHeading\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("Analysis result");
                writer.WriteEndTag("p");


                writer.WriteBeginTag("p id=\"inF\" class=\"title\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(" Feature Model");

                writer.WriteBeginTag("span id=\"titleFArr\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("&#x25BC");
                //upp &#x25B2
                writer.WriteEndTag("span");
                writer.WriteEndTag("p");

                writer.WriteBeginTag("div id=\"inFContent\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writeVariants(writer);

                writer.WriteBeginTag("p style=\"font-size:18px\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(" ");
                writer.WriteEndTag("p");


                writeConstraints(writer);
                writer.WriteEndTag("div");

                writeResourcesAndTraits(writer);

                writeOperationsWithPreCon(writer);
                writeVariantOperationMappings(writer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in writeInputNoPost");
                Console.WriteLine(ex.Message);
            }

        }

        public void printCounterExample()
        {
            try
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

            catch (Exception ex)
            {
                Console.WriteLine("error in printCounterExample");
                Console.WriteLine(ex.Message);
            }
        }


        public void writeCounterExample()
        {
            StringWriter stringwriter = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(stringwriter);

            try
            {
                writeDocStart(writer);
                writeTabList(writer);

                writer.WriteFullBeginTag("div id=\"tabs-1\"");
                writeInput(writer);
                writer.WriteEndTag("div");


                writer.WriteFullBeginTag("div id=\"tabs-2\"");
                writer.WriteBeginTag("p class=\"resultHeading\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("Analysis result");
                writer.WriteEndTag("p");

                writer.WriteBeginTag("p class=\"discription\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("Counterexample found, all operations needed could not be performed.");
                writer.WriteEndTag("p");

                writeChosenVariants(writer);
                //writeOpStateTable(writer);
                writeTransitionTableState(writer);
                writeOpOrder(writer);
                //writeTransitionDiagram(writer);
                writeAvailableResources(writer);
                writeFalsePrePost(writer);

                writer.WriteEndTag("div");
                writeDocEnd(writer);

                File.WriteAllText(path + "counterEx.htm", stringwriter.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in writeCounterExample");
                Console.WriteLine(ex.Message);
            }

        }

        public void writeCounterExampleNoPost()
        {
            StringWriter stringwriter = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(stringwriter);

            try
            {
                writeDocStart(writer);
                writeTabList(writer);

                writer.WriteFullBeginTag("div id=\"tabs-1\"");
                writeInputNoPost(writer);
                writer.WriteEndTag("div");

                writer.WriteFullBeginTag("div id=\"tabs-2\"");
                
                writer.WriteBeginTag("p class=\"resultHeading\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("Analysis result");
                writer.WriteEndTag("p");

                writer.WriteBeginTag("p class=\"discription\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("Counterexample found, all operations needed could not be performed.");
                writer.WriteEndTag("p");

                writeChosenVariants(writer);
                //writeOpStateTable(writer);
                writeTransitionTableState(writer);
                writeOpOrder(writer);
                //writeTransitionDiagram(writer);
                writeAvailableResources(writer);
                writeFalsePre(writer);

                writer.WriteEndTag("div");
                writeDocEnd(writer);

                File.WriteAllText(path + "counterExNoPost.htm", stringwriter.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in writeCounterExampleNoPost");
                Console.WriteLine(ex.Message);
            }

        }

        public void writeDebugFile()
        {
            StringWriter stringwriter = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(stringwriter);
            SortAfterState();

            try
            {
                foreach (OutputExp exp in outputResult)
                {
                    writer.WriteBeginTag("p");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Write(exp);
                    writer.WriteEndTag("p");
                }

                File.WriteAllText(path + "debug.htm", stringwriter.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in writeDebugFile");
                Console.WriteLine(ex.Message);
            }

        }

        //Prints all output expressions
        public void Print()
        {
            try
            {
                foreach (OutputExp exp in outputResult)
                {
                    Console.WriteLine(exp.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in Print in outputHandler");
                Console.WriteLine(ex.Message);
            }
        }

        //Print all true outputexpressions
        public void printTrue()
        {
            try
            {
                foreach (OutputExp exp in outputResult)
                {
                    if (exp.value == "true")
                        Console.WriteLine(exp.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in printTrue in outputHandler");
                Console.WriteLine(ex.Message);
            }
        }

        //Print all variants
        public void printVariants()
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine("error in printVariants");
                Console.WriteLine(ex.Message);
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
                                + " $(\"#inOContent\").hide(); $(\"#inMContent\").hide(); $(\"#inRContent\").hide();"
                                + " $(\"#outConContent\").hide(); $(\"#outTTContent\").hide(); $(\"#outOSSContent\").hide();"
                                + " $(\"#outOOTContent\").hide(); $(\"#outCVContent\").hide(); $(\"#outARContent\").hide();"
                                + " $(\"#outOpOContent\").hide(); });</script></head><body>");
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

            writer.WriteBeginTag("p id=\"outOOT\" class=\"title\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Order of operation transitions");

            writer.WriteBeginTag("span id=\"titleOOTArr\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("&#x25BC");
            writer.WriteEndTag("span");
            writer.WriteEndTag("p");

            writer.WriteBeginTag("div id=\"outOOTContent\"");
            writer.Write(HtmlTextWriter.TagRightChar);


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
            writer.WriteEndTag("div");
        }

        //Returns all chosen variants
        public List<String> getChosenVariants()
        {
            List<String> vars = new List<String>();
            try
            {
                foreach (OutputExp exp in outputResult)
                {
                    if (exp.state == -1 && String.Equals(exp.value, "true") && !String.Equals(exp.opState, "possible") && !String.Equals(exp.opState, "use"))
                        vars.Add(exp.name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getChosenVariants in outputHandler");
                Console.WriteLine(ex.Message);
            }
            return vars;

        }

        //Returns all chosen variants
        public List<string[]> getChosenVariantsWithGroup()
        {
            string var, vg;
            string[] list;
            List<string[]> vars = new List<string[]>();

            try
            {
                foreach (OutputExp exp in outputResult)
                {
                    if (exp.state == -1 && String.Equals(exp.value, "true") && !String.Equals(exp.opState, "possible") && !String.Equals(exp.opState, "use"))
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getChosenVariantsWithGroup in outputHandler");
                Console.WriteLine(ex.Message);
            }
            return vars;
        }


        //Returns all chosen variant groups
        public List<string> getChosenVariantGroups()
        {
            string var, vg;
            List<string> vars = new List<string>();

            try
            {
                foreach (OutputExp exp in outputResult)
                {
                    if (exp.state == -1 && String.Equals(exp.value, "true") && !String.Equals(exp.opState, "possible") && !String.Equals(exp.opState, "use"))
                    {

                        var = exp.name;
                        vg = fwrapper.getVariantGroup(var);
                        if (!String.Equals(vg, "Virtual-VG") && !vars.Contains(vg))
                        {
                            vars.Add(vg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getChosenVariantGroups in outputHandler");
                Console.WriteLine(ex.Message);
            }
            return vars;
        }

        //Print all operation transformations
        public void printOpTransformations()
        {
            try
            {
                foreach (OutputExp exp in outputResult)
                {
                    OutputExp nextOp = findNextOp(exp);
                    if (nextOp != null)
                        if (String.Equals(exp.value, "true") && String.Equals(nextOp.value, "true"))
                            Console.WriteLine(exp.ToString() + " -> " + nextOp.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in printOpTransformations in outputHandler");
                Console.WriteLine(ex.Message);
            }
        }

        private void writeChosenVariants(HtmlTextWriter writer)
        {
            List<String[]> variants = getChosenVariantsWithGroup();
            List<String> groups = getChosenVariantGroups();


            writer.WriteBeginTag("p id=\"outCV\" class=\"title\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Chosen variants");

            writer.WriteBeginTag("span id=\"titleCVArr\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("&#x25BC");
            writer.WriteEndTag("span");
            writer.WriteEndTag("p");

            writer.WriteBeginTag("div id=\"outCVContent\"");
            writer.Write(HtmlTextWriter.TagRightChar);



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
            writer.WriteEndTag("div");
        }

        private List<OutputExp> getPossibleResources()
        {
            List<OutputExp> poss = new List<OutputExp>();

            try
            {
                foreach (OutputExp exp in outputResult)
                {
                    if (String.Equals(exp.opState, "possible"))
                        poss.Add(exp);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getPossibleResources in outputHandler");
                Console.WriteLine(ex.Message);
            }

            return poss;
        }

        private void writeAvailableResources(HtmlTextWriter writer)
        {
            SortAfterValue();

            List<OutputExp> possibleRes = getPossibleResources();

            writer.WriteBeginTag("p id=\"outAR\" class=\"title\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Available resources");

            writer.WriteBeginTag("span id=\"titleARArr\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("&#x25BC");
            writer.WriteEndTag("span");
            writer.WriteEndTag("p");

            writer.WriteBeginTag("div id=\"outARContent\"");
            writer.Write(HtmlTextWriter.TagRightChar);

            if (possibleRes.Count != 0)
            {
                writer.WriteBeginTag("table style=\"margin-left:40px\"");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.WriteBeginTag("tr");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.WriteBeginTag("th");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("Resource");
                writer.WriteEndTag("th");

                writer.WriteBeginTag("th");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("Available");
                writer.WriteEndTag("th");
                writer.WriteEndTag("tr");

                foreach (OutputExp res in possibleRes)
                {
                    writer.WriteBeginTag("tr");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.WriteBeginTag("td");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Write(res.operation);
                    writer.WriteEndTag("td");

                    writer.WriteBeginTag("td");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Write(res.value);
                    writer.WriteEndTag("td");

                    writer.WriteEndTag("tr");
                }
                writer.WriteEndTag("table");
            }
            else
            {

                writer.WriteBeginTag("p class=\"empty\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("No available resources.");
                writer.WriteEndTag("p");

            }

            writer.WriteEndTag("div");
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

            if (constraints.Count != 0)
            {
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
                        writer.Write(GeneralUtilities.parseExpression(con, "infix"));
                        writer.WriteEndTag("td");
                        writer.WriteEndTag("tr");
                    }
                }
                writer.WriteEndTag("table");
            }
            else
            {

                writer.WriteBeginTag("p class=\"empty\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("No constraints were specified.");
                writer.WriteEndTag("p");

            }
        }

        private void writeOperationsWithPrePostCon(HtmlTextWriter writer)
        {
            List<operation> operations = new List<operation>(fwrapper.OperationList);

            writer.WriteBeginTag("p id=\"inO\" class=\"title\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write(" Operations");

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

            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.Write("Requirements");

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
                    if (!pre.Contains("Possible"))
                    {
                        writer.WriteBeginTag("li");
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Write(GeneralUtilities.parseExpression(pre, "infix"));
                        writer.WriteEndTag("li");
                    }
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
                    writer.Write(GeneralUtilities.parseExpression(post, "infix"));
                    writer.WriteEndTag("li");
                }
                writer.WriteEndTag("ul");

                writer.WriteEndTag("td");

                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.WriteBeginTag("ul style=\"list-style-type:none\"");
                writer.Write(HtmlTextWriter.TagRightChar);

                foreach (string req in op.requirements)
                {
                    writer.WriteBeginTag("li");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Write(GeneralUtilities.parseExpression(req, "infix"));
                    writer.WriteEndTag("li");
                }
                writer.WriteEndTag("ul");

                writer.WriteEndTag("td");

                writer.WriteEndTag("tr");
            }

            writer.WriteEndTag("table");
            writer.WriteEndTag("div");

        }


        private void writeOperationsWithPreCon(HtmlTextWriter writer)
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

            writer.Write("Requirements");

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
                    writer.Write(GeneralUtilities.parseExpression(pre, "infix"));
                    writer.WriteEndTag("li");
                }
                writer.WriteEndTag("ul");

                writer.WriteEndTag("td");

                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.WriteBeginTag("ul style=\"list-style-type:none\"");
                writer.Write(HtmlTextWriter.TagRightChar);

                foreach (string req in op.requirements)
                {
                    writer.WriteBeginTag("li");
                    writer.Write(HtmlTextWriter.TagRightChar);
                    writer.Write(req);
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
            writer.Write(" Variant operation mappings");

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

                writer.Write(replaceVirtual(vop.getVariantExpr()));

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

        private void writeResourcesAndTraits(HtmlTextWriter writer)
        {

            writer.WriteBeginTag("p id=\"inR\" class=\"title\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Traits and resources");

            writer.WriteBeginTag("span id=\"titleRArr\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("&#x25BC");
            writer.WriteEndTag("span");
            writer.WriteEndTag("p");

            writer.WriteBeginTag("div id=\"inRContent\"");
            writer.Write(HtmlTextWriter.TagRightChar);

            writeTraits(writer);

            writer.WriteBeginTag("p style=\"font-size:18px\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write(" ");
            writer.WriteEndTag("p");


            writeResources(writer);

            writer.WriteEndTag("div");

        }

        private void writeTraits(HtmlTextWriter writer)
        {

            List<trait> traits = new List<trait>(fwrapper.TraitList);

            if (traits.Count != 0)
            {
                writer.WriteBeginTag("table  style=\"margin-left:40px\"");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.WriteBeginTag("tr");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.WriteBeginTag("th");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.Write("Trait");

                writer.WriteEndTag("th");


                writer.WriteBeginTag("th");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.Write("Inherit");

                writer.WriteEndTag("th");


                writer.WriteBeginTag("th");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.Write("Attributes");

                writer.WriteEndTag("th");
                writer.WriteEndTag("tr");


                foreach (trait tra in traits)
                {

                    writer.WriteBeginTag("tr");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    writer.WriteBeginTag("td");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    writer.Write(tra.names);

                    writer.WriteEndTag("td");

                    writer.WriteBeginTag("td");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    writer.WriteBeginTag("ul style=\"list-style-type:none\"");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    foreach (trait inh in tra.inherit)
                    {
                        writer.WriteBeginTag("li");
                        writer.Write(HtmlTextWriter.TagRightChar);

                        writer.Write(inh.names);

                        writer.WriteEndTag("li");

                    }

                    writer.WriteEndTag("ul");
                    writer.WriteEndTag("td");

                    writer.WriteBeginTag("td");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    writer.WriteBeginTag("ul style=\"list-style-type:none\"");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    foreach (Tuple<string, string> att in tra.attributes)
                    {
                        writer.WriteBeginTag("li");
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Write(att.Item1 + ": " + att.Item2);
                        writer.WriteEndTag("li");
                    }
                    writer.WriteEndTag("ul");

                    writer.WriteEndTag("td");

                    writer.WriteEndTag("tr");
                }

                writer.WriteEndTag("table");
            }
            else
            {

                writer.WriteBeginTag("p class=\"empty\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("No Traits were specified.");
                writer.WriteEndTag("p");

            }
        }

        private void writeResources(HtmlTextWriter writer)
        {

            List<resource> resources = new List<resource>(fwrapper.ResourceList);

            if (resources.Count != 0)
            {
                writer.WriteBeginTag("table  style=\"margin-left:40px\"");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.WriteBeginTag("tr");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.WriteBeginTag("th");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.Write("Resource");

                writer.WriteEndTag("th");


                writer.WriteBeginTag("th");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.Write("Of traits");

                writer.WriteEndTag("th");


                writer.WriteBeginTag("th");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.Write("Attributes");

                writer.WriteEndTag("th");

                writer.WriteEndTag("tr");


                foreach (resource res in resources)
                {

                    writer.WriteBeginTag("tr");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    writer.WriteBeginTag("td");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    writer.Write(res.names);

                    writer.WriteEndTag("td");

                    writer.WriteBeginTag("td");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    writer.WriteBeginTag("ul style=\"list-style-type:none\"");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    foreach (trait tra in res.traits)
                    {
                        writer.WriteBeginTag("li");
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Write(tra.names);
                        writer.WriteEndTag("li");
                    }
                    writer.WriteEndTag("ul");

                    writer.WriteEndTag("td");

                    writer.WriteBeginTag("td");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    writer.WriteBeginTag("ul style=\"list-style-type:none\"");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    foreach (Tuple<string, string, string> att in res.attributes)
                    {
                        writer.WriteBeginTag("li");
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Write(att.Item1 + " = " + att.Item3);
                        writer.WriteEndTag("li");
                    }
                    writer.WriteEndTag("ul");

                    writer.WriteEndTag("td");

                    writer.WriteEndTag("tr");
                }

                writer.WriteEndTag("table");
            }
            else
            {

                writer.WriteBeginTag("p class=\"empty\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("No Resources were specified.");
                writer.WriteEndTag("p");

            }
        }


        private void writeFalsePre(HtmlTextWriter writer)
        {
            List<String[]> conditions = getConditionsStateWithValues(getLastState());

            if (conditions.Count != 0)
            {
                writer.WriteBeginTag("p id=\"outCon\" class=\"title\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("False preconditions in last state");

                writer.WriteBeginTag("span id=\"titleConArr\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("&#x25BC");
                writer.WriteEndTag("span");
                writer.WriteEndTag("p");

                writer.WriteBeginTag("div id=\"outConContent\"");
                writer.Write(HtmlTextWriter.TagRightChar);


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
                    if (!con[0].Contains("Post"))
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
                }
                writer.WriteEndTag("table");
                writer.WriteEndTag("div");
            }
            else
            {

                writer.WriteBeginTag("p class=\"empty\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("No false condition in last state.");
                writer.WriteEndTag("p");

            }
        }


        private void writeFalsePrePost(HtmlTextWriter writer)
        {
            List<String[]> conditions = getConditionsStateWithValues(getLastState());

            if (conditions.Count != 0)
            {
                writer.WriteBeginTag("p id=\"outCon\" class=\"title\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("False post/preconditions in last state");

                writer.WriteBeginTag("span id=\"titleConArr\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("&#x25BC");
                writer.WriteEndTag("span");
                writer.WriteEndTag("p");

                writer.WriteBeginTag("div id=\"outConContent\"");
                writer.Write(HtmlTextWriter.TagRightChar);


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
                writer.WriteEndTag("div");
            }
            else
            {

                writer.WriteBeginTag("p class=\"empty\"");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write("No false conditions in last state.");
                writer.WriteEndTag("p");

            }
        }

        private void writeOpOrderWithArrow(HtmlTextWriter writer)
        {
            Boolean first = true;

            writer.WriteBeginTag("p id=\"outOpO\" class=\"title\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Sequence of finished operation");

            writer.WriteBeginTag("span id=\"titleOpOArr\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("&#x25BC");
            writer.WriteEndTag("span");
            writer.WriteEndTag("p");

            writer.WriteBeginTag("div id=\"outOpOContent\"");
            writer.Write(HtmlTextWriter.TagRightChar);

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
            writer.WriteEndTag("div");
        }


        private void writeOpOrder(HtmlTextWriter writer)
        {
            int count = 1;

            writer.WriteBeginTag("p id=\"outOpO\" class=\"title\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Sequence of finished operation");

            writer.WriteBeginTag("span id=\"titleOpOArr\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("&#x25BC");
            writer.WriteEndTag("span");
            writer.WriteEndTag("p");

            writer.WriteBeginTag("div id=\"outOpOContent\"");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.WriteBeginTag("table style=\"margin-left:40px\"");
            writer.Write(HtmlTextWriter.TagRightChar);


            writer.WriteBeginTag("tr ");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteBeginTag("th ");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Operation");
            writer.WriteEndTag("th");
            writer.WriteEndTag("tr");


            foreach (OutputExp exp in outputResult)
            {
                OutputExp nextOp = findNextOp(exp);
                if (nextOp != null)
                    if (String.Equals(exp.value, "true") && String.Equals(nextOp.value, "true") && String.Equals(exp.opState, "E"))
                    {

                        writer.WriteBeginTag("tr ");
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.WriteBeginTag("td ");
                        writer.Write(HtmlTextWriter.TagRightChar);

                        writer.Write((count++) + ". " + exp.operation);
                        writer.WriteEndTag("td");
                        writer.WriteEndTag("tr");
                    }
            }

            writer.WriteEndTag("table");
            writer.WriteEndTag("div");
        }

        private void writeOpStateTable(HtmlTextWriter writer)
        {
            SortAfterValue();
            List<String[]> operationState = getOpState(getLastState());
            String[] transF;
            String[] transI;

            writer.WriteBeginTag("p id=\"outOpLast\" class=\"title\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Operation statuses in last state");

            writer.WriteBeginTag("span id=\"titleOpLastArr\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("&#x25BC");
            writer.WriteEndTag("span");
            writer.WriteEndTag("p");

            writer.WriteBeginTag("div id=\"inOpLastContent\"");
            writer.Write(HtmlTextWriter.TagRightChar);


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
            writer.WriteEndTag("div");
        }

        private void writeTransitionTableStatus(HtmlTextWriter writer)
        {
            SortAfterState();
            String[] transPair;
            List<String[]> transformations = getOpTransformations();

            writer.WriteBeginTag("p id=\"outTT\" class=\"title\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Operations progressing at state");

            writer.WriteBeginTag("span id=\"titleTTArr\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("&#x25BC");
            writer.WriteEndTag("span");
            writer.WriteEndTag("p");

            writer.WriteBeginTag("div id=\"outTTContent\"");
            writer.Write(HtmlTextWriter.TagRightChar);



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
            writer.WriteEndTag("div");

        }


        private void writeTransitionTableState(HtmlTextWriter writer)
        {
            //List<String> activeOps = getActiveOps();
            List<OutputExp> allOps = getAllOps();
            List<String[]> opTransitions;
            int lastState = getLastState();

            writer.WriteBeginTag("p id=\"outOSS\" class=\"title\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("Operation statuses in different states");

            writer.WriteBeginTag("span id=\"titleOSSArr\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write("&#x25BC");
            writer.WriteEndTag("span");
            writer.WriteEndTag("p");

            writer.WriteBeginTag("div id=\"outOSSContent\"");
            writer.Write(HtmlTextWriter.TagRightChar);


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

            foreach (OutputExp op in allOps)
            {
                opTransitions = getOpTransformations(op);

                writer.WriteBeginTag("tr");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);
                writer.Write(op.operation);
                writer.WriteEndTag("td");

                for (int i = 0; i <= lastState + 1; i++)
                {
                    string transString = TransitionStateAt(opTransitions, i, op);
                    if (!transString.Contains("F") && i == lastState + 1 && !transString.Contains("U"))
                        writer.WriteBeginTag("td class=\"false\"");
                    else
                        writer.WriteBeginTag("td");
                    writer.Write(HtmlTextWriter.TagRightChar);

                    writer.Write(transString);
                    writer.WriteEndTag("td");

                }

                writer.WriteEndTag("tr");

            }
            writer.WriteEndTag("table");
            writer.WriteEndTag("div");
        }


        //Returns all operation transformations
        private List<String[]> getOpTransformations()
        {
            List<String[]> transforms = new List<String[]>();
            String[] item = new String[4];
            int lastState = getLastState();

            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getOpTransformations in outputHandler");
                Console.WriteLine(ex.Message);
            }

            return transforms;
        }


        private string TransitionStateAt(List<String[]> transformations, int state, OutputExp exp)
        {
            string currentState = exp.opState;

            try
            {
                foreach (String[] trans in transformations)
                {
                    int transState = Convert.ToInt32(trans[1]);
                    if (transState <= state)
                    {
                        if (String.Equals(trans[2], "F"))

                            if (state == transState)
                            {
                                return "<b> " + trans[2] + "</b>";
                            }
                            else
                                return trans[2];
                        else
                        {
                            currentState = trans[2];
                        }

                        if (state == transState)
                        {
                            currentState = "<b> " + currentState + "</b>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in TransitionStateAt in outputHandler");
                Console.WriteLine(ex.Message);
            }
            return currentState;
        }


        //Returns all operation transformations
        private List<String[]> getOpTransformations(OutputExp operation)
        {
            List<String[]> transforms = new List<String[]>();
            String[] item = new String[4];
            int lastState = getLastState();

            try
            {
                foreach (OutputExp exp in outputResult)
                {
                    if (String.Equals(exp.operation, operation.operation) &&
                        String.Equals(exp.variant, operation.variant))
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getOpTransformations in outputHandler");
                Console.WriteLine(ex.Message);
            }

            return transforms;
        }

        //Print all operation transformations up to a state
        public void printOpTransformations(int max)
        {
            try
            {
                foreach (OutputExp exp in outputResult)
                {
                    OutputExp nextOp = findNextOp(exp);
                    if (nextOp != null && nextOp.state <= max)
                        if (String.Equals(exp.value, "true") && String.Equals(nextOp.value, "true"))
                            Console.WriteLine(exp.ToString() + " -> " + nextOp.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in printOpTransformations in outputHandler");
                Console.WriteLine(ex.Message);
            }
        }

        //Returns true is name is representing a goal selection varable
        private bool goalState(string name, int pState)
        {
            try
            {
                for (int i = 0; i <= pState; i++)
                {
                    if (String.Equals(name, ("P" + i)))
                        return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in goalState in outputHandler");
                Console.WriteLine(ex.Message);
            }
            return false;

        }

        //Returns operation in next state and with the following operation status
        private OutputExp findNextOp(OutputExp first)
        {
            OutputExp next = null;

            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in findNextOp in outputHandler");
                Console.WriteLine(ex.Message);
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
            try
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
            catch (Exception ex)
            {
                Console.WriteLine("error in printConditionState in outputHandler");
                Console.WriteLine(ex.Message);
            }
        }

        //Returns false pre and post conditions for lstate
        private List<String> getConditionsState(int lstate)
        {
            List<String> con = new List<String>();

            try
            {
                foreach (OutputExp exp in outputResult)
                {
                    if (exp.state == lstate &&
                        (String.Equals(exp.opState, "PostCondition") ||
                        (String.Equals(exp.opState, "PreCondition"))) &&
                        String.Equals(exp.value, "false"))
                        con.Add(exp.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getConditionState in outputHandler");
                Console.WriteLine(ex.Message);
            }
            return con;
        }

        //Returns false pre and post conditions for lstate with the pre/post condition
        private List<String[]> getConditionsStateWithValues(int lstate)
        {
            List<String[]> conditions = new List<String[]>();
            List<String> conValue = new List<String>();
            string[] list;

            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getConditionStateWithValues in outputHandler");
                Console.WriteLine(ex.Message);
            }
            return conditions;
        }

        //Prints operations in lstate
        private void printOpState(int lstate)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine("error in printOpState in outputHandler");
                Console.WriteLine(ex.Message);
            }
        }

        //Returns operations inlstate
        private List<String[]> getOpState(int lstate)
        {
            List<String[]> ops = new List<String[]>();
            String[] item;

            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getOpState in outputHandler");
                Console.WriteLine(ex.Message);
            }
            return ops;
        }

        private string consToString(List<string> pcons)
        {
            string exp = GeneralUtilities.parseExpression(pcons[0], "infix");

            try
            {
                pcons.RemoveAt(0);
                foreach (string con in pcons)
                {
                    exp = exp + "and" + GeneralUtilities.parseExpression(con, "infix");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in conToString in outputHandler");
                Console.WriteLine(ex.Message);
            }

            return exp;
        }

        //Returns operations inlstate
        private List<String> getActiveOps()
        {
            List<String> ops = new List<String>();

            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getActiveOps in outputHandler");
                Console.WriteLine(ex.Message);
            }
            return ops;
        }

        //Returns all operations
        private List<OutputExp> getAllOps()
        {
            List<OutputExp> ops = new List<OutputExp>();

            try
            {
                foreach (OutputExp exp in outputResult)
                {
                    if (exp.state == 0 &&
                       String.Equals(exp.value, "true") &&
                       !String.Equals(exp.opState, "E") &&
                       !String.Equals(exp.opState, "F") &&
                       !String.Equals(exp.opState, "PostCondition") &&
                       !String.Equals(exp.opState, "PreCondition"))
                    {
                        ops.Add(exp);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getActiveOps in outputHandler");
                Console.WriteLine(ex.Message);
            }
            return ops;
        }

        //Returns the last state of analysis
        private int getLastState()
        {
            int lastState = 0;

            try
            {
                foreach (OutputExp exp in outputResult)
                {
                    if (exp.state > lastState)
                    {
                        lastState = exp.state;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in getLastState in outputHandler");
                Console.WriteLine(ex.Message);
            }
            return lastState - 1;
        }

        private string replaceVirtual(string p)
        {
            string newP = "";

            try
            {
                if (p.StartsWith("Virtual"))
                {
                    /* RUNA code
                    virtualConnection con = fwrapper.findVirtualConnectionWithName(p);
                    foreach (variant var in con.getVariants())
                    {
                        newP = newP + var.names + "<br>";
                    }
                     */
                    variant virtualVariant = fwrapper.findVirtualVariant(p);
                    newP = virtualVariant.names + "<br>";
                    return newP;
                }
                    

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in replaceVirtual in outputHandler");
                Console.WriteLine(ex.Message);
            }
            return p;

        }


        /*private string parseInfix(string pPrefixExpr)
        {
            string newInfixExpr = null;

            try
            {
                //For each condition first we have to build its coresponding tree
                Parser lConditionParser = new Parser();
                Node<string> lExprTree = new Node<string>("root");

                lConditionParser.AddChild(lExprTree, pPrefixExpr);

                foreach (Node<string> item in lExprTree)
                {
                    //Then we have to traverse the tree
                    newInfixExpr = ParseExpression(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in modCondition in outputHandler");
                Console.WriteLine(ex.Message);
            }

            return newInfixExpr;
        }*/

        private bool checkCondition(string con)
        {
            //For each condition first we have to build its coresponding tree
            Parser lConditionParser = new Parser();
            Node<string> lCnstExprTree = new Node<string>("root");

            try
            {
                lConditionParser.AddChild(lCnstExprTree, con);

                foreach (Node<string> item in lCnstExprTree)
                {
                    //Then we have to traverse the tree
                    return ParseConditionVirtual(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in checkCondition in outputHandler");
                Console.WriteLine(ex.Message);
            }

            return true;
        }


        /*private string ParseExpression(Node<string> pNode)
        {
            try
            {
                List<Node<string>> lChildren = new List<Node<string>>();
                string newCon = null;
                if ((pNode.Data != "and") 
                    && (pNode.Data != "or") 
                    && (pNode.Data != "not")
                    && (pNode.Data != "<=")
                    && (pNode.Data != ">=")
                    && (pNode.Data != "<")
                    && (pNode.Data != ">")
                    )
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
                                newCon = "(" + ParseExpression(lChildren[0]) + " and " + ParseExpression(lChildren[1]) + ")";
                                break;
                            }
                        case "or":
                            {
                                newCon = "(" + ParseExpression(lChildren[0]) + " or " + ParseExpression(lChildren[1]) + ")";
                                break;
                            }
                        case "<=":
                            {
                                newCon = "(" + ParseExpression(lChildren[0]) + " <= " + ParseExpression(lChildren[1]) + ")";
                                break;
                            }
                        case ">=":
                            {
                                newCon = "(" + ParseExpression(lChildren[0]) + " >= " + ParseExpression(lChildren[1]) + ")";
                                break;
                            }
                        case ">":
                            {
                                newCon = "(" + ParseExpression(lChildren[0]) + " > " + ParseExpression(lChildren[1]) + ")";
                                break;
                            }
                        case "<":
                            {
                                newCon = "(" + ParseExpression(lChildren[0]) + " < " + ParseExpression(lChildren[1]) + ")";
                                break;
                            }
                        case "not":
                            {
                                ////lResult = lZ3Solver.NotOperator(ParseConstraint(lChildren[0])).ToString();
                                newCon = "(not " + ParseExpression(lChildren[0]) + ")";
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
                Console.WriteLine("error in ParseExpression in outputHandler");
                Console.WriteLine(ex.Message);
                throw ex;
            }

        }*/


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
                Console.WriteLine("error in replaceVirtual in outputHandler");
                Console.WriteLine(ex.Message);

                throw ex;
            }

        }

        /*private string printStatus(string p)
        {
            string[] condition = new string[3];

            try
            {

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
            catch (Exception ex)
            {
                Console.WriteLine("error in printStatus in outputHandler");
                Console.WriteLine(ex.Message);
            }
            return null;
        }*/
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Z3;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace ProductPlatformAnalyzer
{
    //Helping class for storing ouput expressions
    public partial class OutputExp
    {
        public int State { get; set; }
        public string Name { get; set; }
        public string Operation { get; set; }
        public string Value { get; set; }
        public string OpState { get; set; }
        public string Variant { get; set; }

        public OutputExp(string pName, string pValue)
        {
            Name = pName;
            Value = pValue;
            SetValues(pName);
        }

        public override string ToString()
        {
            return Name + " = " + Value;
        }

        private void SetValues(string pName)
        {
            try
            {
                if (pName.Contains("_"))
                {
                    String[] lOperationNameParts = pName.Split('_');
                    if (string.Equals(lOperationNameParts[0],"Possible"))
                    {
                    
                        Operation = "";
                        foreach (string lStr in lOperationNameParts)
                        {
                            Operation = Operation + lStr + " ";
                        }
                        OpState = "possible";
                        //Variant= -1;
                        State = -1;
                    }
                    else if(string.Equals(lOperationNameParts[0],"Use"))
                    {

                        Operation = "";
                        foreach (string str in lOperationNameParts)
                        {
                            Operation = Operation + str + " ";
                        }
                        OpState = "use";
                        State = -1;
                    }
                    else if (string.Equals(lOperationNameParts[1],"Trigger"))
                    {
                        Operation = lOperationNameParts[0];
                        OpState = "trigger";
                        State = -1;
                    }
                    else if(string.Equals(lOperationNameParts[1],"Precondition"))
                    {
                        Operation = lOperationNameParts[0];
                        OpState = "precondition";
                        State = Convert.ToInt32(lOperationNameParts[2]);
                    }
                    else
                    {
                        Operation = lOperationNameParts[0];
                        if (lOperationNameParts[1] != "E2F" && lOperationNameParts[1] != "I2E")
                        {
                            OpState = lOperationNameParts[1];
                            State = Convert.ToInt32(lOperationNameParts[2]);
                        }
                        else
                        {
                            OpState = null;
                            State = -1;
                        }
                    }
                }
                else if (pName.StartsWith("V"))
                {
                    Variant = pName;
                    Operation = null;
                    OpState = null;
                    State = -1;
                }
                else
                {
                    Operation = null;
                    OpState = null;
                    State = -1;
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
        private List<OutputExp> OutputResult;
        private string Path = "../../Output/";
        private FrameworkWrapper FrameworkWrapper;
        private bool EnableUserMessages;

        public FrameworkWrapper GetFrameworkWrapper()
        {
            return FrameworkWrapper;
        }

        public void SetFrameworkWrapper(FrameworkWrapper pFrameworkWrapper)
        {
            FrameworkWrapper = pFrameworkWrapper;
        }

        public void SetOutputResult(List<OutputExp> pOutputExp)
        {
            OutputResult = pOutputExp;
        }

        public bool GetEnableUserMessages()
        {
            return EnableUserMessages;
        }

        public void SetEnableUserMessages(bool pEnableUserMessages)
        {
            EnableUserMessages = pEnableUserMessages;
        }

        public OutputHandler()
        {
            ResetOutputResult();
        }

        public void ResetOutputResult()
        {
            OutputResult = new List<OutputExp>();
        }

        public void AddExp(string pName, string pValue, string pState)
        {
            try
            {
                if (!GoalState(pName, pState))
                {
                    OutputExp lTemp = new OutputExp(pName, pValue);
                    OutputResult.Add(lTemp);
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in addExp in outputHandler");
                PrintMessageToConsole(ex.Message);
            }
        }

        public void SortAfterState()
        {
            try
            {
                OutputResult.Sort(delegate(OutputExp lExp1, OutputExp lExp2)
                {
                    return lExp1.State.CompareTo(lExp2.State);
                });
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in sortAfterState in outputHandler");
                PrintMessageToConsole(ex.Message);
            }

        }

        public void SortAfterValue()
        {
            try
            {
                OutputResult.Sort(delegate(OutputExp lExp1, OutputExp lExp2)
                {
                    return lExp2.Value.CompareTo(lExp1.Value);
                });
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in sortAfterValue in outputHander");
                PrintMessageToConsole(ex.Message);
            }

        }

        /// <summary>
        /// Prints chosen operation transitions for showing a finished analysis to console
        /// </summary>
        public void PrintOperationsTransitions()
        {
            try
            {
                SortAfterState();
                PrintMessageToConsole("\nOperations in order: ");
                PrintOpTransformations();
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in printOperationTransitions");
                PrintMessageToConsole(ex.Message);
            }
        }

        /// <summary>
        /// Prints chosen Variantvalues for showing a finished analysis to console
        /// </summary>
        public void PrintChosenVariants()
        {
            try
            {
                SortAfterValue();
                PrintMessageToConsole("\nVariants: ");
                PrintVariantsNParts();
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in printChosenVariants");
                PrintMessageToConsole(ex.Message);
            }
        }


        //Writes values for showing a finished analysis to HTML-file
        public void WriteFinished()
        {
            try
            {
                StringWriter lStringwriter = new StringWriter();
                HtmlTextWriter lWriter = new HtmlTextWriter(lStringwriter);

                EriteDocStart(lWriter);
                WriteTabList(lWriter);

                lWriter.WriteFullBeginTag("div id=\"tabs-1\"");
                WriteInput(lWriter);
                lWriter.WriteEndTag("div");

                lWriter.WriteFullBeginTag("div id=\"tabs-2\"");
                lWriter.WriteBeginTag("p class=\"resultHeading\"");
                lWriter.Write(HtmlTextWriter.TagRightChar);
                lWriter.Write("Analysis result");
                lWriter.WriteEndTag("p");

                lWriter.WriteBeginTag("p class=\"discription\"");
                lWriter.Write(HtmlTextWriter.TagRightChar);
                lWriter.Write("The analysis was successful, all operations can be perfomed in the presented order.");
                lWriter.WriteEndTag("p");

                WriteChosenVariants(lWriter);
                WriteTransitionTableState(lWriter);
                WriteOpOrder(lWriter);
                //writeTransitionDiagram(writer);
                WriteAvailableResources(lWriter);

                lWriter.WriteEndTag("div");
                WriteDocEnd(lWriter);

                File.WriteAllText(Path + "result.htm", lStringwriter.ToString());
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in writeFinished");
                PrintMessageToConsole(ex.Message);
            }

        }


        //Writes values for showing a finished analysis to HTML-file
        public void WriteFinishedNoPost()
        {
            try
            {
                StringWriter lStringwriter = new StringWriter();
                HtmlTextWriter lWriter = new HtmlTextWriter(lStringwriter);

                EriteDocStart(lWriter);
                WriteTabList(lWriter);

                lWriter.WriteFullBeginTag("div id=\"tabs-1\"");
                WriteInputNoPost(lWriter);
                lWriter.WriteEndTag("div");


                lWriter.WriteFullBeginTag("div id=\"tabs-2\"");
                lWriter.WriteBeginTag("p style=\"font-size:22px\"");
                lWriter.WriteBeginTag("p class=\"resultHeading\"");
                lWriter.Write(HtmlTextWriter.TagRightChar);
                lWriter.Write("Analysis result");
                lWriter.WriteEndTag("p");

                lWriter.WriteBeginTag("p class=\"discription\"");
                lWriter.Write(HtmlTextWriter.TagRightChar);
                lWriter.Write("The analysis was successful, all operations can be perfomed in the presented order.");
                lWriter.WriteEndTag("p");

                WriteChosenVariants(lWriter);
                WriteTransitionTableState(lWriter);
                WriteOpOrder(lWriter);
                //writeTransitionDiagram(writer);
                WriteAvailableResources(lWriter);

                lWriter.WriteEndTag("div");
                WriteDocEnd(lWriter);

                File.WriteAllText(Path + "resultNoPost.htm", lStringwriter.ToString());
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in writeFinishedNoPost");
                PrintMessageToConsole(ex.Message);
            }

        }



        public void WriteInputFile()
        {
            try
            {
                StringWriter lWtringwriter = new StringWriter();
                HtmlTextWriter lWriter = new HtmlTextWriter(lWtringwriter);

                WriteInput(lWriter);

                File.WriteAllText(Path + "input.htm", lWtringwriter.ToString());
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in writeInputFile");
                PrintMessageToConsole(ex.Message);
            }

        }

        private void WriteInput(HtmlTextWriter pWriter)
        {
            try
            {
                pWriter.WriteBeginTag("p class=\"resultHeading\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("Input");
                pWriter.WriteEndTag("p");


                pWriter.WriteBeginTag("p id=\"inF\" class=\"title\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write(" Feature Model");

                pWriter.WriteBeginTag("span id=\"titleFArr\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("&#x25BC");
                //upp &#x25B2
                pWriter.WriteEndTag("span");
                pWriter.WriteEndTag("p");

                pWriter.WriteBeginTag("div id=\"inFContent\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                WriteVariants(pWriter);

                pWriter.WriteBeginTag("p style=\"font-size:18px\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write(" ");
                pWriter.WriteEndTag("p");


                WriteConstraints(pWriter);
                pWriter.WriteEndTag("div");

                WriteResourcesAndTraits(pWriter);

                WriteOperationsWithPrePostCon(pWriter);
                //writeVariantOperationMappings(writer);
                //writePartOperationMappings(writer);
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in writeInput");
                PrintMessageToConsole(ex.Message);
            }

        }


        private void WriteInputNoPost(HtmlTextWriter pWriter)
        {
            try
            {
                pWriter.WriteBeginTag("p class=\"resultHeading\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("Analysis result");
                pWriter.WriteEndTag("p");


                pWriter.WriteBeginTag("p id=\"inF\" class=\"title\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write(" Feature Model");

                pWriter.WriteBeginTag("span id=\"titleFArr\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("&#x25BC");
                //upp &#x25B2
                pWriter.WriteEndTag("span");
                pWriter.WriteEndTag("p");

                pWriter.WriteBeginTag("div id=\"inFContent\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                WriteVariants(pWriter);

                pWriter.WriteBeginTag("p style=\"font-size:18px\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write(" ");
                pWriter.WriteEndTag("p");


                WriteConstraints(pWriter);
                pWriter.WriteEndTag("div");

                WriteResourcesAndTraits(pWriter);

                WriteOperationsWithPreCon(pWriter);
                //writePartOperationMappings(writer);
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in writeInputNoPost");
                PrintMessageToConsole(ex.Message);
            }

        }

        public void PrintCounterExample()
        {
            try
            {
                int lLastState = GetLastState();

                SortAfterValue();
                PrintMessageToConsole("\nVariants:");
                PrintVariantsNParts();

                PrintMessageToConsole("\nOperation in last state:");
                PrintOpState(lLastState);

                SortAfterState();
                PrintMessageToConsole("\nOperations in order: ");
                PrintOpTransformations(lLastState);

                PrintMessageToConsole("\nFalse pre/post-conditions:");
                PrintConditionsState(lLastState);
            }

            catch (Exception ex)
            {
                PrintMessageToConsole("error in printCounterExample");
                PrintMessageToConsole(ex.Message);
            }
        }


        public void WriteCounterExample()
        {
            StringWriter lStringwriter = new StringWriter();
            HtmlTextWriter lWriter = new HtmlTextWriter(lStringwriter);

            try
            {
                EriteDocStart(lWriter);
                WriteTabList(lWriter);

                lWriter.WriteFullBeginTag("div id=\"tabs-1\"");
                WriteInput(lWriter);
                lWriter.WriteEndTag("div");


                lWriter.WriteFullBeginTag("div id=\"tabs-2\"");
                lWriter.WriteBeginTag("p class=\"resultHeading\"");
                lWriter.Write(HtmlTextWriter.TagRightChar);
                lWriter.Write("Analysis result");
                lWriter.WriteEndTag("p");

                lWriter.WriteBeginTag("p class=\"discription\"");
                lWriter.Write(HtmlTextWriter.TagRightChar);
                lWriter.Write("Counterexample found, all operations needed could not be performed.");
                lWriter.WriteEndTag("p");

                WriteChosenVariants(lWriter);
                //writeOpStateTable(writer);
                WriteTransitionTableState(lWriter);
                WriteOpOrder(lWriter);
                //writeTransitionDiagram(writer);
                WriteAvailableResources(lWriter);
                WriteFalsePrePost(lWriter);

                lWriter.WriteEndTag("div");
                WriteDocEnd(lWriter);

                File.WriteAllText(Path + "counterEx.htm", lStringwriter.ToString());
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in writeCounterExample");
                PrintMessageToConsole(ex.Message);
            }

        }

        public void WriteModel()
        {
            StringWriter lStringwriter = new StringWriter();
            HtmlTextWriter lWriter = new HtmlTextWriter(lStringwriter);

            try
            {
                EriteDocStart(lWriter);
                WriteTabList(lWriter);

                lWriter.WriteFullBeginTag("div id=\"tabs-1\"");
                WriteInput(lWriter);
                lWriter.WriteEndTag("div");


                lWriter.WriteFullBeginTag("div id=\"tabs-2\"");
                lWriter.WriteBeginTag("p class=\"resultHeading\"");
                lWriter.Write(HtmlTextWriter.TagRightChar);
                lWriter.Write("Analysis result");
                lWriter.WriteEndTag("p");

                lWriter.WriteBeginTag("p class=\"discription\"");
                lWriter.Write(HtmlTextWriter.TagRightChar);
                lWriter.Write("Model found.");
                lWriter.WriteEndTag("p");

                WriteChosenVariants(lWriter);
                //writeOpStateTable(writer);
                WriteTransitionTableState(lWriter);
                WriteOpOrder(lWriter);
                //writeTransitionDiagram(writer);
                WriteAvailableResources(lWriter);
                WriteFalsePrePost(lWriter);

                lWriter.WriteEndTag("div");
                WriteDocEnd(lWriter);

                File.WriteAllText(Path + "Model.htm", lStringwriter.ToString());
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in writeModel");
                PrintMessageToConsole(ex.Message);
            }

        }

        public void WriteCounterExampleNoPost()
        {
            StringWriter lStringwriter = new StringWriter();
            HtmlTextWriter lWriter = new HtmlTextWriter(lStringwriter);

            try
            {
                EriteDocStart(lWriter);
                WriteTabList(lWriter);

                lWriter.WriteFullBeginTag("div id=\"tabs-1\"");
                WriteInputNoPost(lWriter);
                lWriter.WriteEndTag("div");

                lWriter.WriteFullBeginTag("div id=\"tabs-2\"");
                
                lWriter.WriteBeginTag("p class=\"resultHeading\"");
                lWriter.Write(HtmlTextWriter.TagRightChar);
                lWriter.Write("Analysis result");
                lWriter.WriteEndTag("p");

                lWriter.WriteBeginTag("p class=\"discription\"");
                lWriter.Write(HtmlTextWriter.TagRightChar);
                lWriter.Write("Counterexample found, all operations needed could not be performed.");
                lWriter.WriteEndTag("p");

                WriteChosenVariants(lWriter);
                //writeOpStateTable(writer);
                WriteTransitionTableState(lWriter);
                WriteOpOrder(lWriter);
                //writeTransitionDiagram(writer);
                WriteAvailableResources(lWriter);
                WriteFalsePre(lWriter);

                lWriter.WriteEndTag("div");
                WriteDocEnd(lWriter);

                File.WriteAllText(Path + "counterExNoPost.htm", lStringwriter.ToString());
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in writeCounterExampleNoPost");
                PrintMessageToConsole(ex.Message);
            }

        }

        public void WriteModelNoPost()
        {
            StringWriter lStringwriter = new StringWriter();
            HtmlTextWriter lWriter = new HtmlTextWriter(lStringwriter);

            try
            {
                EriteDocStart(lWriter);
                WriteTabList(lWriter);

                lWriter.WriteFullBeginTag("div id=\"tabs-1\"");
                WriteInputNoPost(lWriter);
                lWriter.WriteEndTag("div");

                lWriter.WriteFullBeginTag("div id=\"tabs-2\"");

                lWriter.WriteBeginTag("p class=\"resultHeading\"");
                lWriter.Write(HtmlTextWriter.TagRightChar);
                lWriter.Write("Analysis result");
                lWriter.WriteEndTag("p");

                lWriter.WriteBeginTag("p class=\"discription\"");
                lWriter.Write(HtmlTextWriter.TagRightChar);
                lWriter.Write("Model found.");
                lWriter.WriteEndTag("p");

                WriteChosenVariants(lWriter);
                //writeOpStateTable(writer);
                WriteTransitionTableState(lWriter);
                WriteOpOrder(lWriter);
                //writeTransitionDiagram(writer);
                WriteAvailableResources(lWriter);
                WriteFalsePre(lWriter);

                lWriter.WriteEndTag("div");
                WriteDocEnd(lWriter);

                File.WriteAllText(Path + "ModelNoPost.htm", lStringwriter.ToString());
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in writeModelNoPost");
                PrintMessageToConsole(ex.Message);
            }

        }

        public void WriteDebugFile()
        {
            StringWriter lStringwriter = new StringWriter();
            HtmlTextWriter lWriter = new HtmlTextWriter(lStringwriter);
            SortAfterState();

            try
            {
                foreach (OutputExp lExp in OutputResult)
                {
                    lWriter.WriteBeginTag("p");
                    lWriter.Write(HtmlTextWriter.TagRightChar);
                    lWriter.Write(lExp);
                    lWriter.WriteEndTag("p");
                }

                File.WriteAllText(Path + "debug.htm", lStringwriter.ToString());
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in writeDebugFile");
                PrintMessageToConsole(ex.Message);
            }

        }

        //Prints all output expressions
        public void Print()
        {
            try
            {
                foreach (OutputExp lExp in OutputResult)
                {
                    PrintMessageToConsole(lExp.ToString());
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in Print in outputHandler");
                PrintMessageToConsole(ex.Message);
            }
        }

        //Print all true outputexpressions
        public void PrintTrue()
        {
            try
            {
                foreach (OutputExp lExp in OutputResult)
                {
                    if (lExp.Value.Equals("true"))
                        PrintMessageToConsole(lExp.ToString());
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in printTrue in outputHandler");
                PrintMessageToConsole(ex.Message);
            }
        }

        //Print all chosen variants or parts
        public void PrintVariantsNParts()
        {
            try
            {
                string lVariantName, lPartName, vg;
                foreach (OutputExp lExp in OutputResult)
                {
                    //if (exp.state == -1)
                    if (lExp.OpState == null)
                    {
                        //First we have to check if the chosen item is a Variantor a part
                        if (FrameworkWrapper.ExistVariantByName(lExp.ToString()))
                        {
                            lVariantName = lExp.ToString();
                            vg = FrameworkWrapper.GetVariantGroup(lVariantName.Split(' ')[0]);
                            if (vg != "")
                            {
                                //Meaning var was a variant
                                if (!vg.Contains("Virtual-VG"))
                                    PrintMessageToConsole(vg + "." + lVariantName);
                            }
                        }
                        else
                        {
                            //Meaning var was a part
                            lPartName = lExp.ToString();
                            PrintMessageToConsole(lPartName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in printVariants");
                PrintMessageToConsole(ex.Message);
            }

        }

        private void EriteDocStart(HtmlTextWriter pWriter)
        {
            pWriter.WriteLine("<!doctype html> <html lang=\"en\"> <head><meta charset=\"utf-8\">"
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

        private void WriteDocEnd(HtmlTextWriter pWriter)
        {
            pWriter.WriteLine("</body> </html>");
        }

        private void WriteTabList(HtmlTextWriter pWriter)
        {
            pWriter.WriteLine("<div id=\"tabs\">" +
                             "<ul>" +
                             "<li><a href=\"#tabs-1\">Input</a></li>" +
                             "<li><a href=\"#tabs-2\">Result</a></li>" +
                             "</ul>");
        }

        private void WriteTransitionDiagram(HtmlTextWriter pWriter)
        {
            List<string[]> lTransforms = GetOpTransformations();

            pWriter.WriteBeginTag("p id=\"outOOT\" class=\"title\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("Order of operation transitions");

            pWriter.WriteBeginTag("span id=\"titleOOTArr\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("&#x25BC");
            pWriter.WriteEndTag("span");
            pWriter.WriteEndTag("p");

            pWriter.WriteBeginTag("div id=\"outOOTContent\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);


            pWriter.WriteBeginTag("table style=\"margin-left:40px\"");
            //writer.WriteBeginTag("ol style=\"margin-left:1em;\" ");
            pWriter.Write(HtmlTextWriter.TagRightChar);


            pWriter.WriteBeginTag("tr");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.WriteBeginTag("th");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("Operation");
            pWriter.WriteEndTag("th");
            pWriter.WriteBeginTag("th");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("Transition");
            pWriter.WriteEndTag("th");


            foreach (String[] lTrans in lTransforms)
            {
                pWriter.WriteBeginTag("tr");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.WriteBeginTag("td");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                //Pil ner
                //writer.Write("&darr; ");
                pWriter.Write(lTrans[0]);

                pWriter.WriteEndTag("td");
                pWriter.WriteBeginTag("td");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                if (String.Equals(lTrans[2], "E"))
                    //pil upp
                    pWriter.Write(" &uarr;");
                else
                    //pil ner
                    pWriter.Write(" &darr;");

                pWriter.WriteEndTag("td");
                pWriter.WriteEndTag("tr");
            }
            pWriter.WriteEndTag("table");
            pWriter.WriteEndTag("div");
        }

        //Returns all chosen variants
        public List<String> GetChosenVariants()
        {
            List<String> lVars = new List<String>();
            try
            {
                foreach (OutputExp lExp in OutputResult)
                {
                    if (lExp.State == -1 && String.Equals(lExp.Value, "true") && !String.Equals(lExp.OpState, "possible") && !String.Equals(lExp.OpState, "use"))
                        lVars.Add(lExp.Name);
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in getChosenVariants in outputHandler");
                PrintMessageToConsole(ex.Message);
            }
            return lVars;

        }

        //Returns all chosen variants
        public List<string[]> GetChosenVariantsWithGroup()
        {
            string lVar, lVg;
            string[] lList;
            List<string[]> lVars = new List<string[]>();

            try
            {
                foreach (OutputExp lExp in OutputResult)
                {
                    //if (exp.opState == null 
                    //    && String.Equals(exp.value, "true") 
                    //    && !String.Equals(exp.opState, "possible") 
                    //    && !String.Equals(exp.opState, "use")
                    //    && !String.Equals(exp.opState, "precondition")
                    //    && !String.Equals(exp.opState, "trigger")
                    //    )
                    if (lExp.Variant != null)
                    {

                        lVar = lExp.Name;
                        lVg = FrameworkWrapper.GetVariantGroup(lVar);
                        if (!String.Equals(lVg, "Virtual-VG"))
                        {
                            lList = new string[2] { lVg, lVar };
                            lVars.Add(lList);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in getChosenVariantsWithGroup in outputHandler");
                PrintMessageToConsole(ex.Message);
            }
            return lVars;
        }


        //Returns all chosen Variantgroups
        public List<string> GetChosenVariantGroups()
        {
            string lVar, lVg;
            List<string> lVars = new List<string>();

            try
            {
                foreach (OutputExp lExp in OutputResult)
                {
                    //if (exp.opState == null 
                    //    && String.Equals(exp.value, "true") 
                    //    && !String.Equals(exp.opState, "possible") 
                    //    && !String.Equals(exp.opState, "use")
                    //    && !String.Equals(exp.opState, "precondition")
                    //    && !String.Equals(exp.opState, "trigger")
                    //    )
                    if (lExp.Variant != null)
                    {

                        lVar = lExp.Name;
                        lVg = FrameworkWrapper.GetVariantGroup(lVar);
                        if (!String.Equals(lVg, "Virtual-VG") && !lVars.Contains(lVg))
                        {
                            lVars.Add(lVg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in getChosenVariantGroups in outputHandler");
                PrintMessageToConsole(ex.Message);
            }
            return lVars;
        }

        //Print all operation transformations
        public void PrintOpTransformations()
        {
            try
            {
                foreach (OutputExp lExp in OutputResult)
                {
                    OutputExp lNextOp = FindNextOp(lExp);
                    if (lNextOp != null)
                        if (String.Equals(lExp.Value, "true") && String.Equals(lNextOp.Value, "true"))
                            PrintMessageToConsole(lExp.ToString() + " -> " + lNextOp.ToString());
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in printOpTransformations in outputHandler");
                PrintMessageToConsole(ex.Message);
            }
        }

        private void WriteChosenVariants(HtmlTextWriter pWriter)
        {
            List<String[]> lVariants = GetChosenVariantsWithGroup();
            List<String> lGroups = GetChosenVariantGroups();


            pWriter.WriteBeginTag("p id=\"outCV\" class=\"title\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("Chosen variants");

            pWriter.WriteBeginTag("span id=\"titleCVArr\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("&#x25BC");
            pWriter.WriteEndTag("span");
            pWriter.WriteEndTag("p");

            pWriter.WriteBeginTag("div id=\"outCVContent\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);



            pWriter.WriteBeginTag("table style=\"margin-left:40px\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.WriteBeginTag("tr");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.WriteBeginTag("th");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("Variants");
            pWriter.WriteEndTag("th");
            pWriter.WriteEndTag("tr");

            foreach (String lVg in lGroups)
            {
                if (!String.Equals(lVg, "Virtual-VG"))
                {

                    pWriter.WriteBeginTag("tr");
                    pWriter.Write(HtmlTextWriter.TagRightChar);
                    pWriter.WriteBeginTag("th class=\"vg\"");
                    pWriter.Write(HtmlTextWriter.TagRightChar);
                    pWriter.Write(lVg);
                    pWriter.WriteEndTag("th");
                    pWriter.WriteEndTag("tr");
                    foreach (string[] var in lVariants)
                        if (String.Equals(var[0], lVg))
                        {
                            pWriter.WriteBeginTag("tr");
                            pWriter.Write(HtmlTextWriter.TagRightChar);
                            pWriter.WriteBeginTag("td");
                            pWriter.Write(HtmlTextWriter.TagRightChar);
                            pWriter.Write(var[1]);
                            pWriter.WriteEndTag("td");
                            pWriter.WriteEndTag("tr");
                        }
                }
            }
            pWriter.WriteEndTag("table");
            pWriter.WriteEndTag("div");
        }

        private List<OutputExp> GetPossibleResources()
        {
            List<OutputExp> lPoss = new List<OutputExp>();

            try
            {
                foreach (OutputExp lExp in OutputResult)
                {
                    if (String.Equals(lExp.OpState, "possible"))
                        lPoss.Add(lExp);
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in getPossibleResources in outputHandler");
                PrintMessageToConsole(ex.Message);
            }

            return lPoss;
        }

        private void WriteAvailableResources(HtmlTextWriter pWriter)
        {
            SortAfterValue();

            List<OutputExp> lPossibleRes = GetPossibleResources();

            pWriter.WriteBeginTag("p id=\"outAR\" class=\"title\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("Available resources");

            pWriter.WriteBeginTag("span id=\"titleARArr\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("&#x25BC");
            pWriter.WriteEndTag("span");
            pWriter.WriteEndTag("p");

            pWriter.WriteBeginTag("div id=\"outARContent\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            if (lPossibleRes.Count != 0)
            {
                pWriter.WriteBeginTag("table style=\"margin-left:40px\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.WriteBeginTag("tr");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.WriteBeginTag("th");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("Resource");
                pWriter.WriteEndTag("th");

                pWriter.WriteBeginTag("th");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("Available");
                pWriter.WriteEndTag("th");
                pWriter.WriteEndTag("tr");

                foreach (OutputExp lRes in lPossibleRes)
                {
                    pWriter.WriteBeginTag("tr");
                    pWriter.Write(HtmlTextWriter.TagRightChar);
                    pWriter.WriteBeginTag("td");
                    pWriter.Write(HtmlTextWriter.TagRightChar);
                    pWriter.Write(lRes.Operation);
                    pWriter.WriteEndTag("td");

                    pWriter.WriteBeginTag("td");
                    pWriter.Write(HtmlTextWriter.TagRightChar);
                    pWriter.Write(lRes.Value);
                    pWriter.WriteEndTag("td");

                    pWriter.WriteEndTag("tr");
                }
                pWriter.WriteEndTag("table");
            }
            else
            {

                pWriter.WriteBeginTag("p class=\"empty\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("No available resources.");
                pWriter.WriteEndTag("p");

            }

            pWriter.WriteEndTag("div");
        }

        private void WriteVariants(HtmlTextWriter pWriter)
        {
            HashSet<VariantGroup> lVariants = FrameworkWrapper.GetVariantGroupSet();


            pWriter.WriteBeginTag("table style=\"margin-left:40px\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.WriteBeginTag("tr");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.WriteBeginTag("th");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("Variant groups");
            pWriter.WriteEndTag("th");
            pWriter.WriteEndTag("tr");


            foreach (VariantGroup lGroup in lVariants)
            {
                if (!String.Equals(lGroup.Names, "Virtual-VG"))
                {
                    pWriter.WriteBeginTag("tr");
                    pWriter.Write(HtmlTextWriter.TagRightChar);


                    pWriter.WriteBeginTag("th class =\"vg\"");
                    pWriter.Write(HtmlTextWriter.TagRightChar);

                    pWriter.WriteBeginTag("b");
                    pWriter.Write(HtmlTextWriter.TagRightChar);
                    pWriter.Write(lGroup.Names);
                    pWriter.WriteEndTag("b");
                    pWriter.Write(" - " + lGroup.GCardinality);

                    pWriter.WriteEndTag("th");

                    pWriter.WriteEndTag("tr");

                    foreach (Variant var in lGroup.Variants)
                    {
                        pWriter.WriteFullBeginTag("tr");
                        pWriter.WriteBeginTag("td");
                        pWriter.Write(HtmlTextWriter.TagRightChar);
                        pWriter.Write(var.Names);
                        pWriter.WriteEndTag("td");

                        pWriter.WriteEndTag("tr");
                    }

                }
            }
            pWriter.WriteEndTag("table");
        }

        private void WriteConstraints(HtmlTextWriter pWriter)
        {
            HashSet<string> lConstraints = new HashSet<string>(FrameworkWrapper.GetConstraintSet());

            if (lConstraints.Count != 0)
            {
                pWriter.WriteBeginTag("p style=\"font-size:18px\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write(" ");
                pWriter.WriteEndTag("p");


                pWriter.WriteBeginTag("table style=\"margin-left:40px\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.WriteBeginTag("tr");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.WriteBeginTag("th");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("Constraints");
                pWriter.WriteEndTag("th");
                pWriter.WriteEndTag("tr");

                foreach (String lCon in lConstraints)
                {
                    //if (checkCondition(con))
                    //{
                        pWriter.WriteBeginTag("tr");
                        pWriter.Write(HtmlTextWriter.TagRightChar);
                        pWriter.WriteBeginTag("td");
                        pWriter.Write(HtmlTextWriter.TagRightChar);
                        pWriter.Write(GeneralUtilities.ParseExpression(lCon, "infix"));
                        pWriter.WriteEndTag("td");
                        pWriter.WriteEndTag("tr");
                    //}
                }
                pWriter.WriteEndTag("table");
            }
            else
            {

                pWriter.WriteBeginTag("p class=\"empty\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("No constraints were specified.");
                pWriter.WriteEndTag("p");

            }
        }

        private void WriteOperationsWithPrePostCon(HtmlTextWriter pWriter)
        {
            List<Operation> lOperations = new List<Operation>(FrameworkWrapper.OperationSet);

            pWriter.WriteBeginTag("p id=\"inO\" class=\"title\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write(" Operations");

            pWriter.WriteBeginTag("span id=\"titleOArr\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("&#x25BC");
            //upp &#x25B2
            pWriter.WriteEndTag("span");
            pWriter.WriteEndTag("p");

            pWriter.WriteBeginTag("div id=\"inOContent\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.WriteBeginTag("table  style=\"margin-left:40px\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.WriteBeginTag("tr");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.WriteBeginTag("th");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.Write("Operation");

            pWriter.WriteEndTag("th");

            pWriter.WriteBeginTag("th");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.Write("Precondition");

            pWriter.WriteEndTag("th");


            pWriter.WriteBeginTag("th");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.Write("Postcondition");

            pWriter.WriteEndTag("th");

            pWriter.WriteBeginTag("th");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.Write("Requirements");

            pWriter.WriteEndTag("th");

            foreach (Operation lOp in lOperations)
            {
                

                pWriter.WriteBeginTag("tr");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.WriteBeginTag("td");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.Write(lOp.Name);

                pWriter.WriteEndTag("td");


                pWriter.WriteBeginTag("td");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.WriteBeginTag("ul style=\"list-style-type:none\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                if (lOp.Precondition != null)
                {
                    foreach (string lPre in lOp.Precondition)
                    {
                        if (!lPre.Contains("Possible"))
                        {
                            pWriter.WriteBeginTag("li");
                            pWriter.Write(HtmlTextWriter.TagRightChar);
                            pWriter.Write(GeneralUtilities.ParseExpression(lPre, "infix"));
                            pWriter.WriteEndTag("li");
                        }
                    }
                }
                pWriter.WriteEndTag("ul");

                pWriter.WriteEndTag("td");

                pWriter.WriteBeginTag("td");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.WriteBeginTag("ul style=\"list-style-type:none\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                /*if (op.postcondition != null)
                {
                    foreach (string post in op.postcondition)
                    {
                        writer.WriteBeginTag("li");
                        writer.Write(HtmlTextWriter.TagRightChar);
                        writer.Write(GeneralUtilities.parseExpression(post, "infix"));
                        writer.WriteEndTag("li");
                    }
                }*/
                pWriter.WriteEndTag("ul");

                pWriter.WriteEndTag("td");

                pWriter.WriteBeginTag("td");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.WriteBeginTag("ul style=\"list-style-type:none\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                if (lOp.Requirement != null)
                {
                    pWriter.WriteBeginTag("li");
                    pWriter.Write(HtmlTextWriter.TagRightChar);
                    pWriter.Write(GeneralUtilities.ParseExpression(lOp.Requirement, "infix"));
                    pWriter.WriteEndTag("li");
                }
                pWriter.WriteEndTag("ul");

                pWriter.WriteEndTag("td");

                pWriter.WriteEndTag("tr");
            }

            pWriter.WriteEndTag("table");
            pWriter.WriteEndTag("div");

        }


        private void WriteOperationsWithPreCon(HtmlTextWriter pWriter)
        {
            List<Operation> lOperations = new List<Operation>(FrameworkWrapper.OperationSet);

            pWriter.WriteBeginTag("p id=\"inO\" class=\"title\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("Operations");

            pWriter.WriteBeginTag("span id=\"titleOArr\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("&#x25BC");
            //upp &#x25B2
            pWriter.WriteEndTag("span");
            pWriter.WriteEndTag("p");

            pWriter.WriteBeginTag("div id=\"inOContent\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.WriteBeginTag("table  style=\"margin-left:40px\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.WriteBeginTag("tr");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.WriteBeginTag("th");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.Write("Operation");

            pWriter.WriteEndTag("th");

            pWriter.WriteBeginTag("th");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.Write("Precondition");

            pWriter.WriteEndTag("th");

            pWriter.WriteBeginTag("th");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.Write("Requirements");

            pWriter.WriteEndTag("th");

            foreach (Operation lOp in lOperations)
            {

                pWriter.WriteBeginTag("tr");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.WriteBeginTag("td");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.Write(lOp.Name);

                pWriter.WriteEndTag("td");


                pWriter.WriteBeginTag("td");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.WriteBeginTag("ul style=\"list-style-type:none\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                if (lOp.Precondition != null)
                {
                    foreach (string lPre in lOp.Precondition)
                    {
                        pWriter.WriteBeginTag("li");
                        pWriter.Write(HtmlTextWriter.TagRightChar);
                        pWriter.Write(GeneralUtilities.ParseExpression(lPre, "infix"));
                        pWriter.WriteEndTag("li");
                    }
                }

                pWriter.WriteEndTag("ul");

                pWriter.WriteEndTag("td");

                pWriter.WriteBeginTag("td");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.WriteBeginTag("ul style=\"list-style-type:none\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                if (lOp.Requirement != null)
                {
                    pWriter.WriteBeginTag("li");
                    pWriter.Write(HtmlTextWriter.TagRightChar);
                    pWriter.Write(lOp.Requirement);
                    pWriter.WriteEndTag("li");
                }
                pWriter.WriteEndTag("ul");

                pWriter.WriteEndTag("td");

                pWriter.WriteEndTag("tr");
            }

            pWriter.WriteEndTag("table");
            pWriter.WriteEndTag("div");

        }


        /*private void writePartOperationMappings(HtmlTextWriter writer)
        {
            HashSet<partOperations> lPartOperationsList = new HashSet<partOperations>(cFrameworkWrapper.getPartsOperationsSet());

            writer.WriteBeginTag("p id=\"inM\" class=\"title\"");
            writer.Write(HtmlTextWriter.TagRightChar);
            writer.Write(" Part operation mappings");

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

            writer.Write("Parts");

            writer.WriteEndTag("th");

            writer.WriteBeginTag("th");
            writer.Write(HtmlTextWriter.TagRightChar);

            writer.Write("Operations");

            writer.WriteEndTag("th");
            writer.WriteEndTag("tr");

            foreach (partOperations vop in lPartOperationsList)
            {

                writer.WriteBeginTag("tr");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.WriteBeginTag("td");
                writer.Write(HtmlTextWriter.TagRightChar);

                writer.Write(replaceVirtual(vop.getPartExpr()));

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

        }*/

        /*private void writeVariantOperationMappings(HtmlTextWriter writer)
        {
            HashSet<variantOperations> lVariantOperationsList = new HashSet<variantOperations>(cFrameworkWrapper.getVariantsOperationsSet());

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

            foreach (variantOperations vop in lVariantOperationsList)
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

        }*/

        private void WriteResourcesAndTraits(HtmlTextWriter pWriter)
        {

            pWriter.WriteBeginTag("p id=\"inR\" class=\"title\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("Traits and resources");

            pWriter.WriteBeginTag("span id=\"titleRArr\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("&#x25BC");
            pWriter.WriteEndTag("span");
            pWriter.WriteEndTag("p");

            pWriter.WriteBeginTag("div id=\"inRContent\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            WriteTraits(pWriter);

            pWriter.WriteBeginTag("p style=\"font-size:18px\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write(" ");
            pWriter.WriteEndTag("p");


            WriteResources(pWriter);

            pWriter.WriteEndTag("div");

        }

        private void WriteTraits(HtmlTextWriter pWriter)
        {

            HashSet<Trait> lTraits = new HashSet<Trait>(FrameworkWrapper.TraitSet);

            if (lTraits.Count != 0)
            {
                pWriter.WriteBeginTag("table  style=\"margin-left:40px\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.WriteBeginTag("tr");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.WriteBeginTag("th");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.Write("Trait");

                pWriter.WriteEndTag("th");


                pWriter.WriteBeginTag("th");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.Write("Inherit");

                pWriter.WriteEndTag("th");


                pWriter.WriteBeginTag("th");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.Write("Attributes");

                pWriter.WriteEndTag("th");
                pWriter.WriteEndTag("tr");


                foreach (Trait lTra in lTraits)
                {

                    pWriter.WriteBeginTag("tr");
                    pWriter.Write(HtmlTextWriter.TagRightChar);

                    pWriter.WriteBeginTag("td");
                    pWriter.Write(HtmlTextWriter.TagRightChar);

                    pWriter.Write(lTra.Names);

                    pWriter.WriteEndTag("td");

                    pWriter.WriteBeginTag("td");
                    pWriter.Write(HtmlTextWriter.TagRightChar);

                    pWriter.WriteBeginTag("ul style=\"list-style-type:none\"");
                    pWriter.Write(HtmlTextWriter.TagRightChar);

                    foreach (Trait inh in lTra.Inherit)
                    {
                        pWriter.WriteBeginTag("li");
                        pWriter.Write(HtmlTextWriter.TagRightChar);

                        pWriter.Write(inh.Names);

                        pWriter.WriteEndTag("li");

                    }

                    pWriter.WriteEndTag("ul");
                    pWriter.WriteEndTag("td");

                    pWriter.WriteBeginTag("td");
                    pWriter.Write(HtmlTextWriter.TagRightChar);

                    pWriter.WriteBeginTag("ul style=\"list-style-type:none\"");
                    pWriter.Write(HtmlTextWriter.TagRightChar);

                    foreach (Tuple<string, string> att in lTra.Attributes)
                    {
                        pWriter.WriteBeginTag("li");
                        pWriter.Write(HtmlTextWriter.TagRightChar);
                        pWriter.Write(att.Item1 + ": " + att.Item2);
                        pWriter.WriteEndTag("li");
                    }
                    pWriter.WriteEndTag("ul");

                    pWriter.WriteEndTag("td");

                    pWriter.WriteEndTag("tr");
                }

                pWriter.WriteEndTag("table");
            }
            else
            {

                pWriter.WriteBeginTag("p class=\"empty\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("No Traits were specified.");
                pWriter.WriteEndTag("p");

            }
        }

        private void WriteResources(HtmlTextWriter pWriter)
        {

            HashSet<Resource> lResources = new HashSet<Resource>(FrameworkWrapper.ResourceSet);

            if (lResources.Count != 0)
            {
                pWriter.WriteBeginTag("table  style=\"margin-left:40px\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.WriteBeginTag("tr");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.WriteBeginTag("th");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.Write("Resource");

                pWriter.WriteEndTag("th");


                pWriter.WriteBeginTag("th");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.Write("Of traits");

                pWriter.WriteEndTag("th");


                pWriter.WriteBeginTag("th");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.Write("Attributes");

                pWriter.WriteEndTag("th");

                pWriter.WriteEndTag("tr");


                foreach (Resource lRes in lResources)
                {

                    pWriter.WriteBeginTag("tr");
                    pWriter.Write(HtmlTextWriter.TagRightChar);

                    pWriter.WriteBeginTag("td");
                    pWriter.Write(HtmlTextWriter.TagRightChar);

                    pWriter.Write(lRes.Name);

                    pWriter.WriteEndTag("td");

                    pWriter.WriteBeginTag("td");
                    pWriter.Write(HtmlTextWriter.TagRightChar);

                    pWriter.WriteBeginTag("ul style=\"list-style-type:none\"");
                    pWriter.Write(HtmlTextWriter.TagRightChar);

                    //foreach (trait lTra in lRes.traits)
                    //{
                    //    pWriter.WriteBeginTag("li");
                    //    pWriter.Write(HtmlTextWriter.TagRightChar);
                    //    pWriter.Write(lTra.names);
                    //    pWriter.WriteEndTag("li");
                    //}
                    //pWriter.WriteEndTag("ul");

                    pWriter.WriteEndTag("td");

                    pWriter.WriteBeginTag("td");
                    pWriter.Write(HtmlTextWriter.TagRightChar);

                    pWriter.WriteBeginTag("ul style=\"list-style-type:none\"");
                    pWriter.Write(HtmlTextWriter.TagRightChar);

                    //foreach (Tuple<string, string, string> att in lRes.attributes)
                    //{
                    //    pWriter.WriteBeginTag("li");
                    //    pWriter.Write(HtmlTextWriter.TagRightChar);
                    //    pWriter.Write(att.Item1 + " = " + att.Item3);
                    //    pWriter.WriteEndTag("li");
                    //}
                    //pWriter.WriteEndTag("ul");

                    pWriter.WriteEndTag("td");

                    pWriter.WriteEndTag("tr");
                }

                pWriter.WriteEndTag("table");
            }
            else
            {

                pWriter.WriteBeginTag("p class=\"empty\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("No Resources were specified.");
                pWriter.WriteEndTag("p");

            }
        }


        private void WriteFalsePre(HtmlTextWriter pWriter)
        {
            HashSet<String[]> lConditions = GetConditionsStateWithValues(GetLastState());

            if (lConditions.Count != 0)
            {
                pWriter.WriteBeginTag("p id=\"outCon\" class=\"title\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("False preconditions in last state");

                pWriter.WriteBeginTag("span id=\"titleConArr\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("&#x25BC");
                pWriter.WriteEndTag("span");
                pWriter.WriteEndTag("p");

                pWriter.WriteBeginTag("div id=\"outConContent\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);


                pWriter.WriteBeginTag("table style=\"margin-left:40px\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);


                pWriter.WriteBeginTag("tr");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.WriteBeginTag("th");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("Name");
                pWriter.WriteEndTag("th");
                pWriter.WriteBeginTag("th");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("Condition");
                pWriter.WriteEndTag("th");
                pWriter.WriteBeginTag("th");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("Value");
                pWriter.WriteEndTag("th");
                pWriter.WriteEndTag("tr");

                foreach (String[] lCon in lConditions)
                {
                    if (!lCon[0].Contains("Post"))
                    {
                        pWriter.WriteBeginTag("tr");
                        pWriter.Write(HtmlTextWriter.TagRightChar);
                        pWriter.WriteBeginTag("td");
                        pWriter.Write(HtmlTextWriter.TagRightChar);
                        pWriter.Write(lCon[0]);
                        pWriter.WriteEndTag("td");
                        pWriter.WriteBeginTag("td");
                        pWriter.Write(HtmlTextWriter.TagRightChar);
                        pWriter.Write(lCon[1]);
                        pWriter.WriteEndTag("td");
                        pWriter.WriteBeginTag("td");
                        pWriter.Write(HtmlTextWriter.TagRightChar);
                        pWriter.Write(lCon[2]);
                        pWriter.WriteEndTag("td");
                        pWriter.WriteEndTag("tr");
                    }
                }
                pWriter.WriteEndTag("table");
                pWriter.WriteEndTag("div");
            }
            else
            {

                pWriter.WriteBeginTag("p class=\"empty\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("No false condition in last state.");
                pWriter.WriteEndTag("p");

            }
        }


        private void WriteFalsePrePost(HtmlTextWriter pWriter)
        {
            HashSet<String[]> lConditions = GetConditionsStateWithValues(GetLastState());

            if (lConditions.Count != 0)
            {
                pWriter.WriteBeginTag("p id=\"outCon\" class=\"title\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("False post/preconditions in last state");

                pWriter.WriteBeginTag("span id=\"titleConArr\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("&#x25BC");
                pWriter.WriteEndTag("span");
                pWriter.WriteEndTag("p");

                pWriter.WriteBeginTag("div id=\"outConContent\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);


                pWriter.WriteBeginTag("table style=\"margin-left:40px\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);


                pWriter.WriteBeginTag("tr");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.WriteBeginTag("th");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("Name");
                pWriter.WriteEndTag("th");
                pWriter.WriteBeginTag("th");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("Condition");
                pWriter.WriteEndTag("th");
                pWriter.WriteBeginTag("th");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("Value");
                pWriter.WriteEndTag("th");
                pWriter.WriteEndTag("tr");

                foreach (String[] lCon in lConditions)
                {
                    pWriter.WriteBeginTag("tr");
                    pWriter.Write(HtmlTextWriter.TagRightChar);
                    pWriter.WriteBeginTag("td");
                    pWriter.Write(HtmlTextWriter.TagRightChar);
                    pWriter.Write(lCon[0]);
                    pWriter.WriteEndTag("td");
                    pWriter.WriteBeginTag("td");
                    pWriter.Write(HtmlTextWriter.TagRightChar);
                    pWriter.Write(lCon[1]);
                    pWriter.WriteEndTag("td");
                    pWriter.WriteBeginTag("td");
                    pWriter.Write(HtmlTextWriter.TagRightChar);
                    pWriter.Write(lCon[2]);
                    pWriter.WriteEndTag("td");
                    pWriter.WriteEndTag("tr");
                }
                pWriter.WriteEndTag("table");
                pWriter.WriteEndTag("div");
            }
            else
            {

                pWriter.WriteBeginTag("p class=\"empty\"");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("No false conditions in last state.");
                pWriter.WriteEndTag("p");

            }
        }

        private void WriteOpOrderWithArrow(HtmlTextWriter pWriter)
        {
            Boolean lFirst = true;

            pWriter.WriteBeginTag("p id=\"outOpO\" class=\"title\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("Sequence of finished operation");

            pWriter.WriteBeginTag("span id=\"titleOpOArr\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("&#x25BC");
            pWriter.WriteEndTag("span");
            pWriter.WriteEndTag("p");

            pWriter.WriteBeginTag("div id=\"outOpOContent\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.WriteBeginTag("p style=\"margin-left:2.5em;\" ");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            foreach (OutputExp lExp in OutputResult)
            {
                OutputExp lNextOp = FindNextOp(lExp);
                if (lNextOp != null)
                    if (String.Equals(lExp.Value, "true") && String.Equals(lNextOp.Value, "true") && String.Equals(lExp.OpState, "E"))
                    {
                        if (!lFirst)
                        {
                            pWriter.Write(" &rarr; ");
                        }
                        else
                            lFirst = false;
                        pWriter.Write(lExp.Operation);
                    }
            }

            pWriter.WriteEndTag("p");
            pWriter.WriteEndTag("div");
        }


        private void WriteOpOrder(HtmlTextWriter pWriter)
        {
            int lCount = 1;

            pWriter.WriteBeginTag("p id=\"outOpO\" class=\"title\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("Sequence of finished operation");

            pWriter.WriteBeginTag("span id=\"titleOpOArr\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("&#x25BC");
            pWriter.WriteEndTag("span");
            pWriter.WriteEndTag("p");

            pWriter.WriteBeginTag("div id=\"outOpOContent\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.WriteBeginTag("table style=\"margin-left:40px\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);


            pWriter.WriteBeginTag("tr ");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.WriteBeginTag("th ");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("Operation");
            pWriter.WriteEndTag("th");
            pWriter.WriteEndTag("tr");

            OutputResult.Sort((x, y) => x.State.CompareTo(y.State));

            foreach (OutputExp lExp in OutputResult)
            {
                OutputExp lNextOp = FindNextOp(lExp);
                if (lNextOp != null)
                    if (String.Equals(lExp.Value, "true") && String.Equals(lNextOp.Value, "true") && String.Equals(lExp.OpState, "E"))
                    {

                        pWriter.WriteBeginTag("tr ");
                        pWriter.Write(HtmlTextWriter.TagRightChar);
                        pWriter.WriteBeginTag("td ");
                        pWriter.Write(HtmlTextWriter.TagRightChar);

                        pWriter.Write((lCount++) + ". " + lExp.Operation);
                        pWriter.WriteEndTag("td");
                        pWriter.WriteEndTag("tr");
                    }
            }

            pWriter.WriteEndTag("table");
            pWriter.WriteEndTag("div");
        }

        private void WriteOpStateTable(HtmlTextWriter pWriter)
        {
            SortAfterValue();
            List<String[]> lOperationState = GetOpState(GetLastState());
            String[] lTransF;
            String[] lTransI;

            pWriter.WriteBeginTag("p id=\"outOpLast\" class=\"title\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("Operation statuses in last state");

            pWriter.WriteBeginTag("span id=\"titleOpLastArr\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("&#x25BC");
            pWriter.WriteEndTag("span");
            pWriter.WriteEndTag("p");

            pWriter.WriteBeginTag("div id=\"inOpLastContent\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);


            pWriter.WriteBeginTag("table style=\"margin-left:40px\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.WriteBeginTag("tr");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.WriteBeginTag("th");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("Operation");
            pWriter.WriteEndTag("th");

            pWriter.WriteBeginTag("th");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("I");
            pWriter.WriteEndTag("th");

            pWriter.WriteBeginTag("th");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("E");
            pWriter.WriteEndTag("th");

            pWriter.WriteBeginTag("th");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("F");
            pWriter.WriteEndTag("th");


            pWriter.WriteEndTag("tr");

            foreach (String[] lTrans in lOperationState)
            {
                if (String.Equals("E", lTrans[2]))
                {
                    pWriter.WriteBeginTag("tr");
                    pWriter.Write(HtmlTextWriter.TagRightChar);

                    lTransF = lOperationState.Find(x => String.Equals(x[0], lTrans[0]) &&
                                                       String.Equals(x[2], "F"));

                    lTransI = lOperationState.Find(x => String.Equals(x[0], lTrans[0]) &&
                                                       String.Equals(x[2], "I"));

                    if (String.Equals(lTrans[3], "false") && String.Equals(lTransI[3], "false") &&
                                                            String.Equals(lTransF[3], "false"))
                    {
                        lTransI[3] = "-";
                        lTrans[3] = "-";
                        lTransF[3] = "-";
                    }

                    pWriter.WriteBeginTag("td");
                    pWriter.Write(HtmlTextWriter.TagRightChar);
                    pWriter.Write(lTrans[0]);
                    pWriter.WriteEndTag("td");


                    pWriter.WriteBeginTag("td");
                    pWriter.Write(HtmlTextWriter.TagRightChar);

                    if (lTransI != null)
                        pWriter.Write(lTransI[3]);

                    pWriter.WriteEndTag("td");

                    pWriter.WriteBeginTag("td");
                    pWriter.Write(HtmlTextWriter.TagRightChar);
                    if (String.Equals("E", lTrans[2]))
                        pWriter.Write(lTrans[3]);

                    pWriter.WriteEndTag("td");

                    pWriter.WriteBeginTag("td");
                    pWriter.Write(HtmlTextWriter.TagRightChar);
                    if (lTransF != null)
                        pWriter.Write(lTransF[3]);

                    pWriter.WriteEndTag("td");

                    pWriter.WriteEndTag("tr");
                }
            }
            pWriter.WriteEndTag("table");
            pWriter.WriteEndTag("div");
        }

        private void WriteTransitionTableStatus(HtmlTextWriter pWriter)
        {
            SortAfterState();
            String[] lTransPair;
            List<String[]> lTransformations = GetOpTransformations();

            pWriter.WriteBeginTag("p id=\"outTT\" class=\"title\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("Operations progressing at state");

            pWriter.WriteBeginTag("span id=\"titleTTArr\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("&#x25BC");
            pWriter.WriteEndTag("span");
            pWriter.WriteEndTag("p");

            pWriter.WriteBeginTag("div id=\"outTTContent\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);



            pWriter.WriteBeginTag("table style=\"margin-left:40px\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.WriteBeginTag("tr");
            pWriter.Write(HtmlTextWriter.TagRightChar);


            pWriter.WriteBeginTag("th");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("Operation");
            pWriter.WriteEndTag("th");

            pWriter.WriteBeginTag("th");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("I->E");
            pWriter.WriteEndTag("th");


            pWriter.WriteBeginTag("th");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("E->F");
            pWriter.WriteEndTag("th");


            pWriter.WriteEndTag("tr");

            if (lTransformations.Count == 0)
            {
                pWriter.WriteBeginTag("tr");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.WriteBeginTag("td");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("-");
                pWriter.WriteEndTag("td");

                pWriter.WriteBeginTag("td");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("-");
                pWriter.WriteEndTag("td");

                pWriter.WriteBeginTag("td");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("-");
                pWriter.WriteEndTag("td");

                pWriter.WriteEndTag("tr");
            }

            foreach (String[] lTrans in lTransformations)
            {
                if (String.Equals("E", lTrans[2]))
                {
                    pWriter.WriteBeginTag("tr");
                    pWriter.Write(HtmlTextWriter.TagRightChar);

                    lTransPair = lTransformations.Find(x => String.Equals(x[0], lTrans[0]) &&
                                                          String.Equals(x[3], lTrans[3]) &&
                                                          !String.Equals(x[2], lTrans[2]));

                    pWriter.WriteBeginTag("td");
                    pWriter.Write(HtmlTextWriter.TagRightChar);
                    pWriter.Write(lTrans[0]);
                    pWriter.WriteEndTag("td");


                    pWriter.WriteBeginTag("td");
                    pWriter.Write(HtmlTextWriter.TagRightChar);

                    if (String.Equals("E", lTrans[2]))
                        pWriter.Write(lTrans[1]);
                    else if (lTransPair != null)
                    {
                        if (String.Equals("E", lTransPair[2]))
                            pWriter.Write(lTransPair[1]);
                    }

                    pWriter.WriteEndTag("td");

                    pWriter.WriteBeginTag("td");
                    pWriter.Write(HtmlTextWriter.TagRightChar);
                    if (String.Equals("F", lTrans[2]))
                        pWriter.Write(lTrans[1]);
                    else if (lTransPair != null)
                    {
                        if (String.Equals("F", lTransPair[2]))
                            pWriter.Write(lTransPair[1]);
                    }
                    pWriter.WriteEndTag("td");

                    pWriter.WriteEndTag("tr");
                }
            }
            pWriter.WriteEndTag("table");
            pWriter.WriteEndTag("div");

        }


        private void WriteTransitionTableState(HtmlTextWriter pWriter)
        {
            //List<String> activeOps = getActiveOps();
            List<OutputExp> lAllOps = GetAllOps();
            List<String[]> lOpTransitions;
            int lLastState = GetLastState();

            pWriter.WriteBeginTag("p id=\"outOSS\" class=\"title\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("Operation statuses in different states");

            pWriter.WriteBeginTag("span id=\"titleOSSArr\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("&#x25BC");
            pWriter.WriteEndTag("span");
            pWriter.WriteEndTag("p");

            pWriter.WriteBeginTag("div id=\"outOSSContent\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);


            pWriter.WriteBeginTag("table style=\"margin-left:40px\"");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.WriteBeginTag("tr");
            pWriter.Write(HtmlTextWriter.TagRightChar);

            pWriter.WriteBeginTag("th");
            pWriter.Write(HtmlTextWriter.TagRightChar);
            pWriter.Write("Operation");
            pWriter.WriteEndTag("th");

            for (int i = 0; i <= lLastState + 1; i++)
            {
                pWriter.WriteBeginTag("th");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write("" + i);
                pWriter.WriteEndTag("th");
            }

            pWriter.WriteEndTag("tr");

            foreach (OutputExp op in lAllOps)
            {
                lOpTransitions = GetOpTransformations(op);

                pWriter.WriteBeginTag("tr");
                pWriter.Write(HtmlTextWriter.TagRightChar);

                pWriter.WriteBeginTag("td");
                pWriter.Write(HtmlTextWriter.TagRightChar);
                pWriter.Write(op.Operation);
                pWriter.WriteEndTag("td");

                for (int i = 0; i <= lLastState + 1; i++)
                {
                    string transString = TransitionStateAt(lOpTransitions, i, op);
                    if (!transString.Contains("F") && i == lLastState + 1 && !transString.Contains("U"))
                        pWriter.WriteBeginTag("td class=\"false\"");
                    else
                        pWriter.WriteBeginTag("td");
                    pWriter.Write(HtmlTextWriter.TagRightChar);

                    pWriter.Write(transString);
                    pWriter.WriteEndTag("td");

                }

                pWriter.WriteEndTag("tr");

            }
            pWriter.WriteEndTag("table");
            pWriter.WriteEndTag("div");
        }



        private string TransitionStateAt(List<String[]> pTransformations, int pState, OutputExp pExp)
        {
            string lCurrentState = pExp.OpState;

            try
            {
                foreach (String[] lTrans in pTransformations)
                {
                    int lTransState = Convert.ToInt32(lTrans[1]);
                    if (lTransState <= pState)
                    {
                        if (String.Equals(lTrans[2], "F"))

                            if (pState == lTransState)
                            {
                                return "<b> " + lTrans[2] + "</b>";
                            }
                            else
                                return lTrans[2];
                        else
                        {
                            lCurrentState = lTrans[2];
                        }

                        if (pState == lTransState)
                        {
                            lCurrentState = "<b> " + lCurrentState + "</b>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in TransitionStateAt in outputHandler");
                PrintMessageToConsole(ex.Message);
            }
            return lCurrentState;
        }


        //Returns all operation transformations
        //This function has an overload which follows
        private List<String[]> GetOpTransformations()
        {
            List<String[]> lTransforms = new List<String[]>();
            String[] lItem = new String[4];
            int lLastState = GetLastState();

            try
            {
                foreach (OutputExp lExp in OutputResult)
                {
                    OutputExp lNextOp = FindNextOp(lExp);
                    if (lNextOp != null)
                        if (String.Equals(lExp.Value, "true") && String.Equals(lNextOp.Value, "true"))
                        {
                            lItem = new String[4];
                            lItem[0] = lExp.Operation; //Name of operation
                            lItem[1] = lNextOp.State.ToString(); //State after finished transition
                            lItem[2] = lNextOp.OpState; //Operation status (I/E/F) after transition
                            lItem[3] = lNextOp.Variant.ToString(); //Operation variant
                            lTransforms.Add(lItem);
                        }
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in getOpTransformations in outputHandler");
                PrintMessageToConsole(ex.Message);
            }

            return lTransforms;
        }

        //Returns all operation transformations
        private List<String[]> GetOpTransformations(OutputExp pOperation)
        {
            List<String[]> lTransforms = new List<String[]>();
            String[] lItem = new String[4];
            int lLastState = GetLastState();

            try
            {
                foreach (OutputExp lExp in OutputResult)
                {
                    if (String.Equals(lExp.Operation, pOperation.Operation) &&
                        String.Equals(lExp.Variant, pOperation.Variant))
                    {
                        OutputExp lNextOp = FindNextOp(lExp);
                        if (lNextOp != null)
                            if (String.Equals(lExp.Value, "true") && String.Equals(lNextOp.Value, "true"))
                            {
                                lItem = new String[4];
                                lItem[0] = lExp.Operation; //Name of operation
                                lItem[1] = lNextOp.State.ToString(); //State after finished transition
                                lItem[2] = lNextOp.OpState; //Operation status (I/E/F) after transition
                                if (lNextOp.Variant != null)
                                    lItem[3] = lNextOp.Variant.ToString(); //Operation variant

                                lTransforms.Add(lItem);
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in getOpTransformations in outputHandler");
                PrintMessageToConsole(ex.Message);
            }

            return lTransforms;
        }

        //Print all operation transformations up to a state
        public void PrintOpTransformations(int pMax)
        {
            try
            {
                foreach (OutputExp lExp in OutputResult)
                {
                    OutputExp lNextOp = FindNextOp(lExp);
                    if (lNextOp != null && lNextOp.State <= pMax)
                        if (String.Equals(lExp.Value, "true") && String.Equals(lNextOp.Value, "true"))
                            PrintMessageToConsole(lExp.ToString() + " -> " + lNextOp.ToString());
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in printOpTransformations in outputHandler");
                PrintMessageToConsole(ex.Message);
            }
        }

        //Returns true is name is representing a goal selection varable
        private bool GoalState(string pName, string pState)
        {
            try
            {
                for (int i = 0; i <= int.Parse(pState); i++)
                {
                    if (String.Equals(pName, ("P" + i)))
                        return true;
                    else if (String.Equals(pName, ("F" + i)))
                        return true;
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in goalState in outputHandler");
                PrintMessageToConsole(ex.Message);
            }
            return false;

        }

        //Returns operation in next state and with the following operation status
        private OutputExp FindNextOp(OutputExp pFirst)
        {
            OutputExp lNext = null;

            try
            {
                if (pFirst.Name.Contains("_"))
                {
                    foreach (OutputExp exp in OutputResult)
                    {
                        if (exp.Name.Contains("_"))
                        {
                            if ((pFirst.State + 1) == exp.State &&
                                String.Equals(pFirst.Operation, exp.Operation) &&
                                String.Equals(NextOpState(pFirst.OpState), exp.OpState) &&
                                pFirst.Variant == exp.Variant &&
                                !String.Equals(pFirst.OpState, "U") &&
                                !String.Equals(pFirst.OpState, "F"))
                            {
                                lNext = exp;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in findNextOp in outputHandler");
                PrintMessageToConsole(ex.Message);
            }
            return lNext;
        }

        //Returns the following operation status
        private string NextOpState(string pState)
        {
            string lNext = null;

            switch (pState)
            {
                case "I": lNext = "E"; break;
                case "E": lNext = "F"; break;
                case "F": lNext = "F"; break;
                case "U": lNext = "U"; break;
            }
            return lNext;
        }

        //Prints false pre and post conditions for lstate
        private void PrintConditionsState(int pState)
        {
            try
            {
                foreach (OutputExp exp in OutputResult)
                {
                    if (exp.State == pState &&
                        (String.Equals(exp.OpState, "PostCondition") ||
                        (String.Equals(exp.OpState, "PreCondition"))) &&
                        String.Equals(exp.Value, "false"))
                        PrintMessageToConsole(exp.ToString());
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in printConditionState in outputHandler");
                PrintMessageToConsole(ex.Message);
            }
        }

        //Returns false pre and post conditions for lstate
        private List<String> GetConditionsState(int pState)
        {
            List<String> lCon = new List<String>();

            try
            {
                foreach (OutputExp lExp in OutputResult)
                {
                    if (lExp.State == pState &&
                        (String.Equals(lExp.OpState, "PostCondition") ||
                        (String.Equals(lExp.OpState, "PreCondition"))) &&
                        String.Equals(lExp.Value, "false"))
                        lCon.Add(lExp.ToString());
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in getConditionState in outputHandler");
                PrintMessageToConsole(ex.Message);
            }
            return lCon;
        }

        //Returns false pre and post conditions for lstate with the pre/post condition
        private HashSet<String[]> GetConditionsStateWithValues(int pState)
        {
            HashSet<String[]> lConditions = new HashSet<String[]>();
            List<string> lConValue = new List<string>();
            string[] lList;

            try
            {
                foreach (OutputExp lExp in OutputResult)
                {
                    if (lExp.State == pState &&
                        (String.Equals(lExp.OpState, "PostCondition") ||
                        (String.Equals(lExp.OpState, "PreCondition"))) &&
                        String.Equals(lExp.Value, "false"))
                    {
                        /*if (String.Equals(exp.opState, "PostCondition"))
                            conValue = fwrapper.getPostconditionForOperation(exp.operation);
                        else*/
                            lConValue = FrameworkWrapper.GetPreconditionForOperation(lExp.Operation);
                            lList = new string[3] { lExp.Operation + "_" + lExp.OpState, ConsToString(lConValue), lExp.Value };
                        lConditions.Add(lList);
                    }
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in getConditionStateWithValues in outputHandler");
                PrintMessageToConsole(ex.Message);
            }
            return lConditions;
        }

        //Prints operations in lstate
        private void PrintOpState(int pState)
        {
            try
            {
                foreach (OutputExp lExp in OutputResult)
                {
                    if (lExp.State == pState &&
                       !String.Equals(lExp.OpState, "U") &&
                       !String.Equals(lExp.OpState, "PostCondition") &&
                       !String.Equals(lExp.OpState, "PreCondition"))
                    {
                        PrintMessageToConsole(lExp.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in printOpState in outputHandler");
                PrintMessageToConsole(ex.Message);
            }
        }

        //Returns operations inlstate
        private List<String[]> GetOpState(int pState)
        {
            List<String[]> lOps = new List<String[]>();
            String[] lItem;

            try
            {
                foreach (OutputExp lExp in OutputResult)
                {
                    if (lExp.State == pState &&
                       !String.Equals(lExp.OpState, "U") &&
                       !String.Equals(lExp.OpState, "PostCondition") &&
                       !String.Equals(lExp.OpState, "PreCondition"))
                    {
                        lItem = new String[4];
                        lItem[0] = lExp.Operation;
                        lItem[1] = lExp.State.ToString();
                        lItem[2] = lExp.OpState;
                        lItem[3] = lExp.Value;
                        lOps.Add(lItem);
                    }
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in getOpState in outputHandler");
                PrintMessageToConsole(ex.Message);
            }
            return lOps;
        }

        private string ConsToString(List<string> pCons)
        {
            string lExp = "";

            try
            {
                if (pCons.Count > 0)
                {
                    string lTemp = pCons.First();
                    lExp = GeneralUtilities.ParseExpression(lTemp, "infix");
                    pCons.Remove(lTemp);
                    foreach (string con in pCons)
                    {
                        lExp = lExp + "and" + GeneralUtilities.ParseExpression(con, "infix");
                    }
                }

            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in conToString in outputHandler");
                PrintMessageToConsole(ex.Message);
            }

            return lExp;
        }

        //Returns operations inlstate
        private List<String> GetActiveOps()
        {
            List<String> lOps = new List<String>();

            try
            {
                foreach (OutputExp lExp in OutputResult)
                {
                    if (lExp.State == 0 &&
                       String.Equals(lExp.OpState, "I") &&
                       String.Equals(lExp.Value, "true") &&
                       !String.Equals(lExp.OpState, "PostCondition") &&
                       !String.Equals(lExp.OpState, "PreCondition"))
                    {
                        lOps.Add(lExp.Operation);
                    }
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in getActiveOps in outputHandler");
                PrintMessageToConsole(ex.Message);
            }
            return lOps;
        }

        //Returns all operations
        private List<OutputExp> GetAllOps()
        {
            List<OutputExp> lOps = new List<OutputExp>();

            try
            {
                foreach (OutputExp lExp in OutputResult)
                {
                    if (lExp.State == 0 &&
                       String.Equals(lExp.Value, "true") &&
                       !String.Equals(lExp.OpState, "E") &&
                       !String.Equals(lExp.OpState, "F") &&
                       !String.Equals(lExp.OpState, "PostCondition") &&
                       !String.Equals(lExp.OpState, "PreCondition"))
                    {
                        lOps.Add(lExp);
                    }
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in getActiveOps in outputHandler");
                PrintMessageToConsole(ex.Message);
            }
            return lOps;
        }

        //Returns the last state of analysis
        private int GetLastState()
        {
            int lLastState = 0;

            try
            {
                foreach (OutputExp lExp in OutputResult)
                {
                    if (lExp.State > lLastState)
                    {
                        lLastState = lExp.State;
                    }
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in getLastState in outputHandler");
                PrintMessageToConsole(ex.Message);
            }
            return lLastState - 1;
        }

        /*private string replaceVirtual(string p)
        {
            string newP = "";

            try
            {
                if (p.StartsWith("Virtual"))
                {
                    // RUNA code
                    //virtualConnection con = fwrapper.findVirtualConnectionWithName(p);
                    //foreach (Variantvar in con.getVariants())
                    //{
                    //    newP = newP + var.names + "<br>";
                    //}
                     
                    string virtualVariantExpression = cFrameworkWrapper.findVirtualVariantExpression(p);
                    newP = virtualVariantExpression + "<br>";
                    return newP;
                }
                    

            }
            catch (Exception ex)
            {
                printMessageToConsole("error in replaceVirtual in outputHandler");
                printMessageToConsole(ex.Message);
            }
            return p;

        }*/


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
                cOutputHandler.printMessageToConsole("error in modCondition in outputHandler");
                cOutputHandler.printMessageToConsole(ex.Message);
            }

            return newInfixExpr;
        }*/

        private bool CheckCondition(string pCon)
        {
            //For each condition first we have to build its coresponding tree
            Parser lConditionParser = new Parser();
            Node<string> lCnstExprTree = new Node<string>("root");

            try
            {
                lConditionParser.AddChild(lCnstExprTree, pCon);

                foreach (Node<string> lItem in lCnstExprTree)
                {
                    //Then we have to traverse the tree
                    return ParseConditionVirtual(lItem);
                }
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in checkCondition in outputHandler");
                PrintMessageToConsole(ex.Message);
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
                cOutputHandler.printMessageToConsole("error in ParseExpression in outputHandler");
                cOutputHandler.printMessageToConsole(ex.Message);
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
                PrintMessageToConsole("error in replaceVirtual in outputHandler");
                PrintMessageToConsole(ex.Message);

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
                cOutputHandler.printMessageToConsole("error in printStatus in outputHandler");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return null;
        }*/

        public void PrintMessageToConsole(string pMessage)
        {
            try
            {
                if (EnableUserMessages)               
                    Console.WriteLine(pMessage);
            }
            catch (Exception ex)
            {
                PrintMessageToConsole("error in printMessageToConsole");
                PrintMessageToConsole(ex.Message);
            }
        }
    }
}
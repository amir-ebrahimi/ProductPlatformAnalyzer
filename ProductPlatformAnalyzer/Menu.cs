using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Z3;
using System.Collections;
using System.IO;
using System.Xml;
using CAEX_ClassModel;
using AMLEngineExtensions;
using CAEX_ClassModel.Validation;
using System.Diagnostics;

namespace ProductPlatformAnalyzer
{
    public class Menu
    {
        private OutputHandler cOutputHandler;
        private Z3SolverEngineer cZ3SolverEngineer;

        public Menu()
        {

        }

        /// <summary>
        /// This is where the program starts
        /// </summary>
        /// <param name="args">This list can be used to input any wanted parameters to the program</param>
        static void Main(string[] args)
        {
            try
            {
                Menu lMenu = new Menu();
                lMenu.MainMenu();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        public void MainMenu()
        {
            try
            {
                bool lExitProgram = false;


                while (!lExitProgram)
                {
                    Console.Clear();
                    Console.WriteLine("Please pick one of the two options: ");
                    Console.WriteLine("1. Anlyze an external file");
                    Console.WriteLine("2. Analyzer Settings");
                    Console.WriteLine("3. Exit");

                    var lUserPick = Console.ReadLine();
                    switch (lUserPick)
                    {
                        case "1":
                            {
                                //FileAnalysis();
                                ExternalFileAnalysis();
                                break;
                            }
                        case "2":
                            AnalyzerSetting();
                            break;
                        case "3":
                            {
                                lExitProgram = true;
                                break;
                            }
                        default:
                            Console.WriteLine("Wrong Option! Please pick agian!");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in MainMenu");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void AnalyzerSetting()
        {
            try
            {
                var lReturnToMainMenu = false;
                while (!lReturnToMainMenu)
                {
                    DefaultAnalyzerSetting();
                    Console.Clear();
                    Console.WriteLine("1.Analysis Settings");
                    Console.WriteLine("2.Reporting Settings");
                    Console.WriteLine("3.Random File Creation Settings");
                    Console.WriteLine("4.Main Menu");
                    Console.WriteLine("Choose the category of the settings you want to change: ");
                    var lUserResponce = Console.ReadLine();

                    switch (lUserResponce)
                    {
                        case "1":
                            AnalysisSetting();
                            break;
                        case "2":
                            ReportingSetting();
                            break;
                        case "3":
                            RandomFileCreationSetting();
                            break;
                        case "4":
                            {
                                UpdateVariationPoints();
                                lReturnToMainMenu = true;
                                break;

                            }
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("AnalyzerSetting");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void UpdateVariationPoints()
        {
            try
            {
                SaveVariationPointsToXMLFile();
                LoadVariationPointsFromXMLFile();
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in UpdateVariationPoints");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void FileAnalysis()
        {
            try
            {
                bool lExitToMainMenu = false;

                while (!lExitToMainMenu)
                {

                    DefaultAnalyzerSetting();
                    Console.Clear();
                    Console.WriteLine("Please choose one of the following options:");
                    Console.WriteLine("1. Analyse an external file");
                    Console.WriteLine("2. Return to main menu");

                    var lUserAnalysisFileType = Console.ReadLine();

                    switch (lUserAnalysisFileType)
                    {
                        case "1":
                            ExternalFileAnalysis();
                            break;
                        case "2":
                            lExitToMainMenu = true;
                            break;
                        default:
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("FileAnalysis");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void DefaultAnalyzerSetting()
        {
            try
            {
                cZ3SolverEngineer = new Z3SolverEngineer();

                cOutputHandler = new OutputHandler();
                cZ3SolverEngineer.DefaultAnalyzerSetting(cOutputHandler);
                LoadVariationPointsFromXMLFile();
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in DefaultAnalyzerSetting");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void ExternalFileAnalysis()
        {
            try
            {
                bool lExitToMainMenu = false;
                string lFileName = "";
                string lPathPrefix = "";

                while (!lExitToMainMenu)
                {
                    var lCreateRandomFile = false;
                    DefaultAnalyzerSetting();
                    Console.Clear();
                    Console.WriteLine("Please enter in the file name to be analyzed: ");
                    lFileName = Console.ReadLine();

                    if (lFileName.Equals("random"))
                        lCreateRandomFile = true;

                    lPathPrefix = "../../Test/";

                    Console.Clear();

                    bool lAnotherAnalysis = true;
                    while (lAnotherAnalysis)
                    {
                        DefaultAnalyzerSetting();
                        cZ3SolverEngineer.RandomInputFile = lCreateRandomFile;

                        Console.Clear();
                        SetAnalysisType();

                        if (cZ3SolverEngineer.RandomInputFile)
                            cZ3SolverEngineer.ProductPlatformAnalysis("", "");
                        else
                            cZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + lFileName + ".xml");

                        Console.Clear();
                        Console.WriteLine("Please choose one of the following: ");
                        Console.WriteLine("1. Choose another analysis type");
                        Console.WriteLine("2. Choose another file for analysis");
                        Console.WriteLine("3. Return to main menu");
                        string lUserResponce = Console.ReadLine();

                        switch (lUserResponce)
                        {
                            case "1":
                                {
                                    lAnotherAnalysis = true;
                                    lExitToMainMenu = false;
                                    break;
                                }
                            case "3":
                                {
                                    lAnotherAnalysis = false;
                                    lExitToMainMenu = true;
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                    #region Previous-Version
                    //Insuficiant data -> No Analysis
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "0.0.0V0VG0O0C0P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "0.1.0V0VG1O0C0P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "0.2.1V0VG1O0C0P.xml");

                    //Different no of variant, different operation trigger
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "1.0.1V1VG2O0C0P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "1.1.2V1VG2O0C0P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "1.2.3V1VG2O0C0P.xml");

                    //Different variant group cardinality
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "2.0.3V1VG2O0C0P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "2.1.3V1VG2O0C0P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "2.2.3V1VG2O0C0P.xml");

                    //Configuration Constraint added
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "3.0.2V1VG2O1C0P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "3.1.2V1VG2O2C0P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "3.2.2V1VG2O1C0P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "3.3.2V1VG2O1C0P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "3.4.2V1VG2O1C0P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "3.5.2V1VG2O1C0P.xml");

                    //Operation precondition added
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "4.0.1V1VG2O0C1P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "4.1.1V1VG2O0C1P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "4.2.1V1VG2O0C1P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "4.3.1V1VG2O0C1P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "4.4.1V1VG3O0C1P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "4.6.1V1VG3O0C1P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "4.7.1V1VG5O0C4P.xml");

                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "5.0.4V2VG2O0C0P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "5.1.4V2VG2O0C0P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "5.2.4V2VG2O0C0P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "5.0.4V2VG2O0C0P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "5.0.4V2VG2O0C0P.xml");

                    //TODO: for a variant which is selectable what would be a good term for completing the analysis?
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "2.2V1VG2O1C1PNoTransitions-1UnselectV.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "3.2V1VG2O1C1PNoTransitions.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "4.2V1VG3O1C1PNoTransition.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "5.2V1VG2O1C1PNoTransition.xml");

                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "6.2V1VG2O0C0P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "7.2V1VG2O0C0P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "8.2V1VG2O0C0P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "9.0.2V1VG2O0C1P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "10.2V1VG2O0C1P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "11.2V1VG2O0C1P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "12.2V1VG2O0C2P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "13.2V1VG2O0C2P.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "14.2V1VG2O0C2P.xml");

                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "Case3Demo.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "Case3DemoV2.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "demo_variant.aml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "Volvo_CAB.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "Volvo_CAB_V2.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "Volvo_CAB_V3.xml");
                    //lZ3SolverEngineer.ProductPlatformAnalysis(lPathPrefix + "Volvo_CAB_V4.xml");

                    //Random data creation!
                    /*int lMaxVariantGroupumber = 2;
                    int lMaxVariantNumber = 4;
                    int lMaxPartNumber = 0;
                    int lMaxOperationNumber = 3;
                    int lTrueProbability = 100;
                    int lFalseProbability = 0;
                    int lExpressionProbability = 0;
                    int lMaxTraitNumber = 4;
                    int lMaxNoOfTraitAttributes = 3;
                    int lMaxResourceNumber = 2;
                    int lMaxExpressionOperandNumber = 3;
                    lZ3SolverEngineer.ProductPlatformAnalysis("", ""
                                                                , lMaxVariantGroupumber, lMaxVariantNumber, lMaxPartNumber
                                                                , lMaxOperationNumber, lTrueProbability, lFalseProbability
                                                                , lExpressionProbability, lMaxTraitNumber, lMaxNoOfTraitAttributes
                                                                , lMaxResourceNumber, lMaxExpressionOperandNumber);*/
                    #endregion
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in ExternalFileAnalysis");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        /// <summary>
        /// Saves the values of the variation points to external XML file
        /// </summary>
        public void SaveVariationPointsToXMLFile()
        {
            try
            {
                string newValue = string.Empty;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("..\\..\\VariationPoints.xml"); /// you must modify path with what you want
                /// 
                XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("//variationPoint");

                if (nodeList.Count > 0)
                {
                    foreach (XmlNode lXmlNode in nodeList)
                    {
                        string lVariationPointName = lXmlNode["name"].InnerText;
                        switch (lVariationPointName)
                        {
                            case "ReportAnalysisResult":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.ReportAnalysisResult.ToString();
                                break;
                            case "ReportAnalysisDetailResult":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.ReportAnalysisDetailResult.ToString();
                                break;
                            case "ReportVariantsResult":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.ReportVariantsResult.ToString();
                                break;
                            case "ReportTransitionsResult":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.ReportTransitionsResult.ToString();
                                break;
                            case "ReportAnalysisTiming":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.ReportAnalysisTiming.ToString();
                                break;
                            case "ReportUnsatCore":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.ReportUnsatCore.ToString();
                                break;
                            case "StopBetweenEachTransition":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.StopBetweenEachTransition.ToString();
                                break;
                            case "StopAEndOfAnalysis":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.StopAEndOfAnalysis.ToString();
                                break;
                            case "CreateHTMLOutput":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.CreateHTMLOutput.ToString();
                                break;
                            case "ReportTimings":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.ReportTimings.ToString();
                                break;
                            case "NoOfModelsRequired":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.NoOfModelsRequired.ToString();
                                break;
                            case "OperationWaiting":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.OperationWaiting.ToString();
                                break;
                            case "OperationMutualExecution":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.OperationMutualExecution.ToString();
                                break;
                            case "DebugMode":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.getDebugMode().ToString();
                                break;
                            case "UserMessages":
                                lXmlNode["value"].InnerText = cOutputHandler.getEnableUserMessages().ToString();
                                break;
                            case "RandomMaxNoOfConfigurationRules":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.RandomMaxNoOfConfigurationRules.ToString();
                                break;
                            //1.RandomMaxVariantGroupumber
                            case "RandomMaxVariantGroupumber":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.RandomMaxVariantGroupumber.ToString();
                                break;
                            //2.RandomMaxVariantNumber
                            case "RandomMaxVariantNumber":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.RandomMaxVariantNumber.ToString();
                                break;
                            //3.RandomMaxPartNumber
                            case "RandomMaxPartNumber":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.RandomMaxPartNumber.ToString();
                                break;
                            //4.RandomMaxOperationNumber
                            case "RandomMaxOperationNumber":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.RandomMaxOperationNumber.ToString();
                                break;
                            //5.RandomTrueProbability
                            case "RandomTrueProbability":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.RandomTrueProbability.ToString();
                                break;
                            //6.RandomFalseProbability
                            case "RandomFalseProbability":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.RandomFalseProbability.ToString();
                                break;
                            //7.RandomExpressionProbability
                            case "RandomExpressionProbability":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.RandomExpressionProbability.ToString();
                                break;
                            //8.RandomMaxTraitNumber
                            case "RandomMaxTraitNumber":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.RandomMaxTraitNumber.ToString();
                                break;
                            //9.RandomMaxNoOfTraitAttributes
                            case "RandomMaxNoOfTraitAttributes":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.RandomMaxNoOfTraitAttributes.ToString();
                                break;
                            //10.RandomMaxResourceNumber
                            case "RandomMaxResourceNumber":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.RandomMaxResourceNumber.ToString();
                                break;
                            //11.RandomMaxExpressionOperandNumber
                            case "RandomMaxExpressionOperandNumber":
                                lXmlNode["value"].InnerText = cZ3SolverEngineer.RandomMaxExpressionOperandNumber.ToString();
                                break;
                            default:
                                break;
                        }
                    }
                }

                xmlDoc.Save("..\\..\\VariationPoints.xml"); /// you must modify path with what you want
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in SaveVariationPointsToXMLFile");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        /// <summary>
        /// This function reads the variation points values from the external file
        /// </summary>
        public void LoadVariationPointsFromXMLFile()
        {
            try
            {
                string lFileName = "..\\..\\VariationPoints.xml";
                //new instance of xdoc
                XmlDocument xDoc = new XmlDocument();

                //First load the XML file from the file path
                xDoc.Load(lFileName);

                XmlNodeList nodeList = xDoc.DocumentElement.SelectNodes("//variationPoint");

                if (nodeList.Count > 0)
                {
                    foreach (XmlNode lXmlNode in nodeList)
                    {
                        string lVariationPointName = lXmlNode["name"].InnerText;
                        switch (lVariationPointName)
                        {
                            case "ReportAnalysisResult":
                                cZ3SolverEngineer.ReportAnalysisResult = bool.Parse(lXmlNode["value"].InnerText);
                                break;
                            case "ReportAnalysisDetailResult":
                                cZ3SolverEngineer.ReportAnalysisDetailResult = bool.Parse(lXmlNode["value"].InnerText);
                                break;
                            case "ReportVariantsResult":
                                cZ3SolverEngineer.ReportVariantsResult = bool.Parse(lXmlNode["value"].InnerText);
                                break;
                            case "ReportTransitionsResult":
                                cZ3SolverEngineer.ReportTransitionsResult = bool.Parse(lXmlNode["value"].InnerText);
                                break;
                            case "ReportAnalysisTiming":
                                cZ3SolverEngineer.ReportAnalysisTiming = bool.Parse(lXmlNode["value"].InnerText);
                                break;
                            case "ReportUnsatCore":
                                cZ3SolverEngineer.ReportUnsatCore = bool.Parse(lXmlNode["value"].InnerText);
                                break;
                            case "StopBetweenEachTransition":
                                cZ3SolverEngineer.StopBetweenEachTransition = bool.Parse(lXmlNode["value"].InnerText);
                                break;
                            case "StopAEndOfAnalysis":
                                cZ3SolverEngineer.StopAEndOfAnalysis = bool.Parse(lXmlNode["value"].InnerText);
                                break;
                            case "CreateHTMLOutput":
                                cZ3SolverEngineer.CreateHTMLOutput = bool.Parse(lXmlNode["value"].InnerText);
                                break;
                            case "ReportTimings":
                                cZ3SolverEngineer.ReportTimings = bool.Parse(lXmlNode["value"].InnerText);
                                break;
                            case "NoOfModelsRequired":
                                cZ3SolverEngineer.NoOfModelsRequired = int.Parse(lXmlNode["value"].InnerText);
                                break;
                            case "OperationWaiting":
                                cZ3SolverEngineer.OperationWaiting = bool.Parse(lXmlNode["value"].InnerText);
                                break;
                            case "OperationMutualExecution":
                                cZ3SolverEngineer.OperationMutualExecution = bool.Parse(lXmlNode["value"].InnerText);
                                break;
                            case "DebugMode":
                                cZ3SolverEngineer.setDebugMode(bool.Parse(lXmlNode["value"].InnerText));
                                break;
                            case "UserMessages":
                                cOutputHandler.setEnableUserMessages(bool.Parse(lXmlNode["value"].InnerText));
                                break;
                            case "RandomMaxNoOfConfigurationRules":
                                cZ3SolverEngineer.RandomMaxNoOfConfigurationRules = int.Parse(lXmlNode["value"].InnerText);
                                break;
                            //1.RandomMaxVariantGroupumber
                            case "RandomMaxVariantGroupumber":
                                cZ3SolverEngineer.RandomMaxVariantGroupumber = int.Parse(lXmlNode["value"].InnerText);
                                break;
                            //2.RandomMaxVariantNumber
                            case "RandomMaxVariantNumber":
                                cZ3SolverEngineer.RandomMaxVariantNumber = int.Parse(lXmlNode["value"].InnerText);
                                break;
                            //3.RandomMaxPartNumber
                            case "RandomMaxPartNumber":
                                cZ3SolverEngineer.RandomMaxPartNumber = int.Parse(lXmlNode["value"].InnerText);
                                break;
                            //4.RandomMaxOperationNumber
                            case "RandomMaxOperationNumber":
                                cZ3SolverEngineer.RandomMaxOperationNumber = int.Parse(lXmlNode["value"].InnerText);
                                break;
                            //5.RandomTrueProbability
                            case "RandomTrueProbability":
                                cZ3SolverEngineer.RandomTrueProbability = int.Parse(lXmlNode["value"].InnerText);
                                break;
                            //6.RandomFalseProbability
                            case "RandomFalseProbability":
                                cZ3SolverEngineer.RandomFalseProbability = int.Parse(lXmlNode["value"].InnerText);
                                break;
                            //7.RandomExpressionProbability
                            case "RandomExpressionProbability":
                                cZ3SolverEngineer.RandomExpressionProbability = int.Parse(lXmlNode["value"].InnerText);
                                break;
                            //8.RandomMaxTraitNumber
                            case "RandomMaxTraitNumber":
                                cZ3SolverEngineer.RandomMaxTraitNumber = int.Parse(lXmlNode["value"].InnerText);
                                break;
                            //9.RandomMaxNoOfTraitAttributes
                            case "RandomMaxNoOfTraitAttributes":
                                cZ3SolverEngineer.RandomMaxNoOfTraitAttributes = int.Parse(lXmlNode["value"].InnerText);
                                break;
                            //10.RandomMaxResourceNumber
                            case "RandomMaxResourceNumber":
                                cZ3SolverEngineer.RandomMaxResourceNumber = int.Parse(lXmlNode["value"].InnerText);
                                break;
                            //11.RandomMaxExpressionOperandNumber
                            case "RandomMaxExpressionOperandNumber":
                                cZ3SolverEngineer.RandomMaxExpressionOperandNumber = int.Parse(lXmlNode["value"].InnerText);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in LoadVariationPointsFromXMLFile");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void SetAnalysisType()
        {
            //bool lResultChooseAnotherFile = false;
            try
            {
                Console.WriteLine("1. Product enumeration analysis");
                Console.WriteLine("2. Product manufacturing enumeration analysis");
                Console.WriteLine("3. Is there any variant(s) that are ALWAYS selected for a configuration?");
                Console.WriteLine("4. Is there any variant(s) that are NEVER selected for a configuration?");
                Console.WriteLine("5. Is there any variant(s) which is possible to choose for any configuration?");
                Console.WriteLine("6. Is there any part(s) that are ALWAYS selected for a configuration?");
                Console.WriteLine("7. Is there any part(s) that are NEVER selected for a configuration?");
                Console.WriteLine("8. Is there any part(s) which will not be possible to choose for any configuration?");
                Console.WriteLine("9. Is there any operation(s) that are ALWAYS selected for a configuration?");
                Console.WriteLine("10. Is there any operation(s) that are NEVER selected for a configuration?");
                Console.WriteLine("11. Is there any operation(s) which is possible to choose for a configuration?");
                Console.WriteLine("12. Existence of deadlock analysis");
                //Console.WriteLine("0. Choose another file to analyze.");
                Console.WriteLine("Which analysis do you want to perform: ");
                var lAnalysisType = Console.ReadLine();

                switch (lAnalysisType)
                {
                    /*case "0":
                        lResultChooseAnotherFile = true;
                        break;*/
                    case "1":
                        //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                        //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                        //             , Build P Constraints, Number Of Models Required
                        cZ3SolverEngineer.setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                                            , Enumerations.AnalysisType.ProductModelEnumerationAnalysis);
                        break;
                    case "2":
                        //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                        //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                        //             , Build P Constraints, Number Of Models Required
                        cZ3SolverEngineer.setVariationPoints(Enumerations.GeneralAnalysisType.Dynamic
                                                            , Enumerations.AnalysisType.ProductManufacturingModelEnumerationAnalysis);
                        break;
                    case "3":
                        //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                        //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                        //             , Build P Constraints, Number Of Models Required
                        cZ3SolverEngineer.setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                                            , Enumerations.AnalysisType.AlwaysSelectedVariantAnalysis);
                        break;
                    case "4":
                        //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                        //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                        //             , Build P Constraints, Number Of Models Required
                        cZ3SolverEngineer.setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                                            , Enumerations.AnalysisType.NeverSelectedVariantAnalysis);
                        break;
                    case "5":
                        //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                        //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                        //             , Build P Constraints, Number Of Models Required
                        cZ3SolverEngineer.setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                                            , Enumerations.AnalysisType.VariantSelectabilityAnalysis);
                        break;
                    case "6":
                        //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                        //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                        //             , Build P Constraints, Number Of Models Required
                        cZ3SolverEngineer.setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                                            , Enumerations.AnalysisType.AlwaysSelectedPartAnalysis);
                        break;
                    case "7":
                        //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                        //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                        //             , Build P Constraints, Number Of Models Required
                        cZ3SolverEngineer.setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                                            , Enumerations.AnalysisType.NeverSelectedPartAnalysis);
                        break;
                    case "8":
                        //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                        //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                        //             , Build P Constraints, Number Of Models Required
                        cZ3SolverEngineer.setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                                            , Enumerations.AnalysisType.PartSelectabilityAnalysis);
                        break;
                    case "9":
                        //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                        //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                        //             , Build P Constraints, Number Of Models Required
                        cZ3SolverEngineer.setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                                            , Enumerations.AnalysisType.AlwaysSelectedOperationAnalysis);
                        break;
                    case "10":
                        //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                        //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                        //             , Build P Constraints, Number Of Models Required
                        cZ3SolverEngineer.setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                                            , Enumerations.AnalysisType.NeverSelectedOperationAnalysis);
                        break;
                    case "11":
                        //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                        //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                        //             , Build P Constraints, Number Of Models Required
                        cZ3SolverEngineer.setVariationPoints(Enumerations.GeneralAnalysisType.Static
                                                            , Enumerations.AnalysisType.OperationSelectabilityAnalysis);
                        break;
                    case "12":
                        //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                        //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                        //             , Build P Constraints, Number Of Models Required
                        cZ3SolverEngineer.setVariationPoints(Enumerations.GeneralAnalysisType.Dynamic
                                                            , Enumerations.AnalysisType.ExistanceOfDeadlockAnalysis);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("SetAnalysisType");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            //return lResultChooseAnotherFile;
        }

        public void ReportingSetting()
        {
            try
            {
                //Parameters: Analysis Result, Analysis Detail Result, Variants Result
                //          , Transitions Result, Analysis Timing, Unsat Core
                //          , Stop between each transition, Stop at end of analysis, Create HTML Output
                //          , Report timings, Debug Mode (Make model file), User Messages
                bool lReturnToMainMenu = false;
                while (!lReturnToMainMenu)
                {
                    Console.Clear();
                    //1.Analysis Result
                    Console.WriteLine("1.Analysis Result: " + cZ3SolverEngineer.ReportAnalysisResult.ToString());
                    //2.Analysis Detail Result
                    Console.WriteLine("2.Analysis Detail Result: " + cZ3SolverEngineer.ReportAnalysisDetailResult.ToString());
                    //3.Variants Result
                    Console.WriteLine("3.Variants Result: " + cZ3SolverEngineer.ReportVariantsResult.ToString());
                    //4.Transitions Result
                    Console.WriteLine("4.Transitions Result: " + cZ3SolverEngineer.ReportTransitionsResult.ToString());
                    //5.Analysis Timing
                    Console.WriteLine("5.Analysis Timing: " + cZ3SolverEngineer.ReportAnalysisTiming.ToString());
                    //6.Unsat Core
                    Console.WriteLine("6.Unsat Core: " + cZ3SolverEngineer.ReportUnsatCore.ToString());
                    //7.Stop between each transition
                    Console.WriteLine("7.Stop between each transition: " + cZ3SolverEngineer.StopBetweenEachTransition.ToString());
                    //8.Stop at end of analysis
                    Console.WriteLine("8.Stop at end of analysis: " + cZ3SolverEngineer.StopAEndOfAnalysis.ToString());
                    //9.Create HTML Output
                    Console.WriteLine("9.Create HTML Output: " + cZ3SolverEngineer.CreateHTMLOutput.ToString());
                    //10.Report timings
                    Console.WriteLine("10.Report timings: " + cZ3SolverEngineer.ReportTimings.ToString());
                    //11.Debug Mode (Make model file)
                    Console.WriteLine("11.Debug Mode (Make model file): " + cZ3SolverEngineer.getDebugMode().ToString());
                    //12.User Messages
                    Console.WriteLine("12.User Messages: " + cOutputHandler.getEnableUserMessages().ToString());

                    Console.WriteLine("13.Return to main menu.");

                    Console.WriteLine("Which setting do you want to change? ");
                    var lSettingToChange = Console.ReadLine();

                    switch (lSettingToChange)
                    {
                        case "1":
                            cZ3SolverEngineer.ReportAnalysisResult = ChangeSetting(cZ3SolverEngineer.ReportAnalysisResult);
                            break;
                        case "2":
                            cZ3SolverEngineer.ReportAnalysisDetailResult = ChangeSetting(cZ3SolverEngineer.ReportAnalysisDetailResult);
                            break;
                        case "3":
                            cZ3SolverEngineer.ReportVariantsResult = ChangeSetting(cZ3SolverEngineer.ReportVariantsResult);
                            break;
                        case "4":
                            cZ3SolverEngineer.ReportTransitionsResult = ChangeSetting(cZ3SolverEngineer.ReportTransitionsResult);
                            break;
                        case "5":
                            cZ3SolverEngineer.ReportAnalysisTiming = ChangeSetting(cZ3SolverEngineer.ReportAnalysisTiming);
                            break;
                        case "6":
                            cZ3SolverEngineer.ReportUnsatCore = ChangeSetting(cZ3SolverEngineer.ReportUnsatCore);
                            break;
                        case "7":
                            cZ3SolverEngineer.StopBetweenEachTransition = ChangeSetting(cZ3SolverEngineer.StopBetweenEachTransition);
                            break;
                        case "8":
                            cZ3SolverEngineer.StopAEndOfAnalysis = ChangeSetting(cZ3SolverEngineer.StopAEndOfAnalysis);
                            break;
                        case "9":
                            cZ3SolverEngineer.CreateHTMLOutput = ChangeSetting(cZ3SolverEngineer.CreateHTMLOutput);
                            break;
                        case "10":
                            cZ3SolverEngineer.ReportTimings = ChangeSetting(cZ3SolverEngineer.ReportTimings);
                            break;
                        case "11":
                            cZ3SolverEngineer.setDebugMode(ChangeSetting(cZ3SolverEngineer.getDebugMode()));
                            break;
                        case "12":
                            {
                                var lResultValue = ChangeSetting(cOutputHandler.getEnableUserMessages());
                                cOutputHandler.setEnableUserMessages(lResultValue);
                                break;
                            }
                        case "13":
                            lReturnToMainMenu = true;
                            break;
                        default:
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("ReportingSetting");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        public void AnalysisSetting()
        {
            try
            {
                bool lReturnToMainMenu = false;
                while (!lReturnToMainMenu)
                {
                    Console.Clear();
                    //1.Convert variants
                    Console.WriteLine("1.Convert variants: " + cZ3SolverEngineer.ConvertVariants.ToString());
                    //2.Convert configuration rules
                    Console.WriteLine("2.Convert configuration rules: " + cZ3SolverEngineer.ConvertConfigurationRules.ToString());
                    //3.Convert operations
                    Console.WriteLine("3.Convert operations: " + cZ3SolverEngineer.ConvertOperations.ToString());
                    //4.Convert operation precedence rules
                    Console.WriteLine("4.Convert operation precedence rules: " + cZ3SolverEngineer.ConvertOperationPrecedenceRules.ToString());
                    //5.Convert resources
                    Console.WriteLine("5.Convert resources: " + cZ3SolverEngineer.ConvertResources.ToString());
                    //6.Convert goals
                    Console.WriteLine("6.Convert goals: " + cZ3SolverEngineer.ConvertGoal.ToString());
                    //7.Build P Constraints
                    Console.WriteLine("7.Build P Constraints: " + cZ3SolverEngineer.BuildPConstraints.ToString());
                    //8.Number Of Models Required
                    Console.WriteLine("8.Number Of Models Required: " + cZ3SolverEngineer.NoOfModelsRequired.ToString());

                    Console.WriteLine("9.Return to main menu.");

                    Console.WriteLine("Which setting do you want to change? ");
                    var lSettingToChange = Console.ReadLine();

                    switch (lSettingToChange)
                    {
                        case "1":
                            cZ3SolverEngineer.ConvertVariants = ChangeSetting(cZ3SolverEngineer.ConvertVariants);
                            break;
                        case "2":
                            cZ3SolverEngineer.ConvertConfigurationRules = ChangeSetting(cZ3SolverEngineer.ConvertConfigurationRules);
                            break;
                        case "3":
                            cZ3SolverEngineer.ConvertOperations = ChangeSetting(cZ3SolverEngineer.ConvertOperations);
                            break;
                        case "4":
                            cZ3SolverEngineer.ConvertOperationPrecedenceRules = ChangeSetting(cZ3SolverEngineer.ConvertOperationPrecedenceRules);
                            break;
                        case "5":
                            cZ3SolverEngineer.ConvertResources = ChangeSetting(cZ3SolverEngineer.ConvertResources);
                            break;
                        case "6":
                            cZ3SolverEngineer.ConvertGoal = ChangeSetting(cZ3SolverEngineer.ConvertGoal);
                            break;
                        case "7":
                            cZ3SolverEngineer.BuildPConstraints = ChangeSetting(cZ3SolverEngineer.BuildPConstraints);
                            break;
                        case "8":
                            {
                                Console.WriteLine("How many models do you need to be created?");
                                cZ3SolverEngineer.NoOfModelsRequired = int.Parse(Console.ReadLine());

                                break;
                            }
                        case "9":
                            lReturnToMainMenu = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("AnalysisSetting");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

        private bool ChangeSetting(bool pSetting)
        {
            bool lResultSettingValue = false;
            try
            {
                if (pSetting)
                    lResultSettingValue = false;
                else
                    lResultSettingValue = true;

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("ChangeSetting");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
            return lResultSettingValue;
        }

        public void RandomFileCreationSetting()
        {
            try
            {
                bool lReturnToMainMenu = false;
                while (!lReturnToMainMenu)
                {
                    Console.Clear();
                    //1.RandomMaxVariantGroupumber
                    Console.WriteLine("1.RandomMaxVariantGroupumber: " + cZ3SolverEngineer.RandomMaxVariantGroupumber.ToString());
                    //2.RandomMaxVariantNumber
                    Console.WriteLine("2.RandomMaxVariantNumber: " + cZ3SolverEngineer.RandomMaxVariantNumber.ToString());
                    //3.RandomMaxPartNumber
                    Console.WriteLine("3.RandomMaxPartNumber: " + cZ3SolverEngineer.RandomMaxPartNumber.ToString());
                    //4.RandomMaxOperationNumber
                    Console.WriteLine("4.RandomMaxOperationNumber: " + cZ3SolverEngineer.RandomMaxOperationNumber.ToString());
                    //5.RandomMaxNoOfConfigurationRules
                    Console.WriteLine("5.RandomMaxNoOfConfigurationRules: " + cZ3SolverEngineer.RandomMaxNoOfConfigurationRules.ToString());
                    //6.RandomTrueProbability
                    Console.WriteLine("6.RandomTrueProbability: " + cZ3SolverEngineer.RandomTrueProbability.ToString());
                    //7.RandomFalseProbability
                    Console.WriteLine("7.RandomFalseProbability: " + cZ3SolverEngineer.RandomFalseProbability.ToString());
                    //8.RandomExpressionProbability
                    Console.WriteLine("8.RandomExpressionProbability: " + cZ3SolverEngineer.RandomExpressionProbability.ToString());
                    //9.RandomMaxTraitNumber
                    Console.WriteLine("9.RandomMaxTraitNumber: " + cZ3SolverEngineer.RandomMaxTraitNumber.ToString());
                    //10.RandomMaxNoOfTraitAttributes
                    Console.WriteLine("10.RandomMaxNoOfTraitAttributes: " + cZ3SolverEngineer.RandomMaxNoOfTraitAttributes.ToString());
                    //11.RandomMaxResourceNumber
                    Console.WriteLine("11.RandomMaxResourceNumber: " + cZ3SolverEngineer.RandomMaxResourceNumber.ToString());
                    //12.RandomMaxExpressionOperandNumber
                    Console.WriteLine("12.RandomMaxExpressionOperandNumber: " + cZ3SolverEngineer.RandomMaxExpressionOperandNumber.ToString());

                    Console.WriteLine("13.Return to main menu.");

                    Console.WriteLine("Which setting do you want to change? ");
                    var lSettingToChange = Console.ReadLine();

                    switch (lSettingToChange)
                    {
                        case "1":
                            {
                                Console.WriteLine("What is the new value?");
                                cZ3SolverEngineer.RandomMaxVariantGroupumber = int.Parse(Console.ReadLine());

                                break;
                            }
                        case "2":
                            {
                                Console.WriteLine("What is the new value?");
                                cZ3SolverEngineer.RandomMaxVariantNumber = int.Parse(Console.ReadLine());

                                break;
                            }
                        case "3":
                            {
                                Console.WriteLine("What is the new value?");
                                cZ3SolverEngineer.RandomMaxPartNumber = int.Parse(Console.ReadLine());

                                break;
                            }
                        case "4":
                            {
                                Console.WriteLine("What is the new value?");
                                cZ3SolverEngineer.RandomMaxOperationNumber = int.Parse(Console.ReadLine());

                                break;
                            }
                        case "5":
                            {
                                Console.WriteLine("What is the new value?");
                                cZ3SolverEngineer.RandomMaxNoOfConfigurationRules = int.Parse(Console.ReadLine());

                                break;
                            }
                        case "6":
                            {
                                Console.WriteLine("What is the new value?");
                                cZ3SolverEngineer.RandomTrueProbability = int.Parse(Console.ReadLine());

                                break;
                            }
                        case "7":
                            {
                                Console.WriteLine("What is the new value?");
                                cZ3SolverEngineer.RandomFalseProbability = int.Parse(Console.ReadLine());

                                break;
                            }
                        case "8":
                            {
                                Console.WriteLine("What is the new value?");
                                cZ3SolverEngineer.RandomExpressionProbability = int.Parse(Console.ReadLine());

                                break;
                            }
                        case "9":
                            {
                                Console.WriteLine("What is the new value?");
                                cZ3SolverEngineer.RandomMaxTraitNumber = int.Parse(Console.ReadLine());

                                break;
                            }
                        case "10":
                            {
                                Console.WriteLine("What is the new value?");
                                cZ3SolverEngineer.RandomMaxNoOfTraitAttributes = int.Parse(Console.ReadLine());

                                break;
                            }
                        case "11":
                            {
                                Console.WriteLine("What is the new value?");
                                cZ3SolverEngineer.RandomMaxResourceNumber = int.Parse(Console.ReadLine());

                                break;
                            }
                        case "12":
                            {
                                Console.WriteLine("What is the new value?");
                                cZ3SolverEngineer.RandomMaxExpressionOperandNumber = int.Parse(Console.ReadLine());

                                break;
                            }
                        case "13":
                            lReturnToMainMenu = true;
                            break;
                        default:
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                cOutputHandler.printMessageToConsole("error in RandomFileCreationSetting");
                cOutputHandler.printMessageToConsole(ex.Message);
            }
        }

    
    }
}

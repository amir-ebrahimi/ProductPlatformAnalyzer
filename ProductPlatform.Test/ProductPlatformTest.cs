using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductPlatformAnalyzer;
using System.Collections.Generic;
using NUnit.Framework;
using System.IO;

namespace ProductPlatform.Test
{
    //public class TestOperationsData
    //{
    //    public string lTestFile;
    //    public bool lOperationDependencyResult;

    //    public TestOperationsData(string pTestFile, bool pOperationDependencyResult)
    //    {
    //        lTestFile = pTestFile;
    //        lOperationDependencyResult = pOperationDependencyResult;
    //    }
    //}

    //public class TestData
    //{
    //    public string TestFile { get; set; }
    //    public ProductPlatformAnalyzer.Enumerations.GeneralAnalysisType GeneralAnalysisType { get; set; }
    //    public ProductPlatformAnalyzer.Enumerations.AnalysisType AnalysisType { get; set; }
    //    //public bool LoadDataExpectedResult { get; set; }
    //    public int NoOfModelsRequired { get; set; }
    //    //public bool VariantSelectabilityExpectedResult { get; set; }
    //    //public bool OperationSelectabilityExpectedResult { get; set; }
    //    //public bool AlwaysSelectedVariantExpectedResult { get; set; }
    //    //public bool AlwaysSlectedOperationExpectedResult { get; set; }
    //    //public bool DeadlockDetectionExpectedResult { get; set; }
    //    public int AnalysisNoOfCycles { get; set; }

    //    public TestData(string pTestFile
    //                    //, bool pLoadDataExpectedResult
    //                    //, bool pVariantSelectabilityExpectedResult
    //                    //, bool pOperationSelectabilityExpectedResult
    //                    //, bool pAlwaysSelectedVariantExpectedResult
    //                    //, bool pAlwaysSlectedOperationExpectedResult
    //                    //, bool pDeadlockDetectionExpectedResult
    //                    , int pAnalysisNoOfCycles
    //                    , int pNoOfModelsRequired = 4)
    //    {

    //        TestFile = pTestFile;
    //        //LoadDataExpectedResult = pLoadDataExpectedResult;
    //        NoOfModelsRequired = pNoOfModelsRequired;
    //        //VariantSelectabilityExpectedResult = pVariantSelectabilityExpectedResult;
    //        //OperationSelectabilityExpectedResult = pOperationSelectabilityExpectedResult;
    //        //AlwaysSelectedVariantExpectedResult = pAlwaysSelectedVariantExpectedResult;
    //        //AlwaysSlectedOperationExpectedResult = pAlwaysSlectedOperationExpectedResult;
    //        //DeadlockDetectionExpectedResult = pDeadlockDetectionExpectedResult;
    //        AnalysisNoOfCycles = pAnalysisNoOfCycles;
    //    }
    //}

    [TestFixture]
    public class ProductPlatformTest
    {

        //private List<TestData> lTestDataList = new List<TestData>();
        //private List<TestOperationsData> lTestOperationList = new List<TestOperationsData>();
        private const string FileDirectory = "..\\..\\..\\ProductPlatformAnalyzer\\Test\\";
        private string TestFilePath;
        private Z3SolverEngineer _z3SolverEngineer = new Z3SolverEngineer();
        private OutputHandler _outputHandler = new OutputHandler();

        [SetUp]
        public void Setup()
        {
            //var dir = Path.GetDirectoryName(typeof(MySetUpClass).Assembly.Location);
            //Environment.CurrentDirectory = dir;
            var dir = TestContext.CurrentContext.TestDirectory;
            // or
            Directory.SetCurrentDirectory(dir);

            _z3SolverEngineer.DefaultAnalyzerSetting(_outputHandler);
            //LoadVariationPointsFromXMLFile();

        }

        public ProductPlatformTest()
        {

            //Parameter list: Data file name, load data result, Variant Selectability Result, Operation Selectability Result
            //              , Always Selected Variant Result, Always Selected Operation Result, Deadlock Detection Result
            //var lFileDirectory = "..\\..\\..\\ProductPlatformAnalyzer\\Test\\";

            //lTestOperationList.Add(new TestOperationsData(FileDirectory + "OperationDependencyAnalysisTest1.xml", false));

            //lTestDataList.Add(new TestData(lFileDirectory + "0.0.xml", false, false, false, false, false, false));
            //lTestDataList.Add(new TestData(lFileDirectory + "0.1.xml", false, false, false, false, false, false));
            //lTestDataList.Add(new TestData(lFileDirectory + "0.2.xml", false, false, false, false, false, false));
            /*
            lTestDataList.Add(new TestData("1.0.1V1VG2O0C0P.xml", true, true, true, false, false, false));
            lTestDataList.Add(new TestData("1.1.2V1VG2O0C0P.xml", true, true, true, true, false, false));
            lTestDataList.Add(new TestData("1.2.3V1VG2O0C0P.xml", true, true, true, true, false, false));

            lTestDataList.Add(new TestData("2.0.3V1VG2O0C0P.xml", true, true, true, true, false, false));
            lTestDataList.Add(new TestData("2.1.3V1VG2O0C0P.xml", true, true, true, true, false, false));
            lTestDataList.Add(new TestData("2.2.3V1VG2O0C0P.xml", true, true, true, false, false, false));
            
            lTestDataList.Add(new TestData("4.0.3V2VG2O0C0P.xml", false, false, false, false, false, false));
            lTestDataList.Add(new TestData("4.1.3V2VG2O0C0P.xml", true, true, true, true, false, false));

            lTestDataList.Add(new TestData("5.0.4V2VG2O0C0P.xml", true, true, true, true, false, false));
            lTestDataList.Add(new TestData("5.1.4V2VG2O0C0P.xml", true, true, true, true, false, false));
            lTestDataList.Add(new TestData("5.2.4V2VG2O0C0P.xml", true, true, true, true, false, false));
            lTestDataList.Add(new TestData("5.3.4V2VG2O0C0P.xml", true, true, true, true, false, false));
            lTestDataList.Add(new TestData("5.4.4V2VG2O0C0P.xml", true, true, true, false, false, false));
            lTestDataList.Add(new TestData("5.5.4V2VG2O0C0P.xml", true, true, true, true, false, false));
            lTestDataList.Add(new TestData("2.2V1VG2O1C1PNoTransitions-1UnselectV.xml", true, false, true, true, true));
            lTestDataList.Add(new TestData("3.2V1VG2O1C1PNoTransitions.xml", true, true, true, true, false));

            lTestDataList.Add(new TestData("4.2V1VG3O1C1PNoTransition.xml", false, false, false, false, false));
            lTestDataList.Add(new TestData("5.2V1VG2O1C1PNoTransition.xml", false, false, false, false, false));

            lTestDataList.Add(new TestData("6.2V1VG2O0C0P.xml", true, true, true, true, true, false));
            lTestDataList.Add(new TestData("7.2V1VG2O0C0P.xml", true, true, true, true, true));
            lTestDataList.Add(new TestData("8.2V1VG2O0C0P.xml", true, true, true, false, false));
            lTestDataList.Add(new TestData("9.2V1VG2O0C1P.xml", true, true, true, true, true));
            lTestDataList.Add(new TestData("10.2V1VG2O0C1P.xml", true, true, true, true, true));
            lTestDataList.Add(new TestData("11.2V1VG2O0C1P.xml", true, true, true, false, false));
            lTestDataList.Add(new TestData("12.2V1VG2O0C2P.xml", true, true, true, true, true));
            lTestDataList.Add(new TestData("13.2V1VG2O0C2P.xml", true, true, true, true, true));
            lTestDataList.Add(new TestData("14.2V1VG2O0C2P.xml", true, true, true, false, false));
            */
            //lTestDataList.Add(new TestData(FileDirectory + "OperationsNoDeadlock.xml", true, false, false, false, false, false, 6)); //Case 1
            //lTestDataList.Add(new TestData(FileDirectory + "OperationsNoDeadlock2.xml", true, false, false, false, false, false, 6)); //Three operations in sequence
            //lTestDataList.Add(new TestData(FileDirectory + "OperationsDeadlock.xml", true, false, false, false, false, true, 6)); //Case 2
            //lTestDataList.Add(new TestData(FileDirectory + "OperationsNResourcesNoDeadlock.xml", true, false, false, false, false, false, 6)); //Case 2
            //lTestDataList.Add(new TestData(FileDirectory + "OperationsNResourcesNoDeadlock2.xml", true, false, false, false, false, false, 6)); //Case 2
            //lTestDataList.Add(new TestData(FileDirectory + "OperationsNResourcesNoDeadlock3.xml", true, false, false, false, false, false, 4)); //Case 2
            //lTestDataList.Add(new TestData(FileDirectory + "OperationsNResourcesDeadlock.xml", true, false, false, false, false, true, 4)); //Case 2
            //lTestDataList.Add(new TestData(FileDirectory + "ParallelOperationsNoDeadlock.xml", true, false, false, false, false, false, 6)); //Case 2
            //lTestDataList.Add(new TestData(FileDirectory + "OperationsAllUnusedNoDeadlock.xml", true, false, false, false, false, false, 6)); //Case 2
            //lTestDataList.Add(new TestData(FileDirectory + "ParallelOperationsSameResourceNoDeadlock.xml", true, false, false, false, false, false, 6)); //Case 2
            //lTestDataList.Add(new TestData(FileDirectory + "OperationsNoDeadlock3.xml", true, false, false, false, false, false, 40)); //Three operations in sequence
        }
        private string GetTestFilePath(string pTestFileName)
        {
            return FileDirectory + pTestFileName;
        }

        [Test]
        [TestCase("0.NoDataInput.xml", false)]    //No input data
        [TestCase("0.JustOperationInput.xml", false)]    //Just operation
        [TestCase("0.JustVariantInput.xml", false)]    //Just variant
        [TestCase("0.JustVariantGroupInput.xml", false)]    //Just variant group
        [TestCase("0.JustOperationNVariantInput.xml", false)]    //Just operation and variant
        [TestCase("0.JustOperationNVariantGroupInput.xml", false)]    //Just operation and variant group
        [TestCase("0.JustVariantNVariantGroupInput.xml", false)]    //Just variant and variant group
        //We know that Variant, Variant group and Operations are needed the rest are optional
        public void LoadInitialData_WhenRun_IncorrectInputData(string pTestFileName, bool pExpectedAnalysisResult)
        {
            try
            {
                Console.WriteLine("LoadInitialData Test on : " + GetTestFilePath(pTestFileName));

                var lDataLoaded = _z3SolverEngineer.LoadInitialData(Enumerations.InitializerSource.InitialDataFile, GetTestFilePath(pTestFileName));

                Assert.AreEqual(lDataLoaded, pExpectedAnalysisResult);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Initial data was not loaded!");
                Console.WriteLine(ex.Message);
            }
        }

        [Test]
        [TestCase("1.JustVariantNVariantGroupNOperation.xml", true)]    //Just Variant and Variant Group and Operation
        [TestCase("1.JustVariantNVariantGroupNOperationNConfigurationRule.xml", true)]    //Just Variant and Variant Group and Operation and Configuration Rule
        //We know that Variant, Variant group and Operations are needed the rest are optional
        public void LoadInitialData_WhenRun_CorrectInputData(string pTestFileName, bool pExpectedAnalysisResult)
        {
            try
            {
                Console.WriteLine("LoadInitialData Test on : " + GetTestFilePath(pTestFileName));

                var lDataLoaded = _z3SolverEngineer.LoadInitialData(Enumerations.InitializerSource.InitialDataFile, GetTestFilePath(pTestFileName));

                Assert.AreEqual(lDataLoaded, pExpectedAnalysisResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Initial data was not loaded!");
                Console.WriteLine(ex.Message);
            }
        }

        [Test]
        [TestCase("OperationsNoDeadlock.xml", 6)]
        [TestCase("OperationsNoDeadlock2.xml", 6)]
        [TestCase("OperationsDeadlock.xml", 6)]
        [TestCase("OperationsNResourcesNoDeadlock.xml", 6)]
        [TestCase("OperationsNResourcesNoDeadlock2.xml", 6)]
        [TestCase("OperationsNResourcesNoDeadlock3.xml", 4)]
        [TestCase("OperationsNResourcesDeadlock.xml", 4)]
        [TestCase("ParallelOperationsNoDeadlock.xml", 6)]
        [TestCase("OperationsAllUnusedNoDeadlock.xml", 6)]
        [TestCase("ParallelOperationsSameResourceNoDeadlock.xml", 6)]
        [TestCase("OperationsNoDeadlock3.xml", 40)]
        public void CalculateAnalysisNoOfCycles_Test(string pTestFileName, int pExpectedAnalysisResult)
        {
            try
            {
                var lDataLoaded = _z3SolverEngineer.LoadInitialData(Enumerations.InitializerSource.InitialDataFile, GetTestFilePath(pTestFileName));

                if (lDataLoaded)
                {
                    Console.WriteLine("Calculate Analysis No Of Cycles on : " + GetTestFilePath(pTestFileName));
                    int lAnalysisResult = 0;

                    //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                    //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                    //             , Build P Constraints, Number Of Models Required
                    _z3SolverEngineer.SetVariationPoints(ProductPlatformAnalyzer.Enumerations.GeneralAnalysisType.Dynamic
                                                        , ProductPlatformAnalyzer.Enumerations.AnalysisType.ExistanceOfDeadlockAnalysis);

                    //Parameters: Analysis Result, Analysis Detail Result, Variants Result
                    //          , Transitions Result, Analysis Timing, Unsat Core
                    //          , Stop between each transition, Stop at end of analysis, Create HTML Output
                    //          , Report timings, Debug Mode (Make model file), User Messages
                    _z3SolverEngineer.SetReportType(true, false, false
                                                    , false, false, false
                                                    , false, false, true
                                                    , true, true, false);

                    lAnalysisResult = _z3SolverEngineer.CalculateAnalysisNoOfCycles();

                    Assert.AreEqual(lAnalysisResult, pExpectedAnalysisResult);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Calculate Analysis No Of Cycles failed!");
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }
        ///// <summary>
        ///// This function reads the variation points values from the external file
        ///// </summary>
        //public void LoadVariationPointsFromXMLFile()
        //{
        //    try
        //    {
        //        string lFileName = "..\\..\\VariationPoints.xml";
        //        //new instance of xdoc
        //        XmlDocument xDoc = new XmlDocument();

        //        //First load the XML file from the file path
        //        xDoc.Load(lFileName);

        //        XmlNodeList nodeList = xDoc.DocumentElement.SelectNodes("//variationPoint");

        //        if (nodeList.Count > 0)
        //        {
        //            foreach (XmlNode lXmlNode in nodeList)
        //            {
        //                string lVariationPointName = lXmlNode["name"].InnerText;
        //                switch (lVariationPointName)
        //                {
        //                    case "ReportAnalysisResult":
        //                        cZ3SolverEngineer.ReportAnalysisResult = bool.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    case "ReportAnalysisDetailResult":
        //                        cZ3SolverEngineer.ReportAnalysisDetailResult = bool.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    case "ReportVariantsResult":
        //                        cZ3SolverEngineer.ReportVariantsResult = bool.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    case "ReportTransitionsResult":
        //                        cZ3SolverEngineer.ReportTransitionsResult = bool.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    case "ReportAnalysisTiming":
        //                        cZ3SolverEngineer.ReportAnalysisTiming = bool.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    case "ReportUnsatCore":
        //                        cZ3SolverEngineer.ReportUnsatCore = bool.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    case "StopBetweenEachTransition":
        //                        cZ3SolverEngineer.StopBetweenEachTransition = bool.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    case "StopAEndOfAnalysis":
        //                        cZ3SolverEngineer.StopAEndOfAnalysis = bool.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    case "CreateHTMLOutput":
        //                        cZ3SolverEngineer.CreateHTMLOutput = bool.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    case "ReportTimings":
        //                        cZ3SolverEngineer.ReportTimings = bool.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    case "NoOfModelsRequired":
        //                        cZ3SolverEngineer.NoOfModelsRequired = int.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    case "OperationWaiting":
        //                        cZ3SolverEngineer.OperationWaiting = bool.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    case "OperationMutualExecution":
        //                        cZ3SolverEngineer.OperationMutualExecution = bool.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    case "DebugMode":
        //                        cZ3SolverEngineer.setDebugMode(bool.Parse(lXmlNode["value"].InnerText));
        //                        break;
        //                    case "UserMessages":
        //                        cOutputHandler.setEnableUserMessages(bool.Parse(lXmlNode["value"].InnerText));
        //                        break;
        //                    case "RandomMaxNoOfConfigurationRules":
        //                        cZ3SolverEngineer.RandomMaxNoOfConfigurationRules = int.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    //1.RandomMaxVariantGroupumber
        //                    case "RandomMaxVariantGroupumber":
        //                        cZ3SolverEngineer.RandomMaxVariantGroupumber = int.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    //2.RandomMaxVariantNumber
        //                    case "RandomMaxVariantNumber":
        //                        cZ3SolverEngineer.RandomMaxVariantNumber = int.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    //3.RandomMaxPartNumber
        //                    case "RandomMaxPartNumber":
        //                        cZ3SolverEngineer.RandomMaxPartNumber = int.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    //4.RandomMaxOperationNumber
        //                    case "RandomMaxOperationNumber":
        //                        cZ3SolverEngineer.RandomMaxOperationNumber = int.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    //5.RandomTrueProbability
        //                    case "RandomTrueProbability":
        //                        cZ3SolverEngineer.RandomTrueProbability = int.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    //6.RandomFalseProbability
        //                    case "RandomFalseProbability":
        //                        cZ3SolverEngineer.RandomFalseProbability = int.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    //7.RandomExpressionProbability
        //                    case "RandomExpressionProbability":
        //                        cZ3SolverEngineer.RandomExpressionProbability = int.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    //8.RandomMaxTraitNumber
        //                    case "RandomMaxTraitNumber":
        //                        cZ3SolverEngineer.RandomMaxTraitNumber = int.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    //9.RandomMaxNoOfTraitAttributes
        //                    case "RandomMaxNoOfTraitAttributes":
        //                        cZ3SolverEngineer.RandomMaxNoOfTraitAttributes = int.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    //10.RandomMaxResourceNumber
        //                    case "RandomMaxResourceNumber":
        //                        cZ3SolverEngineer.RandomMaxResourceNumber = int.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    //11.RandomMaxExpressionOperandNumber
        //                    case "RandomMaxExpressionOperandNumber":
        //                        cZ3SolverEngineer.RandomMaxExpressionOperandNumber = int.Parse(lXmlNode["value"].InnerText);
        //                        break;
        //                    default:
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        cOutputHandler.printMessageToConsole("error in LoadVariationPointsFromXMLFile");
        //        cOutputHandler.printMessageToConsole(ex.Message);
        //    }
        //}


        [Test]
        [TestCase("OperationsNoDeadlock.xml", false)]
        [TestCase("OperationsNoDeadlock2.xml", false)]
        [TestCase("OperationsDeadlock.xml", true)]
        [TestCase("OperationsNResourcesNoDeadlock.xml", false)]
        [TestCase("OperationsNResourcesNoDeadlock2.xml", false)]
        [TestCase("OperationsNResourcesNoDeadlock3.xml", false)]
        [TestCase("OperationsNResourcesDeadlock.xml", true)]
        [TestCase("ParallelOperationsNoDeadlock.xml", false)]
        [TestCase("OperationsAllUnusedNoDeadlock.xml", false)]
        [TestCase("ParallelOperationsSameResourceNoDeadlock.xml", false)]
        [TestCase("OperationsNoDeadlock3.xml", false)]
        public void ExistanceOfDeadlockAnalysis_Test(string pTestFileName, bool pExpectedAnalysisResult)
        {
            try
            {
                var lDataLoaded = _z3SolverEngineer.LoadInitialData(Enumerations.InitializerSource.InitialDataFile, GetTestFilePath(pTestFileName));

                if (lDataLoaded)
                {
                    Console.WriteLine("Existance of deadlock analysis on : " + GetTestFilePath(pTestFileName));
                    bool lAnalysisResult = false;

                    //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                    //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                    //             , Build P Constraints, Number Of Models Required
                    _z3SolverEngineer.SetVariationPoints(ProductPlatformAnalyzer.Enumerations.GeneralAnalysisType.Dynamic
                                                        , ProductPlatformAnalyzer.Enumerations.AnalysisType.ExistanceOfDeadlockAnalysis);

                    //Parameters: Analysis Result, Analysis Detail Result, Variants Result
                    //          , Transitions Result, Analysis Timing, Unsat Core
                    //          , Stop between each transition, Stop at end of analysis, Create HTML Output
                    //          , Report timings, Debug Mode (Make model file), User Messages
                    _z3SolverEngineer.SetReportType(true, false, false
                                                    , false, false, false
                                                    , false, false, true
                                                    , true, true, false);

                    lAnalysisResult = _z3SolverEngineer.ExistanceOfDeadlockAnalysis(true, true);

                    Assert.AreEqual(lAnalysisResult, pExpectedAnalysisResult);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Existance Of Deadlock Analysis failed!");
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }

        //[Test]
        //public void OperationDependencyAnalysis_Test()
        //{
        //    try
        //    {
        //        foreach (TestOperationsData lTestOperation in lTestOperationList)
        //        {
        //            Z3SolverEngineer lZ3SolverEngineer = new Z3SolverEngineer();

        //            OutputHandler lOutputHandler = new OutputHandler();
        //            lZ3SolverEngineer.DefaultAnalyzerSetting(lOutputHandler);
        //            //LoadVariationPointsFromXMLFile();

        //            bool lDataLoaded = false;

        //            lDataLoaded = lZ3SolverEngineer.loadInitialData(Enumerations.InitializerSource.InitialDataFile, lTestOperation.lTestFile);

        //            if (lDataLoaded)
        //            {
        //                Console.WriteLine("Operation Dependency Analysis on : " + lTestOperation.lTestFile);
        //                bool lAnalysisResult = false;

        //                //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
        //                //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
        //                //             , Build P Constraints, Number Of Models Required
        //                lZ3SolverEngineer.setVariationPoints(ProductPlatformAnalyzer.Enumerations.GeneralAnalysisType.Dynamic
        //                                                    , ProductPlatformAnalyzer.Enumerations.AnalysisType.ExistanceOfDeadlockAnalysis);

        //                //Parameters: Analysis Result, Analysis Detail Result, Variants Result
        //                //          , Transitions Result, Analysis Timing, Unsat Core
        //                //          , Stop between each transition, Stop at end of analysis, Create HTML Output
        //                //          , Report timings, Debug Mode (Make model file), User Messages
        //                lZ3SolverEngineer.setReportType(true, false, false
        //                                                , false, false, false
        //                                                , false, false, true
        //                                                , true, true, false);

        //                lAnalysisResult = lZ3SolverEngineer.OperationDependencyAnalysis();

        //                Assert.AreEqual(lAnalysisResult, lTestOperation.lOperationDependencyResult);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Existance Of Deadlock Analysis failed!");
        //        Console.WriteLine(ex.Message);
        //        Assert.Fail();
        //    }
        //}

        //[TestMethod]
        //public void VariantSelectabilityAnalysis_Test()
        //{
        //    try
        //    {
        //        foreach (TestData lTestData in lTestDataList)
        //        {
        //            Z3SolverEngineer lZ3SolverEngineer = new Z3SolverEngineer();

        //            bool lDataLoaded = false;

        //            lDataLoaded = lZ3SolverEngineer.loadInitialData(Enumerations.InitializerSource.InitialDataFile, lTestData.lTestFile);

        //            if (lDataLoaded)
        //            {
        //                Console.WriteLine("VariantSelectabilityAnalysis on : " + lTestData.lTestFile);

        //                bool lAnalysisResult = false;

        //                //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
        //                //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
        //                //             , Build P Constraints, Number Of Models Required
        //                lZ3SolverEngineer.setVariationPoints(lTestData.lGeneralAnalysisType
        //                                                    , lTestData.lAnalysisType);

        //                //Parameters: Analysis Result, Analysis Detail Result, Variants Result
        //                //          , Transitions Result, Analysis Timing, Unsat Core
        //                //          , Stop between each transition, Stop at end of analysis, Create HTML Output
        //                //          , Report timings, Debug Mode (Make model file), User Messages
        //                lZ3SolverEngineer.setReportType(true, false, false
        //                                                , false, false, false
        //                                                , false, false, true
        //                                                , true, true, false);

        //                lAnalysisResult = lZ3SolverEngineer.VariantSelectabilityAnalysis(false, true);

        //                Assert.AreEqual(lAnalysisResult, lTestData.lVariantSelectabilityExpectedResult);
        //            }
        //            else
        //            {
        //                Console.WriteLine("Initial data was not loaded!");
        //                Assert.AreEqual(false, lTestData.lVariantSelectabilityExpectedResult);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Variant Selectability Analysis failed!");
        //        Console.WriteLine(ex.Message);
        //        Assert.Fail();
        //    }
        //}

        //[TestMethod]
        //public void AlwaysSelectedVariantAnalysis_Test()
        //{
        //    try
        //    {
        //        foreach (TestData lTestData in lTestDataList)
        //        {
        //            Z3SolverEngineer lZ3SolverEngineer = new Z3SolverEngineer();

        //            bool lDataLoaded = false;

        //            lDataLoaded = lZ3SolverEngineer.loadInitialData(Enumerations.InitializerSource.InitialDataFile, lTestData.lTestFile);

        //            if (lDataLoaded)
        //            {
        //                Console.WriteLine("AlwaysSelectedVariant Analysis on : " + lTestData.lTestFile);

        //                bool lAnalysisResult = false;

        //                //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
        //                //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
        //                //             , Build P Constraints, Number Of Models Required
        //                lZ3SolverEngineer.setVariationPoints(lTestData.lGeneralAnalysisType
        //                                                    , lTestData.lAnalysisType);

        //                //Parameters: Analysis Result, Analysis Detail Result, Variants Result
        //                //          , Transitions Result, Analysis Timing, Unsat Core
        //                //          , Stop between each transition, Stop at end of analysis, Create HTML Output
        //                //          , Report timings, Debug Mode (Make model file), User Messages
        //                lZ3SolverEngineer.setReportType(true, false, false
        //                                                , false, false, false
        //                                                , false, false, true
        //                                                , true, true, false);

        //                lAnalysisResult = lZ3SolverEngineer.NeverSelectedVariantAnalysis(false, true);

        //                Assert.AreEqual(lAnalysisResult, lTestData.lAlwaysSelectedVariantExpectedResult);
        //            }
        //            else
        //            {
        //                Console.WriteLine("Initial data was not loaded!");
        //                Assert.AreEqual(false, lTestData.lAlwaysSelectedVariantExpectedResult);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Always Selected Variant Analysis failed!");
        //        Console.WriteLine(ex.Message);
        //        Assert.Fail();
        //    }
        //}

        //[TestMethod]
        //public void OperationSelectabilityAnalysis_Test()
        //{
        //    try
        //    {
        //        foreach (TestData lTestData in lTestDataList)
        //        {
        //            Z3SolverEngineer lZ3SolverEngineer = new Z3SolverEngineer();

        //            bool lDataLoaded = false;

        //            lDataLoaded = lZ3SolverEngineer.loadInitialData(Enumerations.InitializerSource.InitialDataFile, lTestData.lTestFile);

        //            if (lDataLoaded)
        //            {
        //                Console.WriteLine("OperationSelectabilityAnalysis on : " + lTestData.lTestFile);

        //                bool lAnalysisResult = false;

        //                //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
        //                //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
        //                //             , Build P Constraints, Number Of Models Required
        //                lZ3SolverEngineer.setVariationPoints(lTestData.lGeneralAnalysisType
        //                                                    , lTestData.lAnalysisType);

        //                //Parameters: Analysis Result, Analysis Detail Result, Variants Result
        //                //          , Transitions Result, Analysis Timing, Unsat Core
        //                //          , Stop between each transition, Stop at end of analysis, Create HTML Output
        //                //          , Report timings, Debug Mode (Make model file), User Messages
        //                lZ3SolverEngineer.setReportType(true, false, false
        //                                                , false, false, false
        //                                                , false, false, true
        //                                                , true, true, false);

        //                lAnalysisResult = lZ3SolverEngineer.OperationSelectabilityAnalysis(false, true);

        //                Assert.AreEqual(lAnalysisResult, lTestData.lOperationSelectabilityExpectedResult);
        //            }
        //            else
        //            {
        //                Console.WriteLine("Initial data was not loaded!");
        //                Assert.AreEqual(false, lTestData.lOperationSelectabilityExpectedResult);
        //            }
                    
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Operation Selectability Analysis failed!");
        //        Console.WriteLine(ex.Message);
        //        Assert.Fail();
        //    }
        //}

        //[TestMethod]
        //public void AlwaysSelectedOperationAnalysis_Test()
        //{
        //    try
        //    {
        //        foreach (TestData lTestData in lTestDataList)
        //        {
        //            Z3SolverEngineer lZ3SolverEngineer = new Z3SolverEngineer();

        //            bool lDataLoaded = false;

        //            lDataLoaded = lZ3SolverEngineer.loadInitialData(Enumerations.InitializerSource.InitialDataFile, lTestData.lTestFile);

        //            if (lDataLoaded)
        //            {
        //                Console.WriteLine("AlwaysSelectedOperationAnalysis on : " + lTestData.lTestFile);
        //                bool lAnalysisResult = false;

        //                //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
        //                //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
        //                //             , Build P Constraints, Number Of Models Required
        //                lZ3SolverEngineer.setVariationPoints(lTestData.lGeneralAnalysisType
        //                                                    , lTestData.lAnalysisType);

        //                //Parameters: Analysis Result, Analysis Detail Result, Variants Result
        //                //          , Transitions Result, Analysis Timing, Unsat Core
        //                //          , Stop between each transition, Stop at end of analysis, Create HTML Output
        //                //          , Report timings, Debug Mode (Make model file), User Messages
        //                lZ3SolverEngineer.setReportType(true, false, false
        //                                                , false, false, false
        //                                                , false, false, true
        //                                                , true, true, false);

        //                lAnalysisResult = lZ3SolverEngineer.NeverSelectedOperationAnalysis(false, true);

        //                Assert.AreEqual(lAnalysisResult, lTestData.lAlwaysSlectedOperationExpectedResult);
        //            }
        //            else
        //            {
        //                Console.WriteLine("Initial data was not loaded!");
        //                Assert.AreEqual(false, lTestData.lAlwaysSlectedOperationExpectedResult);
        //            }
                    
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Always Selected Operation Analysis failed!");
        //        Console.WriteLine(ex.Message);
        //        Assert.Fail();
        //    }
        //}

        //[TestMethod]
        //public void OperationDependencyAnalysis_Test()
        //{
        //    try
        //    {
        //        OutputHandler lOutputHandler = new OutputHandler();
        //        FrameworkWrapper lFrameworkWrapper = new FrameworkWrapper(lOutputHandler);
        //        Z3SolverEngineer lZ3SolverEngineer = new Z3SolverEngineer(lFrameworkWrapper);

        //        Operation lOperation1 = new Operation("O1", "", "", "true", "true");
        //        Operation lOperation2 = new Operation("O2", "", "", "true", "true");

        //        lFrameworkWrapper.OperationSet.Add(lOperation1);
        //        lFrameworkWrapper.OperationSet.Add(lOperation2);

                
        //        lZ3SolverEngineer.OperationDependencyAnalysis();

        //        Assert.AreEqual(lZ3SolverEngineer.DependentActionsList.Count, 0);

                
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("OperationDependencyAnalysis Test failed!");
        //        Console.WriteLine(ex.Message);
        //        Assert.Fail();
        //    }
        //}
    }
}

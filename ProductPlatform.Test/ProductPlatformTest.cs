using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductPlatformAnalyzer;
using System.Collections.Generic;

namespace ProductPlatform.Test
{
    public class TestData
    {
        public string lTestFile;
        public ProductPlatformAnalyzer.Enumerations.GeneralAnalysisType lGeneralAnalysisType;
        public ProductPlatformAnalyzer.Enumerations.AnalysisType lAnalysisType;
        public bool lLoadDataExpectedResult;
        public int lNoOfModelsRequired;
        public bool lVariantSelectabilityExpectedResult;
        public bool lOperationSelectabilityExpectedResult;
        public bool lAlwaysSelectedVariantExpectedResult;
        public bool lAlwaysSlectedOperationExpectedResult;
        public bool lDeadlockDetectionExpectedResult;

        public TestData(string pTestFile
                        , bool pLoadDataExpectedResult
                        , bool pVariantSelectabilityExpectedResult
                        , bool pOperationSelectabilityExpectedResult
                        , bool pAlwaysSelectedVariantExpectedResult
                        , bool pAlwaysSlectedOperationExpectedResult
                        , bool pDeadlockDetectionExpectedResult
                        , int pNoOfModelsRequired = 4)
        {
            lTestFile = pTestFile;
            lLoadDataExpectedResult = pLoadDataExpectedResult;
            lNoOfModelsRequired = pNoOfModelsRequired;
            lVariantSelectabilityExpectedResult = pVariantSelectabilityExpectedResult;
            lOperationSelectabilityExpectedResult = pOperationSelectabilityExpectedResult;
            lAlwaysSelectedVariantExpectedResult = pAlwaysSelectedVariantExpectedResult;
            lAlwaysSlectedOperationExpectedResult = pAlwaysSlectedOperationExpectedResult;
            lDeadlockDetectionExpectedResult = pDeadlockDetectionExpectedResult;
        }
    }

    [TestClass]
    public class ProductPlatformTest
    {
        private List<TestData> lTestDataList = new List<TestData>();

        public ProductPlatformTest()
        {
            //Parameter list: Data file name, load data result, Variant Selectability Result, Operation Selectability Result
            //              , Always Selected Variant Result, Always Selected Operation Result, Deadlock Detection Result

            lTestDataList.Add(new TestData("0.0.0V0VG0O0C0P.xml", false, false, false, false, false, false));
            lTestDataList.Add(new TestData("0.1.0V0VG1O0C0P.xml", false, false, false, false, false, false));
            lTestDataList.Add(new TestData("0.2.1V0VG1O0C0P.xml", false, false, false, false, false, false));

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
            //lTestDataList.Add(new TestData("2.2V1VG2O1C1PNoTransitions-1UnselectV.xml", true, false, true, true, true));
            //lTestDataList.Add(new TestData("3.2V1VG2O1C1PNoTransitions.xml", true, true, true, true, false));

            //lTestDataList.Add(new TestData("4.2V1VG3O1C1PNoTransition.xml", false, false, false, false, false));
            //lTestDataList.Add(new TestData("5.2V1VG2O1C1PNoTransition.xml", false, false, false, false, false));

            lTestDataList.Add(new TestData("6.2V1VG2O0C0P.xml", true, true, true, true, true, false));
            /*lTestDataList.Add(new TestData("7.2V1VG2O0C0P.xml", true, true, true, true, true));
            lTestDataList.Add(new TestData("8.2V1VG2O0C0P.xml", true, true, true, false, false));
            lTestDataList.Add(new TestData("9.2V1VG2O0C1P.xml", true, true, true, true, true));
            lTestDataList.Add(new TestData("10.2V1VG2O0C1P.xml", true, true, true, true, true));
            lTestDataList.Add(new TestData("11.2V1VG2O0C1P.xml", true, true, true, false, false));
            lTestDataList.Add(new TestData("12.2V1VG2O0C2P.xml", true, true, true, true, true));
            lTestDataList.Add(new TestData("13.2V1VG2O0C2P.xml", true, true, true, true, true));
            lTestDataList.Add(new TestData("14.2V1VG2O0C2P.xml", true, true, true, false, false));*/
        }
        
        [TestMethod]
        public void LoadInitialData_Test()
        {
            try
            {
                foreach (TestData lTestData in lTestDataList)
                {
                    Z3SolverEngineer lZ3SolverEngineer = new Z3SolverEngineer();

                    bool lDataLoaded = false;

                    Console.WriteLine("LoadInitialData Test on : " + lTestData.lTestFile);
                    lDataLoaded = lZ3SolverEngineer.loadInitialData(Enumerations.InitializerSource.InitialDataFile, lTestData.lTestFile);

                    Assert.AreEqual(lDataLoaded, lTestData.lLoadDataExpectedResult);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Initial data was not loaded!");
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }

        [TestMethod]
        public void VariantSelectabilityAnalysis_Test()
        {
            try
            {
                foreach (TestData lTestData in lTestDataList)
                {
                    Z3SolverEngineer lZ3SolverEngineer = new Z3SolverEngineer();

                    bool lDataLoaded = false;

                    lDataLoaded = lZ3SolverEngineer.loadInitialData(Enumerations.InitializerSource.InitialDataFile, lTestData.lTestFile);

                    if (lDataLoaded)
                    {
                        Console.WriteLine("VariantSelectabilityAnalysis on : " + lTestData.lTestFile);

                        bool lAnalysisResult = false;

                        //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                        //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                        //             , Build P Constraints, Number Of Models Required
                        lZ3SolverEngineer.setVariationPoints(lTestData.lGeneralAnalysisType
                                                            , lTestData.lAnalysisType
                                                            , lTestData.lNoOfModelsRequired);

                        //Parameters: Analysis Result, Analysis Detail Result, Variants Result
                        //          , Transitions Result, Analysis Timing, Unsat Core
                        //          , Stop between each transition, Stop at end of analysis, Create HTML Output
                        //          , Report timings, Debug Mode (Make model file)
                        lZ3SolverEngineer.setReportType(true, false, false
                                                        , false, false, false
                                                        , false, false, true
                                                        , true, true);

                        lAnalysisResult = lZ3SolverEngineer.VariantSelectabilityAnalysis(false, true);

                        Assert.AreEqual(lAnalysisResult, lTestData.lVariantSelectabilityExpectedResult);
                    }
                    else
                    {
                        Console.WriteLine("Initial data was not loaded!");
                        Assert.AreEqual(false, lTestData.lVariantSelectabilityExpectedResult);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Variant Selectability Analysis failed!");
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AlwaysSelectedVariantAnalysis_Test()
        {
            try
            {
                foreach (TestData lTestData in lTestDataList)
                {
                    Z3SolverEngineer lZ3SolverEngineer = new Z3SolverEngineer();

                    bool lDataLoaded = false;

                    lDataLoaded = lZ3SolverEngineer.loadInitialData(Enumerations.InitializerSource.InitialDataFile, lTestData.lTestFile);

                    if (lDataLoaded)
                    {
                        Console.WriteLine("AlwaysSelectedVariant Analysis on : " + lTestData.lTestFile);

                        bool lAnalysisResult = false;

                        //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                        //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                        //             , Build P Constraints, Number Of Models Required
                        lZ3SolverEngineer.setVariationPoints(lTestData.lGeneralAnalysisType
                                                            , lTestData.lAnalysisType
                                                            , lTestData.lNoOfModelsRequired);

                        //Parameters: Analysis Result, Analysis Detail Result, Variants Result
                        //          , Transitions Result, Analysis Timing, Unsat Core
                        //          , Stop between each transition, Stop at end of analysis, Create HTML Output
                        //          , Report timings, Debug Mode (Make model file)
                        lZ3SolverEngineer.setReportType(true, false, false
                                                        , false, false, false
                                                        , false, false, true
                                                        , true, true);

                        lAnalysisResult = lZ3SolverEngineer.AlwaysSelectedVariantAnalysis(false, true);

                        Assert.AreEqual(lAnalysisResult, lTestData.lAlwaysSelectedVariantExpectedResult);
                    }
                    else
                    {
                        Console.WriteLine("Initial data was not loaded!");
                        Assert.AreEqual(false, lTestData.lAlwaysSelectedVariantExpectedResult);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Always Selected Variant Analysis failed!");
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }

        [TestMethod]
        public void OperationSelectabilityAnalysis_Test()
        {
            try
            {
                foreach (TestData lTestData in lTestDataList)
                {
                    Z3SolverEngineer lZ3SolverEngineer = new Z3SolverEngineer();

                    bool lDataLoaded = false;

                    lDataLoaded = lZ3SolverEngineer.loadInitialData(Enumerations.InitializerSource.InitialDataFile, lTestData.lTestFile);

                    if (lDataLoaded)
                    {
                        Console.WriteLine("OperationSelectabilityAnalysis on : " + lTestData.lTestFile);

                        bool lAnalysisResult = false;

                        //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                        //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                        //             , Build P Constraints, Number Of Models Required
                        lZ3SolverEngineer.setVariationPoints(lTestData.lGeneralAnalysisType
                                                            , lTestData.lAnalysisType
                                                            , lTestData.lNoOfModelsRequired);

                        //Parameters: Analysis Result, Analysis Detail Result, Variants Result
                        //          , Transitions Result, Analysis Timing, Unsat Core
                        //          , Stop between each transition, Stop at end of analysis, Create HTML Output
                        //          , Report timings, Debug Mode (Make model file)
                        lZ3SolverEngineer.setReportType(true, false, false
                                                        , false, false, false
                                                        , false, false, true
                                                        , true, true);

                        lAnalysisResult = lZ3SolverEngineer.OperationSelectabilityAnalysis(false, true);

                        Assert.AreEqual(lAnalysisResult, lTestData.lOperationSelectabilityExpectedResult);
                    }
                    else
                    {
                        Console.WriteLine("Initial data was not loaded!");
                        Assert.AreEqual(false, lTestData.lOperationSelectabilityExpectedResult);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Operation Selectability Analysis failed!");
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AlwaysSelectedOperationAnalysis_Test()
        {
            try
            {
                foreach (TestData lTestData in lTestDataList)
                {
                    Z3SolverEngineer lZ3SolverEngineer = new Z3SolverEngineer();

                    bool lDataLoaded = false;

                    lDataLoaded = lZ3SolverEngineer.loadInitialData(Enumerations.InitializerSource.InitialDataFile, lTestData.lTestFile);

                    if (lDataLoaded)
                    {
                        Console.WriteLine("AlwaysSelectedOperationAnalysis on : " + lTestData.lTestFile);
                        bool lAnalysisResult = false;

                        //Parameters: General Analysis Type, Analysis Type, Convert variants, Convert configuration rules
                        //             , Convert operations, Convert operation precedence rules, Convert variant operation relation, Convert resources, Convert goals
                        //             , Build P Constraints, Number Of Models Required
                        lZ3SolverEngineer.setVariationPoints(lTestData.lGeneralAnalysisType
                                                            , lTestData.lAnalysisType
                                                            , lTestData.lNoOfModelsRequired);

                        //Parameters: Analysis Result, Analysis Detail Result, Variants Result
                        //          , Transitions Result, Analysis Timing, Unsat Core
                        //          , Stop between each transition, Stop at end of analysis, Create HTML Output
                        //          , Report timings, Debug Mode (Make model file)
                        lZ3SolverEngineer.setReportType(true, false, false
                                                        , false, false, false
                                                        , false, false, true
                                                        , true, true);

                        lAnalysisResult = lZ3SolverEngineer.AlwaysSelectedOperationAnalysis(false, true);

                        Assert.AreEqual(lAnalysisResult, lTestData.lAlwaysSlectedOperationExpectedResult);
                    }
                    else
                    {
                        Console.WriteLine("Initial data was not loaded!");
                        Assert.AreEqual(false, lTestData.lAlwaysSlectedOperationExpectedResult);
                    }
                    
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Always Selected Operation Analysis failed!");
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }
    }
}

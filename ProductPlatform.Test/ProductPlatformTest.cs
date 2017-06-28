using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductPlatformAnalyzer;

namespace ProductPlatform.Test
{
    public class TestData
    {
        public string lTestFile;
        public bool lLoadDataExpectedResult;
        public bool lVariantSelectabilityExpectedResult;
        public bool lOperationSelectabilityExpectedResult;
        public bool lAlwaysSelectedVariantExpectedResult;
        public bool lAlwaysSlectedOperationExpectedResult;

        public TestData(string pTestFile
                        , bool pLoadDataExpectedResult
                        , bool pVariantSelectabilityExpectedResult
                        , bool pOperationSelectabilityExpectedResult
                        , bool pAlwaysSelectedVariantExpectedResult
                        , bool pAlwaysSlectedOperationExpectedResult)
        {
            lTestFile = pTestFile;
            lLoadDataExpectedResult = pLoadDataExpectedResult;
            lVariantSelectabilityExpectedResult = pVariantSelectabilityExpectedResult;
            lOperationSelectabilityExpectedResult = pOperationSelectabilityExpectedResult;
            lAlwaysSelectedVariantExpectedResult = pAlwaysSelectedVariantExpectedResult;
            lAlwaysSlectedOperationExpectedResult = pAlwaysSlectedOperationExpectedResult;
        }
    }

    [TestClass]
    public class ProductPlatformTest
    {
        //private TestData lTestData = new TestData("0.0V0VG0O0C0P.xml", false, false, false, false, false);
        //private TestData lTestData = new TestData("0.0V0VG1O0C0P.xml", false, false, false, false, false);
        //private TestData lTestData = new TestData("0.1V0VG1O0C0P.xml", false, false, false, false, false);
        //private TestData lTestData = new TestData("1.1V1VG2O1C1P.xml", true, true, true, false, false);
        ///private TestData lTestData = new TestData("2.2V1VG2O1C1PNoTransitions-1UnselectV.xml", true, false, true, true, false);
        ///private TestData lTestData = new TestData("3.2V1VG2O1C1PNoTransitions.xml", true, true, true, true, false);
        
        ///private TestData lTestData = new TestData("4.2V1VG3O1C1PNoTransition.xml", false, false, false, false, false);
        ///private TestData lTestData = new TestData("5.2V1VG2O1C1PNoTransition.xml", false, false, false, false, false);

        //private TestData lTestData = new TestData("6.2V1VG2O0C0P.xml", true, true, true, true, true);
        //private TestData lTestData = new TestData("7.2V1VG2O0C0P.xml", true, true, true, true, true);
        //private TestData lTestData = new TestData("8.2V1VG2O0C0P.xml", true, true, true, false, false);
        //private TestData lTestData = new TestData("9.2V1VG2O0C1P.xml", true, true, true, true, true);
        //private TestData lTestData = new TestData("10.2V1VG2O0C1P.xml", true, true, true, true, true);
        //private TestData lTestData = new TestData("11.2V1VG2O0C1P.xml", true, true, true, false, false);
        //private TestData lTestData = new TestData("12.2V1VG2O0C2P.xml", true, true, true, true, true);
        //private TestData lTestData = new TestData("13.2V1VG2O0C2P.xml", true, true, true, true, true);
        private TestData lTestData = new TestData("14.2V1VG2O0C2P.xml", true, true, true, false, false);

        [TestMethod]
        public void LoadInitialData_Test()
        {
            Z3SolverEngineer lZ3SolverEngineer = new Z3SolverEngineer();

            try
            {
                bool lDataLoaded = false;

                lDataLoaded = lZ3SolverEngineer.loadInitialData(InitializerSource.InternalFile, lTestData.lTestFile);

                Assert.AreEqual(lDataLoaded, lTestData.lLoadDataExpectedResult);

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
            Z3SolverEngineer lZ3SolverEngineer = new Z3SolverEngineer();

            try
            {
                bool lDataLoaded = false;

                lDataLoaded = lZ3SolverEngineer.loadInitialData(InitializerSource.InternalFile, lTestData.lTestFile);

                if (lDataLoaded)
                {
                    bool lAnalysisResult = false;

                    lZ3SolverEngineer.setReportType(true, false, false, false, false, false);

                    lAnalysisResult = lZ3SolverEngineer.VariantSelectabilityAnalysis(false, true);

                    Assert.AreEqual(lAnalysisResult, lTestData.lVariantSelectabilityExpectedResult);
                }
                else
                {
                    Console.WriteLine("Initial data was not loaded!");
                    Assert.AreEqual(false, lTestData.lVariantSelectabilityExpectedResult);
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
            Z3SolverEngineer lZ3SolverEngineer = new Z3SolverEngineer();
 
            try
            {
                bool lDataLoaded = false;

                lDataLoaded = lZ3SolverEngineer.loadInitialData(InitializerSource.InternalFile, lTestData.lTestFile);

                if (lDataLoaded)
                {
                    bool lAnalysisResult = false;

                    lZ3SolverEngineer.setReportType(true, false, false, false, false, false);

                    lAnalysisResult = lZ3SolverEngineer.AlwaysSelectedVariantAnalysis(false, true);

                    Assert.AreEqual(lAnalysisResult, lTestData.lAlwaysSelectedVariantExpectedResult);
                }
                else
                {
                    Console.WriteLine("Initial data was not loaded!");
                    Assert.AreEqual(false, lTestData.lAlwaysSelectedVariantExpectedResult);
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
            Z3SolverEngineer lZ3SolverEngineer = new Z3SolverEngineer();

            try
            {
                bool lDataLoaded = false;

                lDataLoaded = lZ3SolverEngineer.loadInitialData(InitializerSource.InternalFile, lTestData.lTestFile);

                if (lDataLoaded)
                {
                    bool lAnalysisResult = false;

                    lZ3SolverEngineer.setReportType(true, false, false, false, false, false);

                    lAnalysisResult = lZ3SolverEngineer.OperationSelectabilityAnalysis(false, true);

                    Assert.AreEqual(lAnalysisResult, lTestData.lOperationSelectabilityExpectedResult);
                }
                else
                {
                    Console.WriteLine("Initial data was not loaded!");
                    Assert.AreEqual(false, lTestData.lOperationSelectabilityExpectedResult);
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
            Z3SolverEngineer lZ3SolverEngineer = new Z3SolverEngineer();

            try
            {
                bool lDataLoaded = false;

                lDataLoaded = lZ3SolverEngineer.loadInitialData(InitializerSource.InternalFile, lTestData.lTestFile);

                if (lDataLoaded)
                {
                    bool lAnalysisResult = false;

                    lZ3SolverEngineer.setReportType(true, false, false, false, false, false);

                    lAnalysisResult = lZ3SolverEngineer.AlwaysSelectedOperationAnalysis(false, true);

                    Assert.AreEqual(lAnalysisResult, lTestData.lAlwaysSlectedOperationExpectedResult);
                }
                else
                {
                    Console.WriteLine("Initial data was not loaded!");
                    Assert.AreEqual(false, lTestData.lAlwaysSlectedOperationExpectedResult);
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

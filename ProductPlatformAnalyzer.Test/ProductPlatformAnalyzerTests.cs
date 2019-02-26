using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductPlatformAnalyzer;

namespace ProductPlatformAnalyzer.Test
{

    [TestFixture]
    public class ProductPlatformAnalyzerTests
    {
        [Test]
        public void OperationDependencyAnalysis_Test()
        {
            try
            {
                Z3SolverEngineer  lZ3SolverEngineer = new Z3SolverEngineer();

                bool lDataLoaded = false;

                String lTestFileName = "ActionDependency_TestCase1";

                Console.WriteLine("LoadInitialData Test on : " + lTestFileName);
                lDataLoaded = lZ3SolverEngineer.loadInitialData(Enumerations.InitializerSource.InitialDataFile, lTestFileName);


                lZ3SolverEngineer.OperationDependencyAnalysis();

                Assert.AreEqual(lZ3SolverEngineer.DependentActionsList.Count, 0);


            }
            catch (Exception ex)
            {
                Console.WriteLine("OperationDependencyAnalysis Test failed!");
                Console.WriteLine(ex.Message);
                Assert.Fail();
            }
        }
    }
}

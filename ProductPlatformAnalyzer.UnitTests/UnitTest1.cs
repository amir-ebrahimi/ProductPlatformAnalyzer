using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProductPlatformAnalyzer.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Z3SolverEngineer lZ3 = new Z3SolverEngineer();
            Assert.AreEqual(true, true);
        }
    }
}

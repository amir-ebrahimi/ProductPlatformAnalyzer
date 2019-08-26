using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductPlatformAnalyzer;

namespace ProductPlatfrmAnalyzer.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Z3SolverEngineer lZ3 = new Z3SolverEngineer();
            Console.WriteLine("to here");
            throw new Exception();
        }
    }
}

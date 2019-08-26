using NUnit.Framework;
using ProductPlatformAnalyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestFixture]
    public class UnitTests
    {
        [Test]
        public void LoadTest()
        {
            Z3SolverEngineer lZ3 = new Z3SolverEngineer();
            Assert.AreEqual(true, true);
        }
    }
}

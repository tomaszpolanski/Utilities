using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Utilities.Functional;
using System.Linq;
using System.Collections.Generic;


namespace Utilities.Tests
{
    [TestClass]
    public class TestLinqExtensions
    {

        [TestMethod]
        public void TestChoose()
        {
            List<String> list = new List<string> { "Something", null };

            IEnumerable<String> filtered = list.Choose(val => Option<string>.AsOption(val));

            Assert.AreEqual(1, filtered.Count());
        }
    }
}

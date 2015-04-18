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

        [TestMethod]
        public void TestTryFindSome()
        {
            var val = "Something";
            List<String> list = new List<string> { val, null };

            var filtered = list.TryFind(current => current == val);

            Assert.IsTrue(filtered.IsSome);
            Assert.AreEqual(val, filtered.Get());
        }

        [TestMethod]
        public void TestTryFindNone()
        {
            List<String> list = new List<string> { "Something", null };

             var filtered = list.TryFind(current => current == "Wrong");

            Assert.IsFalse(filtered.IsSome);
        }

        [TestMethod]
        public void TestGetSome()
        {
            var val = "Something";
            List<String> list = new List<string> { "Test", val };

            var filtered = list.Get(1);

            Assert.IsTrue(filtered.IsSome);
            Assert.AreEqual(val, filtered.Get());
        }

        [TestMethod]
        public void TestGetNone()
        {
            List<String> list = new List<string> { "Test", "Something" };

            var filtered = list.Get(3);

            Assert.IsFalse(filtered.IsSome);
        }

    }
}

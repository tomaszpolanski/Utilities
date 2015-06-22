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
            Assert.AreEqual(val, filtered.GetUnsafe);
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

            var filtered = list.TryElementAt(1);

            Assert.IsTrue(filtered.IsSome);
            Assert.AreEqual(val, filtered.GetUnsafe);
        }

        [TestMethod]
        public void TestGetNone()
        {
            List<String> list = new List<string> { "Test", "Something" };

            var filtered = list.TryElementAt(3);

            Assert.IsFalse(filtered.IsSome);
        }

        [TestMethod]
        public void TestTryFirstSuccess()
        {
            var op = Enumerable.Range(0, 2)
            .TryFirst();
            Assert.IsTrue(op.IsSome);
            Assert.AreEqual(0, op.GetUnsafe);
        }
        [TestMethod]
        public void TestTryFirstFailure()
        {
            var op = Enumerable.Range(0, 0)
            .TryFirst();
            Assert.IsFalse(op.IsSome);
        }
        [TestMethod]
        public void TestTryLastSuccess()
        {
            var op = Enumerable.Range(0, 2)
            .TryLast();
            Assert.IsTrue(op.IsSome);
            Assert.AreEqual(1, op.GetUnsafe);
        }
        [TestMethod]
        public void TestTryLastFailure()
        {
            var op = Enumerable.Range(0, 0)
            .TryLast();
            Assert.IsFalse(op.IsSome);
        }
        [TestMethod]
        public void TestTryElementAtSuccess()
        {
            var op = Enumerable.Range(0, 10)
            .TryElementAt(5);
            Assert.IsTrue(op.IsSome);
            Assert.AreEqual(5, op.GetUnsafe);
        }
        [TestMethod]
        public void TestTryElementAtFailure()
        {
            var op = Enumerable.Range(0, 0)
            .TryElementAt(5);
            Assert.IsFalse(op.IsSome);
        }

        [TestMethod]
        public void TestTryAggregateSuccess()
        {
            var op = Enumerable.Range(0, 10)
            .TryAggregate((f, s) => f + s);
            Assert.IsTrue(op.IsSome);
            Assert.AreEqual(45, op.GetUnsafe);
        }
        [TestMethod]
        public void TestTryAggregateFailure()
        {
            var op = Enumerable.Range(0, 0)
                               .TryAggregate((f, s) => f + s);
            Assert.IsFalse(op.IsSome);
        }

    }
}
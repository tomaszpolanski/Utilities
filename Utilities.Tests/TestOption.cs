using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utilities.Reactive;
using System;
using Utilities.Functional;

namespace Utilities.Tests
{
    [TestClass]
    public class TestOption
    {

        [TestMethod]
        public void TestAsOptionSome()
        {
            var option = Option<string>.AsOption("");

            Assert.AreNotEqual(Option<string>.None, option);
        }

        [TestMethod]
        public void TestAsOptionNone()
        {
            var option = Option<string>.AsOption(null);

            Assert.AreEqual(Option<string>.None, option);
        }

        [TestMethod]
        public void TestSelectSome()
        {
            var val = "Test";
            var option = Option<string>.AsOption(val)
                                       .Select(str => str.Length);

            Assert.AreEqual(val.Length, option.Get());
        }

        [TestMethod]
        public void TestSelectNone()
        {
            var option = Option<string>.AsOption(null)
                                       .Select(str => str.Length);

            Assert.AreEqual(Option<int>.None, option);
        }

        [TestMethod]
        public void TestSelectManySome()
        {
            var val = "Test";
            var option = Option<string>.AsOption(val)
                                       .SelectMany(str => Option<int>.AsOption(str.Length));

            Assert.AreEqual(val.Length, option.Get());
        }

        [TestMethod]
        public void TestSelectManyNone()
        {
            var val = "Test";
            var option = Option<string>.AsOption(val)
                                       .SelectMany(str => Option<string>.AsOption(null));

            Assert.AreEqual(Option<string>.None, option);
        }

    }
}
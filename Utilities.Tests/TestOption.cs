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
        public void TestMapSome()
        {
            var val = "Test";
            var option = Option<string>.AsOption(val)
                                       .Map(str => str.Length);

            Assert.AreEqual(val.Length, option.Get());
        }

        [TestMethod]
        public void TestMapNone()
        {
            var option = Option<string>.AsOption(null)
                                       .Map(str => str.Length);

            Assert.AreEqual(Option<int>.None, option);
        }

        [TestMethod]
        public void TestBindSome()
        {
            var val = "Test";
            var option = Option<string>.AsOption(val)
                                       .Bind(str => Option<int>.AsOption(str.Length));

            Assert.AreEqual(val.Length, option.Get());
        }

        [TestMethod]
        public void TestBindNone()
        {
            var val = "Test";
            var option = Option<string>.AsOption(val)
                                       .Bind(str => Option<string>.AsOption(null));

            Assert.AreEqual(Option<string>.None, option);
        }

    }
}
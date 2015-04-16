using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var option = Option<string>.None
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
                                       .SelectMany(str => Option<string>.None);

            Assert.AreEqual(Option<string>.None, option);
        }

        [TestMethod]
        public void TestWhereSomePositive()
        {
            var val = "Test";
            var option = Option<string>.AsOption(val)
                                    .Where(str => str.StartsWith(val));

            Assert.AreNotEqual(Option<string>.None, option);
            Assert.AreEqual(val, option.Get());
        }

        [TestMethod]
        public void TestWhereSomeNegative()
        {
            var val = "Test";
            var option = Option<string>.AsOption(val)
                                       .Where(str => str.StartsWith("Wrong"));

            Assert.AreEqual(Option<string>.None, option);
        }

        [TestMethod]
        public void TestWhereNone()
        {
            var option = Option<string>.None
                                       .Where(str => str.StartsWith("Wrong"));

            Assert.AreEqual(Option<string>.None, option);
        }

        [TestMethod]
        public void TestIterSome()
        {
            string valueToSet = "";

            var val = "Test";
            Option<string>.AsOption(val)
                          .Iter(i => valueToSet = i);

            Assert.AreEqual(val, valueToSet);
        }

        [TestMethod]
        public void TestIterNone()
        {
            var val = "";
            string valueToSet = val;
            Option<string>.None
                          .Iter(i => valueToSet = "Test");

            Assert.AreEqual(val, valueToSet);
        }
    }
}
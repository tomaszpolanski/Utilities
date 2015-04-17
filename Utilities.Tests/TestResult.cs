using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Utilities.Functional;

namespace Utilities.Tests
{
    [TestClass]
    public class TestResult
    {

        [TestMethod]
        public void TestAsResultSuccess()
        {
            var result = Result<string, string>.AsResult("", "");

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void TestAsResultFailure()
        {
            var result = Result<string, string>.AsResult(null, "");

            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void TestSelectSuccess()
        {
            var val = "Test";
            var result = Result<string, string>.AsResult(val, "")
                                       .Select(str => str.Length);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(val.Length, result.Get());
        }

        [TestMethod]
        public void TestSelectFailure()
        {
            var result = Result<string, string>.AsResult(null, "")
                                               .Select(str => str.Length);

            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void TestSelectManySuccess()
        {
            var val = "Test";
            var result = Result<string, string>.AsResult(val, "")
                                               .SelectMany(str => Result<int, string>.AsResult(str.Length, ""));

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(val.Length, result.Get());
        }

        [TestMethod]
        public void TestSelectManyFailure()
        {
            var val = "Test";
            var result = Result<string, string>.AsResult(val, "")
                                               .SelectMany(str => Result<int, string>.Fail(""));

            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void TestWhereSuccessPositive()
        {
            var val = "Test";
            var result = Result<string, string>.AsResult(val, "")
                                               .Where(str => str.StartsWith(val), () => "");

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(val, result.Get());
        }

        [TestMethod]
        public void TestWhereSuccessNegative()
        {
            var val = "Test";
            var result = Result<string, string>.AsResult(val, "")
                                               .Where(str => str.StartsWith("Wrong"), () => "");

            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void TestWhereFailure()
        {
            var result = Result<string, string>.AsResult(null, "")
                                               .Where(str => str.StartsWith("Wrong"), () => "");

            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void TestIterSuccess()
        {
            string valueToSet = "";

            var val = "Test";
            Result<string, string>.AsResult(val, "")
                                  .Iter(i => valueToSet = i);

            Assert.AreEqual(val, valueToSet);
        }

        [TestMethod]
        public void TestIterFailure()
        {
            var val = "";
            string valueToSet = val;
            Result<string, string>.AsResult(null, "")
                                  .Iter(i => valueToSet = "Test");

            Assert.AreEqual(val, valueToSet);
        }

        [TestMethod]
        public void TestTrySuccess()
        {
            var number = 12;
            var result = Result<int, string>.Try(() => int.Parse("" + number), e => e.ToString());

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(number, result.Get());
        }

        [TestMethod]
        public void TestTryFailure()
        {
            var notNumber = "strange number";
            var result = Result<int, string>.Try(() => int.Parse(notNumber), e => e.ToString());

            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void TestMatchSuccess()
        {
            var val = "Test";
            var result = Result<string, string>.AsResult(val, "")
                                               .Match(success => success, failure => failure);

            Assert.AreEqual(val, result);
        }

        [TestMethod]
        public void TestMatchFailure()
        {
            var f = "Failed";
            var result = Result<string, string>.AsResult(null, f)
                                               .Match(success => success, failure => failure);

            Assert.AreEqual(f, result);
        }

        [TestMethod]
        public void TestOrSuccess()
        {
            var val = "Test";
            var result = Result<string, string>.AsResult(val, "")
                                               .Or(() => "Something");

            Assert.AreEqual(val, result);
        }

        [TestMethod]
        public void TestOrFailure()
        {
            var val = "Test";
            var result = Result<string, string>.AsResult(null, "")
                                               .Or(() => val);

            Assert.AreEqual(val, result);
        }

        [TestMethod]
        public void TestOfTypeSuccessPositive()
        {
            var result = Result<string, string>.AsResult("Test", "")
                                               .OfType<ICloneable>(() => "");

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void TestOfTypeSuccessNegative()
        {
            var result = Result<string, string>.AsResult("Test", "")
                                               .OfType<int>(() => "");

            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void TestOfTypeFailure()
        {
            var result = Result<string, string>.AsResult(null, "")
                                               .OfType<ICloneable>(() => "");

            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public void TestOrResultSuccess()
        {
            var val = "Test";
            var result = Result<string, string>.AsResult(val, "")
                                       .Or(() => Result<string, string>.AsResult("Something", ""));

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(val, result.Get());
        }

        [TestMethod]
        public void TestOrResultFailure()
        {
            var val = "Test";
            var result = Result<string, string>.AsResult(null, "")
                                               .Or(() => Result<string, string>.AsResult(val, ""));

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(val, result.Get());
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utilities.Reactive;

namespace Utilities.Tests
{
    [TestClass]
    public class TestReactiveExtensions
    {
        [TestMethod]
        public void TestWhereIsNotNull()
        {
            var list = new List<string> {"a", null, "c"};

            Assert.AreEqual(2, list.ToObservable().WhereIsNotNull().ToEnumerable().Count());
        }
    }
}
using EcwidIntegration.GoogleSheets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EcwidIntegration.UnitTests
{
    [TestClass]
    public class GoogleSheetsTests
    {
        private GoogleSheetsService GetService()
        {
            return new GoogleSheetsService("17jXjpfnWocXgSC6NFb8iyROs2ibQhYibu4EJC7vrJVs");
        }

        [TestMethod]
        public void CreateAndDeleteSheet()
        {
            var sheetName = $"Test_{Guid.NewGuid().ToString()}";
            var service = GetService();
            var sheet = service.CreateSheet(sheetName);
            Assert.AreNotEqual(sheet, null);
            var removed = service.RemoveSheet(sheetName);
            Assert.AreEqual(removed, true);
        }

        [TestMethod]
        public void WriteData()
        {
            var sheetName = $"Test_{Guid.NewGuid().ToString()}";
            var service = GetService();
            service.CreateSheet(sheetName);
            var list = new List<object>() { "One", "Two", "three", "four", "five", "six", "seven" };
            var res = service.Write(sheetName, list, "B", "I");
            var items = service.Get(sheetName, "B", "I");
            Assert.AreEqual(items.Count, 1);
            Assert.AreEqual(list.Any(i => !items[0].Contains(i)), false);
            service.RemoveSheet(sheetName);
        }
    }
}

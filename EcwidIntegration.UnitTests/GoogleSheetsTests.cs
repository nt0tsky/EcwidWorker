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
        private SheetService GetService(string tabName)
        {
            return new SheetService(new SheetParams
            {
                SheetId = "19fNPnbkQ6pOw2_DzVbHTkAmZcDZRp3lk1id60LO1hSk",
                ApplicationName = "GoogleSheetsWriter_FCAA338E-426A-44CB-8474-200048847DBD",
                CredentialsName = "credentials.json",
                TabName = tabName
            });
        }

        [TestMethod]
        public void CreateAndDeleteSheet()
        {
            var tabName = $"Test_{Guid.NewGuid().ToString()}";
            var service = GetService(tabName);
            var sheet = service.CreateSheet(tabName);
            Assert.AreNotEqual(sheet, null);
            var removed = service.RemoveSheet(tabName);
            Assert.AreEqual(removed, true);
        }

        [TestMethod]
        public void WriteData()
        {
            var tabName = $"Test_{Guid.NewGuid().ToString()}";
            var service = GetService(tabName);
            service.CreateSheet(tabName);
            var list = new List<object>() { "One", "Two", "three", "four", "five", "six", "seven" };
            var res = service.Write(list, tabName, "B");
            var items = service.Get(tabName, list.Count());
            Assert.AreEqual(items.Count, 1);
            Assert.AreEqual(list.Any(i => !items[0].Contains(i)), false);
            service.RemoveSheet(tabName);
        }
    }
}

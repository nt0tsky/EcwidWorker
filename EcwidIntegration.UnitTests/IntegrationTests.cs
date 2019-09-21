﻿using EcwidIntegration.Ecwid;
using EcwidIntegration.GoogleSheets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EcwidIntegration.UnitTests
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void WriteNewOrdersTests()
        {
            var sheetName = $"TestSheet_${Guid.NewGuid()}";

            var ecwidService = new EcwidService(TestConstants.StoreId, TestConstants.StoreAPIKEY);
            var googleService = new GoogleSheetsService(TestConstants.Sheet);
            var sheet = googleService.CreateSheet(sheetName);
            Assert.AreNotEqual(sheet, null);
            var orderNumbers = googleService.GetOrdersNumbers(sheetName);
            Assert.AreEqual(orderNumbers.Count(), 0);
            var result = ecwidService.GetOrders(6).Result;
            Assert.AreEqual(result.Count(), 6);

            foreach (var item in result)
            {
                var list = new List<object>()
                    {
                        item.OrderNumber,
                        item.ShippingMethod,
                        item.CreateDate.ToString("dd.MM.yyyy"),
                        item.ShippingPerson
                    };
                list.AddRange(item.Items.Select(i => i.Name));

                googleService.Write(sheetName, list, "B");
            }

            var res = googleService.Get(sheetName, "B1", "B");
            Assert.AreEqual(res.Count(), 6);
            Assert.AreEqual(googleService.RemoveSheet(sheetName), true);
        }
    }
}
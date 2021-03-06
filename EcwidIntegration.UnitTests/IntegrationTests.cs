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
            var googleService = new SheetService(new SheetParams
            {
                SheetId = "19fNPnbkQ6pOw2_DzVbHTkAmZcDZRp3lk1id60LO1hSk",
                ApplicationName = "GoogleSheetsWriter_FCAA338E-426A-44CB-8474-200048847DBD",
                CredentialsName = "credentials.json"
            });
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

                googleService.Write(list, sheetName, "B");
            }

            var res = googleService.Get(sheetName, result.Count());
            Assert.AreEqual(res.Count(), 6);

            var orderNumbersRes = googleService.GetOrdersNumbers(sheetName);
            Assert.AreEqual(orderNumbersRes.Count(), 6);
            Assert.AreEqual(googleService.RemoveSheet(sheetName), true);

            
        }
    }
}

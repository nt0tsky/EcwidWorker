using EcwidIntegration.Ecwid;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace EcwidIntegration.UnitTests
{
    [TestClass]
    public class EcwidModuleTests
    {
        [TestMethod]
        public void GetNewOrdersTest()
        {
            var ecwidService = new EcwidService(TestConstants.StoreId, TestConstants.StoreAPIKEY);
            var result = ecwidService.GetPaidNotShippedOrdersAsync().Result;
            Assert.AreEqual(result.Any(), true);
        }
    }
}

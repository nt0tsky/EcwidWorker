using System.Collections.Generic;
using System.Linq;
using EcwidIntegration.DAL.Context;
using EcwidIntegration.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcwidIntegration.DAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Order>> Get()
        {
            using (var context = new ApplicationContext())
            {
                context.ChangeTracker.LazyLoadingEnabled = false;
                return context.Orders.Include(i => i.Items).ThenInclude(k => k.Options).ToList();
            }
        }
    }
}

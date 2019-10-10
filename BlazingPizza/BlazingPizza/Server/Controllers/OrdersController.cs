using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazingPizza.Server.Data;
using BlazingPizza.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazingPizza.Server.Controllers
{
    [Route("[controller]"), ApiController]
    [Authorize]
    public class OrdersController : Controller
    {
        public OrdersController(PizzaContext context)
        {
            Context = context;
        }

        public PizzaContext Context { get; }

        public async Task<List<OrderWithStatus>> GetOrders()
        {
            var orders = await Context.Orders
                //.Where(o => o.UserId == GetUserId())
                .Include(o => o.Pizzas).ThenInclude(p => p.Special)
                .Include(o => o.Pizzas).ThenInclude(p => p.Toppings).ThenInclude(t => t.Topping)
                .OrderByDescending(o => o.CreatedTime)
                .ToListAsync();

            return orders.ConvertAll(o => OrderWithStatus.FromOrder(o));
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderWithStatus>> GetOrderWithStatus(int orderId)
        {
            var order = await Context.Orders
                .Where(o => o.OrderId == orderId)
                //.Where(o => o.UserId == GetUserId())
                .Include(o => o.Pizzas).ThenInclude(p => p.Special)
                .Include(o => o.Pizzas).ThenInclude(p => p.Toppings).ThenInclude(t => t.Topping)
                .SingleOrDefaultAsync();

            if (order == null) return NotFound();

            return OrderWithStatus.FromOrder(order);
        }

        [HttpPost]
        public async Task<int> PostOrder(Order order)
        {
            order.CreatedTime = DateTime.Now;
            order.DeliveryLocation = new LatLong(51.5001, -0.1239);
            //order.UserId = GetUserId();

            Context.Orders.Attach(order);
            await Context.SaveChangesAsync();
            return order.OrderId;
        }

        private string GetUserId()
        {
            // This will be the user's twitter username
            return HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
        }
    }
}

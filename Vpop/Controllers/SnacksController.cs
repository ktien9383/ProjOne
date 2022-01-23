using Microsoft.AspNetCore.Mvc;
using Vpop.Data;
using Vpop.Models;
using System.Collections.Generic;
using System.Linq;
using Vpop.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Vpop.Controllers
{
    public class SnacksController : Controller
    {
        private OrderDbContext context;

        public SnacksController(OrderDbContext dbContext)
        {
            context = dbContext;
        }

        internal static Dictionary<string, string> Snacks1Choices = new Dictionary<string, string>()
        {
            {"Snow Crab Rangoons (3) ", "Snow Crab Rangoons (3) $5"},
            {"Snow Crab Rangoons (6) ", "Snow Crab Rangoons (6) $10"},
            {"Vegan Cheese Rolls ", "Vegan Cheese Rolls $4"},
            {"Fried Shrimp Rolls ", "Fried Shrimp Rolls $5"}
        };
        internal static Dictionary<string, string> Snacks2Choices = new Dictionary<string, string>()
        {
            {"Krab Rangoons(3) ", "Krab Rangoons(3) $2.8"},
            {"Krab Rangoons(6) ", "Krab Rangoons(6) $5"},
            {"Veggie Egg Rolls ", "Veggie Egg Rolls $3.5"},
            {"Vietnamese Egg Rolls ", "Vietnamese Egg Rolls $3.5"}
        };
        public IActionResult Display()
        {
            int orderId = context.Orders.Max(j => j.Id);
            ViewBag.order = context.Orders.Find(orderId);
            ViewBag.message = "Your order has been submitted. Below is your order summary";

            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            OrderSnacksViewModel orderSnacksViewModel = new OrderSnacksViewModel(Snacks1Choices, Snacks2Choices);

            return View(orderSnacksViewModel);
        }
        [HttpPost]
        public IActionResult Index(OrderSnacksViewModel orderSnacksViewModel)
        {
            if (ModelState.IsValid)
            {
                orderSnacksViewModel.Custname = HttpContext.Session.GetString("CustName"); 
                orderSnacksViewModel.Price = double.Parse(orderSnacksViewModel.Item.Split('$')[1]);
                orderSnacksViewModel.Item = orderSnacksViewModel.Item.Split('$')[0];

                Order newOrder = new Order
                {
                    Custname = orderSnacksViewModel.Custname,
                    Item = orderSnacksViewModel.Item,
                    Price = orderSnacksViewModel.Price,
                    Category = orderSnacksViewModel.Category,
                    Currdate = orderSnacksViewModel.Currdate
                };

                context.Orders.Add(newOrder);
                context.SaveChanges();

                return Redirect("/Snacks/Display");
            }

            OrderSnacksViewModel viewModel = new OrderSnacksViewModel(Snacks1Choices, Snacks2Choices);
            return View(viewModel);
        }
    }
}

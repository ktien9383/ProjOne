using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Vpop.Data;
using Vpop.Models;
using Vpop.ViewModels;

namespace Vpop.Controllers
{
    public class BowlsController : Controller
    {
        private OrderDbContext context;
        public BowlsController(OrderDbContext dbContext)
        {
            context = dbContext;
        }

        internal static Dictionary<string, string> CategoryChoices = new Dictionary<string, string>()
        {
            {"Salad Bowl", "Salad Bowl"},
            {"Vermicelli Bowl", "Vermicelli Bowl"},
            {"Rice Bowl", "Rice Bowl"}
        };

        internal static Dictionary<string, string> Protein1Choices = new Dictionary<string, string>()
        {
            {"Duck ", "Duck $15"},
            {"Combo ", "Combo $14"},
            {"Shrimp ", "Shrimp $12"},
            {"Steak ", "Steak $12"},
            {"Vegan Beef ", "Vegan Beef $12"},
        };

        internal static Dictionary<string, string> Protein2Choices = new Dictionary<string, string>()
        {
            {"Fish ", "Fish $12"},
            {"Pork ", "Pork $11"},
            {"Chicken ", "Chicken $11"},
            {"Tofu ", "Tofu $11"},
            {"Veggie ", "Veggie $11"}
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
            OrderBowlsViewModel orderBowlsViewModel = new OrderBowlsViewModel(CategoryChoices, Protein1Choices, Protein2Choices);

            return View(orderBowlsViewModel);
        }
        [HttpPost]
        public IActionResult Index(OrderBowlsViewModel orderBowlsViewModel)
        {
            if (ModelState.IsValid)
            {
                orderBowlsViewModel.Custname = HttpContext.Session.GetString("CustName"); 
                orderBowlsViewModel.Price = double.Parse(orderBowlsViewModel.Item.Split('$')[1]);
                orderBowlsViewModel.Item = orderBowlsViewModel.Item.Split('$')[0];

                Order newOrder = new Order
                {
                    Custname = orderBowlsViewModel.Custname,
                    Item = orderBowlsViewModel.Item,
                    Price = orderBowlsViewModel.Price,
                    Category = orderBowlsViewModel.Category,
                    Currdate = orderBowlsViewModel.Currdate
                };

                context.Orders.Add(newOrder);
                context.SaveChanges();

                return Redirect("/Bowls/Display");
            }

            OrderBowlsViewModel viewModel = new OrderBowlsViewModel(CategoryChoices, Protein1Choices, Protein2Choices);
            return View(viewModel);
        }

    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Vpop.Data;
using Vpop.Models;

namespace Vpop.Controllers
{
    public class ReviewController : Controller
    {
        private OrderDbContext context;
        public ReviewController(OrderDbContext dbContext)
        {
            context = dbContext;
        }

        //Create a new dictionary to hold the data for a specific customer 
        public static Dictionary<int, Order> OrderCust = new Dictionary<int, Order>();
        public IActionResult Index()
        {
            string custname = HttpContext.Session.GetString("CustName"); 
            List<Order> orders = context.Orders.Where(p => p.Custname == custname).ToList();
            double totalprice = context.Orders.Where(p => p.Custname == custname)
                                              .Sum(p => p.Price);

            ViewBag.title = "Order Summary";
            ViewBag.orders = orders;
            //ViewBag.totalprice = totalprice;
            if (orders.Count > 0)
            {
                ViewBag.custname = orders[0].Custname;
                ViewBag.currdate = orders[0].Currdate;
            }
            return View();
        }
        public IActionResult Determine(int[] cancelOrderIds, int updateOrderId, string cmd)
        {
            if (cmd == "Cancel")
            {
                foreach (int orderId in cancelOrderIds)
                {
                    Order theOrder = context.Orders.Find(orderId);
                    context.Orders.Remove(theOrder);
                }

                context.SaveChanges();

                return Redirect("/Review/Index");
            }
            else if (cmd == "Update")
            {
                ViewBag.Title = "Edit Orders";
                Order order = context.Orders.Find(updateOrderId);
                if (order != null)
                {
                    ViewBag.order = order;

                    if (ViewBag.order.Category == "Banh Mi")
                    {
                        ViewBag.protein1 = BanhMiController.Protein1Choices;
                        ViewBag.protein2 = BanhMiController.Protein2Choices;
                    }
                    else if (ViewBag.order.Category == "Snacks")
                    {
                        ViewBag.snacks1 = SnacksController.Snacks1Choices;
                        ViewBag.snacks2 = SnacksController.Snacks2Choices;
                    }
                    else if (ViewBag.order.Category == "Salad Bowl" || ViewBag.order.Category == "Vermicelli Bowl" || ViewBag.order.Category == "Rice Bowl")
                    {
                        ViewBag.categories = BowlsController.CategoryChoices;
                        ViewBag.proteins1 = BowlsController.Protein1Choices;
                        ViewBag.proteins2 = BowlsController.Protein2Choices;
                    }
                    return View("Edit");
                }
                else
                {
                    return Redirect("/Review/Index");
                }
            }
            else
            {
                string custname = HttpContext.Session.GetString("CustName");
                List<Order> orders = context.Orders.Where(p => p.Custname == custname).ToList();
                double totalprice = context.Orders.Where(p => p.Custname == custname)
                                                  .Sum(p => p.Price);

                ViewBag.title = "Payment Summary";
                ViewBag.orders = orders;
                ViewBag.totalprice = totalprice;
                if (orders.Count > 0)
                {
                    ViewBag.custname = orders[0].Custname;
                    ViewBag.currdate = orders[0].Currdate;
                }
                return View("Checkout");

            }
        }
        public IActionResult Save(int id, string custname, string category, string item, string currdate)
        {
            ViewBag.Title = "Save Orders";
            double priceTemp = double.Parse(item.Split('$')[1]);
            string itemTemp = item.Split('$')[0];
            Order order = context.Orders.Find(id);
            order.Category = category;
            order.Item = itemTemp;
            order.Price = priceTemp;
            order.Currdate = currdate;

            context.Update(order);
            context.SaveChanges();

            ViewBag.order = order;
            ViewBag.message = "Your order has been modified. Below is the revised order";
            return View();
        }
    }
}

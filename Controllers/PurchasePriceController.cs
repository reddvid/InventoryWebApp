using ASPNETWebApp48.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ASPNETWebApp48.Controllers
{
    [Authorize]
    public class PurchasePriceController : Controller
    {
        InventoryDbContext _db = new InventoryDbContext();
        public ActionResult Manage()
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                var items = _db.PurchasePrices
                    .OrderByDescending(x => x.Id)
                    .ToList();

                ViewBag.PriceDate = DateTime.Now; //for date

                ViewBag.ForCreatePartial = new Dictionary<string, object>
                {
                    { "ProductLookup", _db.Products.Include(x => x.Brand).ToList() }
                };

                return View(items);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "Purchase manager can access purchase order";

            return View();
        }

        public ActionResult GetProductPrice(int Id) // Product.Id
        {
            var unitPrice = _db.Products.Find(Id).PurchasePrice;
            return Json(unitPrice, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PurchasePrice adjustprice)
        {
            if (ModelState.IsValid)
            {
                if (_db.PurchasePrices.Any(x => x.ProductId == adjustprice.ProductId))
                    TempData["alertbox"] = "Product already exists.";
                else
                {
                    var product = _db.Products.Find(adjustprice.ProductId);
                    product.PurchasePrice = adjustprice.UpdatedPrice;
                    _db.Entry(product).State = EntityState.Modified;
                    string username = Session["user"] as string;
                    LogActivity(username, "Purchase Price", "Adjust Price");
                    _db.PurchasePrices.Add(adjustprice);
                    _db.SaveChanges();
                }
                TempData["alertbox"] = "Purchase Price has been added.";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        private void LogActivity(string username, string activity, string action)
        {
            // Create a new ActivityLog object
            ActivityLog log = new ActivityLog
            {
                Username = username,
                Activity = activity,
                Action = action,
                DateTime = DateTime.Now
            };

            // Add the log to your database
            _db.ActivityLogs.Add(log);
            _db.SaveChanges();
        }
    }
}
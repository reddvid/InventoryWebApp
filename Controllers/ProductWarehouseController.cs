using ASPNETWebApp48.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPNETWebApp48.Controllers
{
    [Authorize]
    public class ProductWarehouseController : Controller
    {
        InventoryDbContext _db = new InventoryDbContext();
        public ActionResult Manage()
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                var products = _db.ProductWarehouses
                    .OrderBy(x => x.Name)
                    .ToList();

                ViewBag.ForCreatePartial = new Dictionary<string, object>
                {
                    { "CategoryLookup", _db.Categories.OrderBy(x => x.Name).ToList() },
                    { "BrandLookup", _db.Brands.OrderBy(x => x.Name).ToList() }
                };

                return View(products);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "purchase & admin manager can access product";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductWarehouse productwh, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                if (_db.ProductWarehouses.Any(x => x.Name == productwh.Name))
                    TempData["alertbox"] = "Product name already exists.";
                else
                {
                    if (fileUpload != null)
                        productwh.PictureFilename = fileUpload.SaveAsImageFile(productwh.Name);
                    string username = Session["user"] as string;
                    LogActivity(username, "Product Warehouse", "Create");
                    _db.ProductWarehouses.Add(productwh);
                    _db.SaveChanges();
                    TempData["alertbox"] = "Product has been added.";
                }
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Manage");
        }

        public ActionResult Edit(int id)
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                ProductWarehouse product = _db.ProductWarehouses.Find(id);

                ViewBag.Brand = _db.Brands.ToList();
                ViewBag.Category = _db.Categories.ToList();

                if (product == null)
                {
                    TempData["alertbox"] = "Product does not exist.";
                    return RedirectToAction("Manage");
                }
                return View(product);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "purchase & admin manager can access product warehouse";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductWarehouse updatedProduct, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(updatedProduct).State = EntityState.Modified;

                if (fileUpload != null)
                    updatedProduct.PictureFilename = fileUpload.SaveAsImageFile(updatedProduct.Name);
                else
                    _db.Entry(updatedProduct).Property(x => x.PictureFilename).IsModified = false;

                string username = Session["user"] as string;
                LogActivity(username, "Product Warehouse", "Edit Product");

                _db.SaveChanges();
                TempData["alertbox"] = "Product warehouse changes have been successfully saved.";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Manage");
        }

        public ActionResult Inventory()
        {
            if (User.IsInRole("admin") || User.IsInRole("wrhmgr") || User.IsInRole("purchmgr"))
            {
                var items = _db.ProductWarehouses
                    .Include(x => x.ReceivingApproveItems)
                    .Include(x => x.PurchaseReturnItems)
                    .Include(x => x.TransferItemStores)
                    .Include(x => x.TransferItemWarehouses)
                    .Include(x => x.AdjustItems)
                    .OrderBy(x => x.Name)
                    .ToList();

                return View(items);
            }
            else if (User.IsInRole("salesmgr"))
                ViewBag.Message = "admin manager can access inventory warehouse";

            return View();
        }

        public ActionResult Print()
        {
            var items = _db.ProductWarehouses
                .Include(x => x.ReceivingApproveItems)
                .Include(x => x.PurchaseReturnItems)
                .Include(x => x.TransferItemStores)
                .Include(x => x.TransferItemWarehouses)
                .Include(x => x.AdjustItems)
                .OrderBy(x => x.Name)
                .ToList();

            return new Rotativa.ViewAsPdf(items);
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
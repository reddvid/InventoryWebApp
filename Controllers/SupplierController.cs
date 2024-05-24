using ASPNETWebApp48.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ASPNETWebApp48.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        readonly InventoryDbContext _db = new InventoryDbContext();
        public ActionResult Manage()
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                var suppliers = _db.Suppliers
                    .OrderByDescending(x => x.Id)
                    .ToList();

                return View(suppliers);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "Purchase manager can access supplier";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                if (_db.Suppliers.Any(r => r.Name == supplier.Name))
                    TempData["alertbox"] = "Supplier name has already exists.";
                else
                {
                    _db.Suppliers.Add(supplier);
                    string username = Session["user"] as string;
                    LogActivity(username, "Supplier", "Create");
                    _db.SaveChanges();
                    TempData["alertbox"] = "Supplier has been added.";
                }
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Manage");
        }

        public ActionResult Edit(int Id)
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                Supplier supplier = _db.Suppliers.Find(Id);

                if (supplier == null)
                {
                    TempData["alertbox"] = "Supplier does not exist.";
                    return RedirectToAction("Manage");
                }

                return View(supplier);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "Purchase manager can access supplier";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Supplier updatedSupplier)
        {
            if (ModelState.IsValid)
            {
                string username = Session["user"] as string;
                LogActivity(username, "Supplier", "Update Detail");
                _db.Entry(updatedSupplier).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["alertbox"] = "Supplier changes have been successfully saved.";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Manage");
        }

        public ActionResult Deactivate(int Id)
        {
            Supplier supplier = _db.Suppliers.Find(Id);
            supplier.Status = "Inactive";
            string username = Session["user"] as string;
            LogActivity(username, "Supplier", "Deactivate Supplier");
            _db.Entry(supplier).State = EntityState.Modified;
            _db.SaveChanges();
            TempData["alertbox"] = "Supplier changes have been successfully saved.";
            return RedirectToAction("Manage");
        }

        public ActionResult Activate(int Id)
        {
            Supplier supplier = _db.Suppliers.Find(Id);
            supplier.Status = "Active";
            string username = Session["user"] as string;
            LogActivity(username, "Supplier", "Activate Supplier");
            _db.Entry(supplier).State = EntityState.Modified;
            _db.SaveChanges();
            TempData["alertbox"] = "Supplier changes have been successfully saved.";
            return RedirectToAction("Manage");
        }

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
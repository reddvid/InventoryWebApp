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
    public class PurchaseController : Controller
    {
        InventoryDbContext _db = new InventoryDbContext();
        public ActionResult Manage()
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                var purchases = _db.Purchases
                    .Include(x => x.PurchaseItems)
                    .OrderByDescending(x => x.Id)
                    .ToList();

                ViewBag.OrderDate = DateTime.Now;

                var filteredSuppliers = _db.ProductSuppliers
                    .Include(x => x.Supplier)
                    .Select(ps => ps.Supplier)
                    .Where(r => r.Status == "Active")
                    .Distinct()
                    .ToList();

                ViewBag.SupplierLookup = filteredSuppliers;

                ViewBag.ForCreatePartial = new Dictionary<string, object>
                {
                    //{ "SupplierLookup", filteredSuppliers }
                };

                return View(purchases);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "Purchase manager can access purchase order";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Purchase purchase)
        {
            string username = Session["user"] as string;
            LogActivity(username, "Purchase", "Create");
            _db.Purchases.Add(purchase);
            _db.SaveChanges();

            return RedirectToAction("Get", new { Id = purchase.Id });
        }

        public ActionResult Get(int Id) // Change int to int?
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                var purchase = _db.Purchases
                    .Where(x => x.Id == Id)
                    .Include(x => x.Supplier)
                    .Include(x => x.PurchaseItems)
                    .OrderByDescending(x => x.Id)
                    .SingleOrDefault();

                var receiving = _db.Receivings
                    .Where(x => x.PurchaseId == Id)
                    .SingleOrDefault();

                if (receiving != null && receiving.Status.Contains("Pending"))
                {
                    TempData["alertbox"] = "Already Receiving cannot visit again";
                    return RedirectToAction("Manage");
                }
                if (receiving != null && receiving.Status.Contains("Received"))
                {
                    TempData["alertbox"] = "Already Receiving cannot visit again";
                    return RedirectToAction("Manage");
                }
                if (receiving != null && receiving.Status.Contains("Complete"))
                {
                    TempData["alertbox"] = "Already Receiving cannot visit again";
                    return RedirectToAction("Manage");
                }
                if (receiving != null && receiving.Status.Contains("Closed"))
                {
                    TempData["alertbox"] = "Already Receiving cannot visit again";
                    return RedirectToAction("Manage");
                }
                if (purchase == null)
                {
                    TempData["alertbox"] = "Purchase Not Exist";
                    return RedirectToAction("Manage");
                }

                var sup = _db.ProductSuppliers
                    .Include(x => x.Product)
                    .Include(x => x.Product.Brand)
                    .ToList();

                ViewBag.OrderDate = DateTime.Now;
                ViewBag.Supplier = _db.Suppliers.ToList();

                ViewBag.ForAddPurchaseItemPartial = new Dictionary<string, object>
                {
                    { "PurchaseId", purchase.Id },
                    { "ProductLookup", sup }
                };

                return View(purchase);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "Purchase manager can access purchase order";

            return View();
        }

        // This action returns products based on the selected supplier
        public ActionResult GetProductsBySupplier(int supplierId)
        {
            var products = _db.ProductSuppliers
                .Where(p => p.SupplierId == supplierId)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Product.Name
                })
                .ToList();

            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProductPrice(int Id) // Product.Id
        {
            var unitPrice = _db.Products.Find(Id).PurchasePrice;
            return Json(unitPrice, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreatePurchaseItem(PurchaseItem recievesItem)
        {
            // Assuming you have a SupplierId or ProductSupplierId in recievesItem to find the ProductSupplier
            int productSupplierId = recievesItem.ProductSupId; // Adjust this to match your actual property
            // Find the ProductSupplier
            var prodsupplier = _db.ProductSuppliers.FirstOrDefault(ps => ps.Id == productSupplierId);
            // Assign the ProductId from the ProductSupplier to the PurchaseItem
            recievesItem.ProductId = prodsupplier.ProductId;
            string username = Session["user"] as string;
            LogActivity(username, "Purchase", "Add Item");
            _db.PurchaseItems.Add(recievesItem);
            _db.SaveChanges();
            return RedirectToAction("Get", new { Id = recievesItem.PurchaseId });
        }

        [HttpPost]
        public ActionResult EditDetail(int Id, PurchaseItem removeitems)
        {
            if (!ModelState.IsValid)
            {
                var purchase = _db.Purchases
                .Where(x => x.Id == Id)
                .Include(x => x.PurchaseItems)
                .Include(x => x.Supplier)
                .OrderByDescending(x => x.Id)
                .SingleOrDefault();

                if (purchase.PurchaseItems == null || purchase.PurchaseItems.Count == 0)
                {
                    TempData["alertbox"] = "No items in the purchase order.";
                    return RedirectToAction("Manage");
                }

                var itemToRemove = purchase.PurchaseItems.FirstOrDefault(item => item.Id == removeitems.Id);
                if (itemToRemove == null)
                {
                    TempData["alertbox"] = "Item to remove not found.";
                    return RedirectToAction("Manage");
                }

                string username = Session["user"] as string;
                LogActivity(username, "Purchase", "Edit Detail");

                _db.PurchaseItems.Remove(itemToRemove);
                _db.SaveChanges();

                _db.Entry(purchase).State = EntityState.Modified;
                _db.SaveChanges();

                TempData["alertbox"] = "Order details changes have been successfully saved.";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Manage");        
        }

        public ActionResult DelItem(int id)
        {
            var purItem = _db.PurchaseItems.Find(id);
            if (purItem != null)
            {
                string username = Session["user"] as string;
                LogActivity(username, "Purchase", "Removed Item");
                _db.PurchaseItems.Remove(purItem);
                _db.SaveChanges();
            }

            return RedirectToAction("Get", new { Id = purItem.PurchaseId });
        }

        private Purchase GetPurchaseDetails(int purchaseId)
        {
            return _db.Purchases
                .Include(x => x.PurchaseItems)
                .Include(x => x.Supplier)
                .SingleOrDefault(x => x.Id == purchaseId);
        }

        public ActionResult Detail(int Id) //for detail
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                var purchase = _db.Purchases
                    .Where(x => x.Id == Id)
                    .Include(x => x.PurchaseItems)
                    .Include(x => x.Supplier)
                    .OrderByDescending(x => x.Id)
                    .SingleOrDefault();

                if (purchase == null)
                {
                    TempData["alertbox"] = "Purchase Not Exist";
                    return RedirectToAction("Manage");
                }

                return View(purchase);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "Purchase manager can access purchase order";

            return View();
        }

        public ActionResult Send(int Id)
        {
            var purchase = _db.Purchases
                .Where(x => x.Id == Id)
                .Include(x => x.PurchaseItems)
                .Include(x => x.Supplier)
                .OrderByDescending(x => x.Id)
                .SingleOrDefault();

            var sup = _db.ProductSuppliers.Include(x => x.Product).ToList();
            var brand = _db.Brands.ToList();

            if (purchase.PurchaseItems == null || purchase.PurchaseItems.Count == 0)
                TempData["alertbox"] = "No items in the purchase order";

            var pdfViewResult = new Rotativa.ViewAsPdf("Print", purchase)
            {
                FileName = "PurchaseOrder.pdf"
            };

            var pdfAttachment = new EmailService.EmailAttachment()
            {
                Data = pdfViewResult.BuildPdf(this.ControllerContext),
                ContentType = "application/pdf",
                FileName = pdfViewResult.FileName
            };

            bool result = EmailService.SendEmail("joseduke325@gmail.com", "- PO PDF Email", "Hello! This is Attachment for purchase order pdf file.", pdfAttachment);
            if (result)
                TempData["alertbox"] = "PDF Email successfully sent.";
            else
                TempData["alertbox"] = "Sending failed.";

            return RedirectToAction("Detail", new { Id = purchase.Id });
        }

        public ActionResult Print(int Id) //for pdf
        {
            var purchase = _db.Purchases
                .Where(x => x.Id == Id)
                .Include(x => x.PurchaseItems)
                .Include(x => x.Supplier)
                .OrderByDescending(x => x.Id)
                .SingleOrDefault();

            var sup = _db.ProductSuppliers.Include(x => x.Product).ToList();
            var brand = _db.Brands.ToList();

            if (purchase.PurchaseItems == null || purchase.PurchaseItems.Count == 0)
            {
                TempData["alertbox"] = "No items in the order";
                return RedirectToAction("Manage");
            }
            if (purchase == null)
            {
                TempData["alertbox"] = "Purchase Not Exist";
                return RedirectToAction("Manage");
            }

            return new Rotativa.ViewAsPdf(purchase);
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
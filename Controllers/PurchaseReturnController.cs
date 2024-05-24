using ASPNETWebApp48.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ASPNETWebApp48.ViewModels;

namespace ASPNETWebApp48.Controllers
{
    [Authorize]
    public class PurchaseReturnController : Controller
    {
        InventoryDbContext _db = new InventoryDbContext();
        public ActionResult Manage()
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                var returns = _db.PurchaseReturns
                    .Include(x => x.PurchaseReturnItems)
                    .Include(x => x.Supplier)
                    .Include(x => x.Purchase)
                    .OrderByDescending(x => x.Id)
                    .ToList();

                // Retrieve purchase IDs that are already in return
                var purchaseIdsInReceiving = _db.PurchaseReturns.Select(r => r.PurchaseId).ToList();

                var availablePurchases = _db.Purchases
                    .Include(x => x.Supplier)
                    .Include(x => x.PurchaseItems) // Include the purchase items if needed
                    .Where(p => p.PurchaseItems.Sum(pi => pi.Quantity) >= 5 && !purchaseIdsInReceiving.Contains(p.Id))
                    .ToList();

                ViewBag.ReturnDate = DateTime.Now;
                ViewBag.ForCreatePartial = new Dictionary<string, object>
                {
                    { "PurchaseLookup", availablePurchases }
                };

                return View(returns);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "Purchase manager can access purchase return";

            return View();
        }

        [HttpPost]
        public ActionResult Create(PurchaseReturn returns)
        {
            var purchase = _db.Purchases
                .Include(p => p.Supplier)
                .FirstOrDefault(p => p.Id == returns.PurchaseId);

            // If the purchase is found, set the SupplierId in the returns object
            if (purchase != null)
            {
                returns.SupplierId = purchase.Supplier.Id;
            }

            string username = Session["user"] as string;
            LogActivity(username, "Purchase Return", "Create");
            _db.PurchaseReturns.Add(returns);
            _db.SaveChanges();
            return RedirectToAction("Get", new { Id = returns.Id });
        }

        public ActionResult Get(int Id) // Order.Id
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                var returns = _db.PurchaseReturns
                    .Where(x => x.Id == Id)
                    .Include(x => x.PurchaseReturnItems)
                    .Include(x => x.Supplier)
                    .SingleOrDefault();

                var prod = _db.Products.ToList();

                // Get the PurchaseId from the returns entry
                var purchaseId = returns.PurchaseId;

                // Fetch the PurchaseItems associated with the specific PurchaseId and include the Product data
                var items = _db.PurchaseItems
                    .Where(pi => pi.PurchaseId == purchaseId)
                    .Include(pi => pi.Product)
                    .ToList();

                // Preparing the ViewBag data
                ViewBag.ForAddItemPartial = new Dictionary<string, object>
                {
                    { "ProductLookup", items },
                    { "ReturnId", returns.Id }
                };

                return View(returns);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "Purchase manager can access purchase return";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateItem(PurchaseReturnItem purchaseReturn)
        {
            if (ModelState.IsValid)
            {
                // Find the product associated with the purchase return
                var product = _db.Products.Find(purchaseReturn.ProductId);

                if (product != null)
                {
                    // Find the product warehouse by the product's name
                    var productWarehouse = _db.ProductWarehouses
                        .FirstOrDefault(x => x.Name == product.Name);

                    if (productWarehouse != null)
                    {
                        // Validate that the PURCHASE ITEMS has enough stock
                        var orderItems = _db.PurchaseItems.Where(x => x.ProductId == purchaseReturn.ProductId).ToList();
                        int totalQuantity = orderItems.Sum(x => x.Quantity);

                        if (purchaseReturn.Quantity <= totalQuantity)
                        {
                            // Set the WarehouseId of the return to the main product's ID
                            purchaseReturn.WarehouseId = productWarehouse.Id;

                            // Log the activity
                            string username = Session["user"] as string;
                            LogActivity(username, "Purchase Return", "Add Item");
                            TransactionActivity("Purchase Return", purchaseReturn.Quantity);
                            _db.PurchaseReturnItems.Add(purchaseReturn);
                            _db.SaveChanges();

                            TempData["alertbox"] = "Purchase Return created successfully.";
                        }
                        else
                            TempData["alertbox"] = "There is insufficient stock for this product.";
                    }
                    else
                        TempData["alertbox"] = "Product warehouse not found with name: " + product.Name;
                }
                else
                    TempData["alertbox"] = "Product not found with ID: " + purchaseReturn.ProductId;
            }
            else
                TempData["alertcard"] = "There are some validation errors.";

            return RedirectToAction("Get", new { Id = purchaseReturn.ReturnId });
        }

        public ActionResult DelItem(int id)
        {
            var orderItem = _db.PurchaseReturnItems.Find(id);
            if (orderItem != null)
            {
                string username = Session["user"] as string;
                LogActivity(username, "Purchase Return", "Delete Item");
                TransactionActivity("Purchase Return", orderItem.Quantity);
                _db.PurchaseReturnItems.Remove(orderItem);
                _db.SaveChanges();
            }

            return RedirectToAction("Get", new { Id = orderItem.ReturnId });
        }

        public ActionResult Cancel()
        {
            // Retrieve order item IDs from the session, if available
            List<int> orderItemIds = Session["orderItemIds"] as List<int>;

            if (orderItemIds != null && orderItemIds.Any())
            {
                foreach (int itemId in orderItemIds)
                {
                    var orderItem = _db.PurchaseReturnItems.Find(itemId);
                    if (orderItem != null)
                    {
                        _db.PurchaseReturnItems.Remove(orderItem);
                    }
                }

                string username = Session["user"] as string;
                LogActivity(username, "Purchase Return", "Cancel");
                Session.Remove("orderItemIds");
                _db.SaveChanges();
                TempData["alertbox"] = "PO return canceled." + Session["orderItemIds"];
            }
            else
                TempData["alertbox"] = "There are no adjustment item to cancel." + Session["orderItemIds"];

            return RedirectToAction("Manage");
        }

        [HttpPost]
        public ActionResult EditDetail(PurchaseReturn updatedOrder)
        {
            if (ModelState.IsValid)
            {
                if (Session["orderItemIds"] != null)
                {
                    // If orderItemIds exist in the session, log the activity
                    string username = Session["user"] as string;
                    LogActivity(username, "Purchase Return", "Edit Detail");
                    _db.Entry(updatedOrder).State = EntityState.Modified;
                    _db.SaveChanges();
                }
                else
                {
                    // If orderItemIds do not exist in the session, update the order status
                    string username = Session["user"] as string;
                    LogActivity(username, "Purchase Return", "Edit Detail");
                    _db.Entry(updatedOrder).State = EntityState.Modified;
                    _db.SaveChanges();
                }
                Session.Remove("orderItemIds");
                TempData["alertbox"] = "PO return changes have been successfully saved.";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Manage");
        }

        public ActionResult ManageApproval()
        {
            if (User.IsInRole("admin"))
            {
                var returns = _db.PurchaseReturns
                    .Include(x => x.PurchaseReturnItems)
                    .Include(x => x.Supplier)
                    .Include(x => x.Purchase)
                    .Where(x => x.Status == "Pending")
                    .OrderByDescending(x => x.Id)
                    .ToList();

                ViewBag.ApproveDate = DateTime.Now; //for date
                ViewBag.RejectDate = DateTime.Now; //for date

                return View(returns);
            }
            else if (User.IsInRole("salesmgr") || User.IsInRole("wrhmgr") || User.IsInRole("purchmgr"))
                ViewBag.Message = "purchase manager can access return approval";

            return View();
        }

        [HttpPost]
        public ActionResult Approve(int Id)
        {
            if (ModelState.IsValid)
            {
                var returns = _db.PurchaseReturns.Where(x => x.Id == Id).SingleOrDefault();

                string username = Session["user"] as string;
                LogActivity(username, "Purchase Return", "Approval Approved");
                returns.Status = "Reject"; // Update to the appropriate new status
                _db.Entry(returns).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["alertbox"] = "Release Return will be Approved";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Detail", new { Id });
        }

        [HttpPost]
        public ActionResult Reject(int Id)
        {
            if (ModelState.IsValid)
            {
                var order = _db.PurchaseReturns.Where(x => x.Id == Id).FirstOrDefault();
                string username = Session["user"] as string;
                LogActivity(username, "Purchase Return", "Approval Reject");
                order.Status = "Reject"; // Update to the appropriate new status
                _db.Entry(order).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["alertbox"] = "Release Return will be Reject";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Manage");
        }

        public ActionResult Detail(int Id) //for invoice detail
        {
            if (User.IsInRole("admin"))
            {
                var returns = _db.PurchaseReturns
                    .Include(x => x.Supplier)
                    .Where(x => x.Id == Id)
                    .SingleOrDefault();

                return View(returns);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr") || User.IsInRole("purchmgr"))
                ViewBag.Message = "admin can access detail of po return";

            return View();
        }

        public ActionResult Send(int Id)
        {
            var returns = _db.PurchaseReturns
                .Include(x => x.PurchaseReturnItems)
                .Include(x => x.Supplier)
                .Where(x => x.Id == Id)
                .SingleOrDefault();

            var prod = _db.Products.ToList();

            if (returns.PurchaseReturnItems == null || returns.PurchaseReturnItems.Count == 0)
                TempData["alertbox"] = "No items in the purchase return";

            var pdfViewResult = new Rotativa.ViewAsPdf("Print", returns)
            {
                FileName = "PurchaseReturn.pdf"
            };

            var pdfAttachment = new EmailService.EmailAttachment()
            {
                Data = pdfViewResult.BuildPdf(this.ControllerContext),
                ContentType = "application/pdf",
                FileName = pdfViewResult.FileName
            };

            bool result = EmailService.SendEmail("joseduke325@gmail.com", "- PO Return PDF", "Hello! This is Attachment for purchase return pdf file.", pdfAttachment);
            if (result)
                TempData["alertbox"] = "PDF Email successfully sent.";
            else
                TempData["alertbox"] = "Sending failed.";

            return RedirectToAction("Detail", new { Id = returns.Id });
        }

        public ActionResult Print(int Id) //for pdf
        {
            var returns = _db.PurchaseReturns
                .Include(x => x.PurchaseReturnItems)
                .Include(x => x.Supplier)
                .Where(x => x.Id == Id)
                .SingleOrDefault();

            var prod = _db.Products.ToList();

            if (returns == null)
            {
                TempData["alertbox"] = "purchase return not found";
                return RedirectToAction("Manage");
            }
            else if (returns.PurchaseReturnItems == null || returns.PurchaseReturnItems.Count == 0)
            {
                TempData["alertbox"] = "No items in the purchase return";
                return RedirectToAction("Manage");
            }

            return new Rotativa.ViewAsPdf(returns);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        private void TransactionActivity(string activity, int qty)
        {
            var products = _db.Products
                .Include(x => x.ReleaseItems)
                .Include(x => x.TransferItemStores)
                .Include(x => x.AdjustItems)
                .Include(x => x.PurchaseReturnItems)
                .Include(x => x.TransferItemWarehouses)
                .ToList();

            var productwhs = _db.ProductWarehouses
                .Include(x => x.ReceivingApproveItems)
                .Include(x => x.PurchaseReturnItems)
                .Include(x => x.TransferItemStores)
                .Include(x => x.TransferItemWarehouses)
                .Include(x => x.AdjustItems)
                .ToList();

            // Sum the stock quantities of all products
            decimal totalStockOnHandmain = products.Sum(x => x.StockOnHand);
            decimal totalStockOnHandWh = productwhs.Sum(x => x.StockOnHand);

            // Create a new ActivityLog object
            TransactionLodge log = new TransactionLodge
            {
                Activity = activity,
                Quantity = qty,
                Date = DateTime.Now,
                CurrentStoreQty = totalStockOnHandmain, // Assuming CurrentStoreQty is a decimal representing the total stock on hand
                CurrentWarehouseQty = totalStockOnHandWh
            };

            // Add the log to your database
            _db.TransactionLodges.Add(log);
            _db.SaveChanges();
        }
    }
}
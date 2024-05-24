using ASPNETWebApp48.Models;
using ASPNETWebApp48.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using X.PagedList;

namespace ASPNETWebApp48.Controllers
{
    [Authorize]
    public class ReleaseReturnController : Controller
    {
        InventoryDbContext _db = new InventoryDbContext();
        public ActionResult Manage()
        {
            if (User.IsInRole("admin") || User.IsInRole("salesmgr"))
            {
                var returns = _db.ReleaseReturns
                    .Include(x => x.Releasing)
                    .Include(x => x.ReleaseReturnItems)
                    .OrderByDescending(x => x.Id)
                    .ToList();

                // Retrieve purchase IDs that are already in receiving
                var purchaseIdsInReceiving = _db.ReleaseReturns.Select(r => r.ReleaseId).ToList();

                var availableReturns = _db.Releasings
                    .Include(x => x.ReleaseItems) // Include the purchase items if needed
                    .Where(p => p.ReleaseItems.Sum(pi => pi.Quantity) >= 5 && !purchaseIdsInReceiving.Contains(p.Id))
                    .ToList();

                ViewBag.ReturnDate = DateTime.Now;
                ViewBag.ForCreatePartial = new Dictionary<string, object>
                {
                    { "ReleaseLookup", availableReturns }
                };

                return View(returns);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("purchmgr"))
                ViewBag.Message = "releaser manager can access releasing return";

            return View();
        }

        [HttpPost]
        public ActionResult Create(ReleaseReturn returns)
        {
            var purchase = _db.Purchases
                .Include(p => p.Supplier)
                .FirstOrDefault(p => p.Id == returns.ReleaseId);

            string username = Session["user"] as string;
            LogActivity(username, "Release Return", "Create");
            _db.ReleaseReturns.Add(returns);
            _db.SaveChanges();
            return RedirectToAction("Get", new { Id = returns.Id });
        }

        public ActionResult Get(int Id) // Order.Id
        {
            if (User.IsInRole("admin") || User.IsInRole("salesmgr"))
            {
                var returns = _db.ReleaseReturns
                    .Where(x => x.Id == Id)
                    .Include(x => x.Releasing)
                    .Include(x => x.ReleaseReturnItems)
                    .SingleOrDefault();

                var prod = _db.Products.ToList();

                // Get the PurchaseId from the returns entry
                var purchaseId = returns.ReleaseId;

                // Fetch the PurchaseItems associated with the specific PurchaseId and include the Product data
                var items = _db.ReleaseItems
                    .Where(pi => pi.ReleaseId == purchaseId)
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
            else if (User.IsInRole("wrhmgr") || User.IsInRole("purchmgr"))
                ViewBag.Message = "Purchase manager can access purchase return";

            return View();
        }

        //public ActionResult GetProductPrice(int Id) // Product.Id
        //{
        //    var unitPrice = _db.ReleaseItems.Find(Id).SellingPrice;
        //    return Json(unitPrice, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateItem(ReleaseReturnItem returnDamage)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the corresponding order items for the product ID
                var orderItems = _db.ReleaseItems.Where(x => x.ProductId == returnDamage.ProductId).ToList();
                int totalQuantity = orderItems.Sum(x => x.Quantity);

                // Check if the quantity being returned is valid
                if (returnDamage.Quantity <= totalQuantity)
                {
                    _db.ReleaseReturnItems.Add(returnDamage);
                    string username = Session["user"] as string;
                    LogActivity(username, "Releasing Return", "Add Item");
                    TempData["alertbox"] = "Release Damage has been successfully added.";
                    _db.SaveChanges();  // Save changes for returnDamages
                }
                else
                    TempData["alertbox"] = "Return quantity exceeds the total quantity in orders.";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Get", new { Id = returnDamage.Id });
        }

        public ActionResult DelItem(int id)
        {
            var orderItem = _db.ReleaseReturnItems.Find(id);
            if (orderItem != null)
            {
                string username = Session["user"] as string;
                LogActivity(username, "Releasing Return", "Delete Item");
                _db.ReleaseReturnItems.Remove(orderItem);
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
                    var orderItem = _db.ReleaseReturnItems.Find(itemId);
                    if (orderItem != null)
                    {
                        _db.ReleaseReturnItems.Remove(orderItem);
                    }
                }

                string username = Session["user"] as string;
                LogActivity(username, "Releasing Return", "Cancel");
                Session.Remove("orderItemIds");
                _db.SaveChanges();
                TempData["alertbox"] = "Releasing return canceled." + Session["orderItemIds"];
            }
            else
                TempData["alertbox"] = "There are no adjustment item to cancel." + Session["orderItemIds"];

            return RedirectToAction("Manage");
        }

        [HttpPost]
        public ActionResult EditDetail(ReleaseReturn updatedOrder)
        {
            if (ModelState.IsValid)
            {
                if (Session["orderItemIds"] != null)
                {
                    // If orderItemIds exist in the session, log the activity
                    string username = Session["user"] as string;
                    LogActivity(username, "Releasing Return", "Edit Detail");
                    _db.Entry(updatedOrder).State = EntityState.Modified;
                    _db.SaveChanges();
                }
                else
                {
                    // If orderItemIds do not exist in the session, update the order status
                    string username = Session["user"] as string;
                    LogActivity(username, "Releasing Return", "Edit Detail");
                    _db.Entry(updatedOrder).State = EntityState.Modified;
                    _db.SaveChanges();
                }
                Session.Remove("orderItemIds");
                TempData["alertbox"] = "Releasing return changes have been successfully saved.";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Manage", new { Id = updatedOrder.Id });
        }

        public ActionResult ManageApproval()
        {
            if (User.IsInRole("admin") || User.IsInRole("salesmgr"))
            {
                // Retrieve receiving data
                var returns = _db.ReleaseReturns
                    .Include(x => x.Releasing)
                    .Where(x => x.Status == "Pending")
                    .Include(x => x.ReleaseReturnItems)
                    .OrderByDescending(x => x.Id)
                    .ToList();

                ViewBag.ApproveDate = DateTime.Now; //for date
                ViewBag.RejectDate = DateTime.Now; //for date

                return View(returns);
            }
            else if (User.IsInRole("purchmgr") || User.IsInRole("wrhmgr"))
                ViewBag.Message = "releaser manager can access return approval";

            return View();
        }

        public ActionResult Detail(int Id) //for invoice detail
        {
            if (User.IsInRole("admin") || User.IsInRole("salesmgr"))
            {
                var returns = _db.ReleaseReturns
                    .Include(x => x.Releasing)
                    .Where(x => x.Id == Id)
                    .SingleOrDefault();

                if (returns == null)
                {
                    TempData["alertbox"] = "Releasing Not Exist";
                    return RedirectToAction("Manage");
                }

                return View(returns);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("purchmgr"))
                ViewBag.Message = "releaser can access invoice detail of releasing";

            return View();
        }

        public ActionResult Print(int Id) //for pdf
        {
            // Find the order with the specified Id along with its associated OrderItemWalkIns
            var order = _db.ReleaseReturns
                .Where(x => x.Id == Id)
                .Include(x => x.ReleaseReturnItems)
                .Include(x => x.Releasing)
                .SingleOrDefault();

            var prod = _db.Products.ToList();

            if (order == null)
            {
                TempData["alertbox"] = "Order not found";
                return RedirectToAction("Manage");
            }
            else if (order.ReleaseReturnItems == null || order.ReleaseReturnItems.Count == 0)
            {
                TempData["alertbox"] = "No items in the order";
                return RedirectToAction("Manage");
            }

            return new Rotativa.ViewAsPdf(order);
        }

        [HttpPost]
        public ActionResult Approve(int Id)
        {
            if (ModelState.IsValid)
            {
                var order = _db.ReleaseReturns
                    .Where(x => x.Id == Id)
                    .Include(x => x.ReleaseReturnItems)
                    .Include(x => x.Releasing)
                    .FirstOrDefault();

                if (order != null)
                {
                    foreach (var orderItem in order.ReleaseReturnItems)
                    {
                        ReleaseReturnItemApprove approves = new ReleaseReturnItemApprove
                        {
                            ReturnId = orderItem.ReturnId,
                            ProductId = orderItem.ProductId,
                            Quantity = orderItem.Quantity,
                            SellingPrice = orderItem.SellingPrice
                        };
                        _db.ReleaseReturnItemApproves.Add(approves);
                    }

                    string username = Session["user"] as string;
                    LogActivity(username, "Release Return", "Approval Approved");
                    order.Status = "Approved"; // Update to the appropriate new status
                    _db.Entry(order).State = EntityState.Modified;
                    _db.SaveChanges();
                    TempData["alertbox"] = "Release Return will be approved";
                }
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
                var order = _db.ReleaseReturns.Where(x => x.Id == Id).FirstOrDefault();
                string username = Session["user"] as string;
                LogActivity(username, "Release Return", "Approval Reject");
                order.Status = "Reject"; // Update to the appropriate new status
                _db.Entry(order).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["alertbox"] = "Release Return will be Reject";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Manage");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Reject(int Id, DateTime RejectDate)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        WarehouseRestockPending requests = _db.WarehouseRestockPendings.Find(Id);
        //        if (requests != null)
        //        {
        //            WarehouseRestockReject reject = new WarehouseRestockReject
        //            {
        //                WarehouseId = requests.WarehouseId,
        //                Quantity = requests.Quantity,
        //                RejectDate = RejectDate,
        //                Notes = requests.Notes
        //            };

        //            _db.WarehouseRestockRejects.Add(reject);
        //            _db.WarehouseRestockPendings.Remove(requests);
        //            string username = Session["user"] as string;
        //            LogActivity(username, "Restock", "Product Request Rejected Warehouse");
        //            TempData["alertbox"] = "Product Request will be Rejected";
        //            _db.SaveChanges();
        //        }
        //        else
        //            TempData["alertcard"] = "There are some validation errors. Please check and try again.";
        //    }
        //    else
        //        TempData["alertcard"] = "There are some validation errors. Please check and try again.";

        //    return RedirectToAction("Manage");
        //}

        //public ActionResult Detail(int Id) //for invoice detail
        //{
        //    if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
        //    {
        //        var returns = _db.PurchaseReturns
        //            .Include(x => x.Supplier)
        //            .Where(x => x.Id == Id)
        //            .SingleOrDefault();

        //        return View(returns);
        //    }
        //    else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
        //        ViewBag.Message = "releaser can access invoice detail of releasing";

        //    return View();
        //}

        //public ActionResult Manage()
        //{
        //    if(User.IsInRole("salesmgr") || User.IsInRole("admin"))
        //    {
        //        var returns = _db.ReleaseReturns
        //            .Include(x => x.Releasing)
        //            .OrderByDescending(x => x.Id)
        //            .ToList();

        //        return View(returns);
        //    }
        //    else if (User.IsInRole("wrhmgr") || User.IsInRole("purchmgr"))
        //        ViewBag.Message = "releaser manager can access releasing return";

        //    return View();
        //}

        //public ActionResult Create()
        //{
        //    if (User.IsInRole("salesmgr") || User.IsInRole("admin"))
        //    {
        //        var deliveredOrderIds = _db.Releasings
        //            .Where(p => p.ReleaseItems.Any(pi => pi.Quantity >= 5))
        //            .ToList();

        //        ViewBag.Order = deliveredOrderIds;
        //        ViewBag.ReturnDate = DateTime.Now;

        //        return View(deliveredOrderIds);
        //    }
        //    else if (User.IsInRole("wrhmgr") || User.IsInRole("purchmgr"))
        //        ViewBag.Message = "releaser manager can access releasing return";

        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(ReleaseReturn returnDamage, ReleaseReturnDmg returnDamages)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (_db.ReleaseReturns.Any(x => x.ProductId == returnDamage.ProductId && x.ReturnDate == returnDamage.ReturnDate && x.ReleaseId == returnDamage.ReleaseId))
        //            TempData["alertbox"] = "A release return for the selected product on the customer and date already exists.";
        //        else
        //        {
        //            // Retrieve the corresponding order items for the product ID
        //            var orderItems = _db.ReleaseItems.Where(x => x.ProductId == returnDamage.ProductId).ToList();
        //            int totalQuantity = orderItems.Sum(x => x.Quantity);

        //            // Check if the quantity being returned is valid
        //            if (returnDamage.Quantity <= totalQuantity)
        //            {
        //                //if (returnDamage.Purpose.Contains("Overstock"))
        //                //{
        //                //    _db.ReleaseReturns.Add(returnDamage);
        //                //    string username = Session["user"] as string;
        //                //    LogActivity(username, "Release Return", "Create");
        //                //    TempData["alertbox"] = "Release Return has been successfully added.";
        //                //    _db.SaveChanges();
        //                //}
        //                if (returnDamage.Purpose.Contains("Damage"))
        //                {
        //                    if (orderItems.Any()) // Check if the list is not empty
        //                    {
        //                        _db.ReleaseReturns.Add(returnDamage);
        //                        _db.SaveChanges(); // Save changes to get the generated Id

        //                        returnDamages.ReturnId = returnDamage.Id; // Set the correct ReturnId
        //                        _db.ReleaseReturnDmgs.Add(returnDamages);
        //                        string username = Session["user"] as string;
        //                        LogActivity(username, "Releasing Return", "Damage");
        //                        TransactionActivity("Releasing Return", returnDamage.Quantity);
        //                        TempData["alertbox"] = "Release Damage has been successfully added.";
        //                        _db.SaveChanges();  // Save changes for returnDamages
        //                    }
        //                    else
        //                        TempData["alertbox"] = "No corresponding order items found for the product ID.";
        //                }
        //            }
        //            else
        //                TempData["alertbox"] = "Return quantity exceeds the total quantity in orders.";
        //        }
        //    }
        //    else
        //        TempData["alertcard"] = "There are some validation errors. Please check and try again.";

        //    return RedirectToAction("Manage");
        //}

        //public ActionResult LoadItems(int orId)
        //{
        //    var items = _db.ReleaseItems
        //        .Where(p => p.ReleaseId == orId)
        //        .Select(p => new
        //        {
        //            ProductId = p.ProductId,
        //            Product = p.Product.Name
        //        }).ToList();

        //    return Json(items, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult ManageDamage()
        //{
        //    if (User.IsInRole("salesmgr") || User.IsInRole("admin"))
        //    {
        //        var returns = _db.ReleaseReturnDmgs
        //            .Include(x => x.Product)
        //            .Include(x => x.Releasing)
        //            .Include(x => x.ReleaseReturn)
        //            .ToList();

        //        return View(returns);
        //    }
        //    else if (User.IsInRole("wrhmgr") || User.IsInRole("purchmgr"))
        //        ViewBag.Message = "releaser manager can access releasing return";

        //    return View();
        //}

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
using ASPNETWebApp48.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ASPNETWebApp48.Controllers
{
    [Authorize]
    public class ReceiveController : Controller
    {
        InventoryDbContext _db = new InventoryDbContext();
        public ActionResult Manage()
        {
            if (User.IsInRole("admin") || User.IsInRole("wrhmgr"))
            {
                // Retrieve purchase IDs that are already in receiving
                var purchaseIdsInReceiving = _db.Receivings.Select(r => r.PurchaseId).ToList();

                var availablePurchases = _db.Purchases
                    .Include(x => x.Supplier)
                    .Include(x => x.PurchaseItems) // Include the purchase items if needed
                    .Where(p => p.PurchaseItems.Sum(pi => pi.Quantity) >= 5 && !purchaseIdsInReceiving.Contains(p.Id))
                    .ToList();

                // Retrieve receiving data
                var receiving = _db.Receivings
                    .Include(x => x.Locations)
                    .Include(x => x.Supplier)
                    .Include(x => x.ReceivingItems)
                    .OrderByDescending(x => x.Id)
                    .ToList();

                // Set ViewBag properties
                ViewBag.ReceiveDate = DateTime.Now;
                ViewBag.ForCreatePartial = new Dictionary<string, object>
                {
                    { "SupplierLookup", availablePurchases },
                    { "LocLookup", _db.Locations
                        .Where(location => location.Id == 2)
                        .ToList()}
                };

                // Return the view with receiving data
                return View(receiving);
            }
            else if (User.IsInRole("purchmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "Warehouse & admin manager can access receiving";

            return View();
        }

        public ActionResult GetPurchase(int Id)
        {
            var purchaseno = _db.Purchases.Find(Id).PurchaseNo;
            return Json(purchaseno, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(Receiving receiving)
        {
            if (ModelState.IsValid)
            {
                var purchase = _db.Purchases
                    .Include(p => p.Supplier)
                    .Include(x => x.PurchaseItems)
                    .FirstOrDefault(p => p.Id == receiving.PurchaseId);

                if (_db.Receivings.Any(x => x.PurchaseId == receiving.PurchaseId))
                {
                    TempData["alertbox"] = "Receiving already exists.";
                    return RedirectToAction("Manage");
                }
                if (purchase.PurchaseItems == null || purchase.PurchaseItems.Count == 0)
                {
                    TempData["alertbox"] = "No items in the order";
                    return RedirectToAction("Manage");
                }
                else
                {
                    var groupedPurchaseItems = purchase.PurchaseItems.GroupBy(pi => pi.ProductId);

                    receiving.SupplierId = purchase.SupplierId;
                    _db.Receivings.Add(receiving);

                    foreach (var group in groupedPurchaseItems)
                    {
                        var firstPurchaseItem = group.First();

                        var receivingItem = new ReceivingItem
                        {
                            ProductSupId = firstPurchaseItem.ProductSupId,
                            ProductId = firstPurchaseItem.ProductId,
                            Quantity = group.Sum(pi => pi.Quantity),
                            PurchasePrice = firstPurchaseItem.PurchasePrice
                        };
                        _db.ReceivingItems.Add(receivingItem);
                    }

                    string username = Session["user"] as string;
                    LogActivity(username, "Receive", "Create Receiving");
                    _db.SaveChanges();
                    return RedirectToAction("Get", new { Id = receiving.Id });
                }
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return View();
        }

        public ActionResult Get(int Id) // Change int to int?
        {
            if (User.IsInRole("admin") || User.IsInRole("wrhmgr"))
            {
                var receiving = _db.Receivings
                    .Where(x => x.Id == Id)
                    .Include(x => x.Locations)
                    .Include(x => x.Supplier)
                    .Include(x => x.ReceivingItems)
                    .OrderByDescending(x => x.Id)
                    .SingleOrDefault();

                var sup = _db.ProductSuppliers.Include(x => x.Product).ToList();
                var brand = _db.Brands.ToList();

                ViewBag.ReceiveDate = DateTime.Now; //for date

                if (receiving == null)
                {
                    TempData["alertbox"] = "Receiving Not Exist";
                    return RedirectToAction("Manage");
                }
                else if (receiving.Status.Contains("Received"))
                {
                    TempData["alertbox"] = "Received | Cannot Visit this page";
                    return RedirectToAction("Manage");
                }
                else if (receiving.Status.Contains("Complete"))
                {
                    TempData["alertbox"] = "Complete | Cannot Visit this page";
                    return RedirectToAction("Manage");
                }
                else if (receiving.Status.Contains("Closed"))
                {
                    TempData["alertbox"] = "Closed | Cannot Visit this page";
                    return RedirectToAction("Manage");
                }

                return View(receiving);
            }
            else if (User.IsInRole("purchmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "Warehouse & admin manager can access receiving";

            return View();
        }

        public ActionResult Edit(int id) //to edit quantity
        {
            if (User.IsInRole("admin") || User.IsInRole("wrhmgr"))
            {
                var purItem = _db.ReceivingItems.Find(id);

                var sup = _db.ProductSuppliers.Include(x => x.Product).ToList();

                return View(purItem);
            }
            else if (User.IsInRole("purchmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "Warehouse & admin manager can access receiving";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReceivingItem receivingItem)
        {
            if (ModelState.IsValid)
            {
                var product = _db.Products.Find(receivingItem.ProductId);
                product.PurchasePrice = receivingItem.PurchasePrice;
                _db.Entry(product).State = EntityState.Modified;
                _db.Entry(receivingItem).State = EntityState.Modified;
                string username = Session["user"] as string;
                LogActivity(username, "Receiving", "Update Product");
                TempData["alertbox"] = "Product changes have been successfully saved.";
                _db.SaveChanges();
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Get", new { Id = receivingItem.ReceiveId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Get(Receiving receivingItem)
        {
            receivingItem.Status = "Received";
            string username = Session["user"] as string;
            LogActivity(username, "Receiving", "Update Detail");
            _db.Entry(receivingItem).State = EntityState.Modified;
            _db.SaveChanges();
            TempData["alertbox"] = "receive detail changes have been successfully saved.";
            return RedirectToAction("Manage");
        }

        public ActionResult DelItem(int id)
        {
            var purItem = _db.ReceivingItems.Find(id);
            if (purItem != null)
            {
                _db.ReceivingItems.Remove(purItem);
                string username = Session["user"] as string;
                LogActivity(username, "Receiving", "Delete Item");
                _db.SaveChanges();
            }

            return RedirectToAction("Get", new { Id = purItem.ReceiveId });
        }

        public ActionResult ManageApproval()
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                // Retrieve receiving data
                var receiving = _db.Receivings
                    .OrderByDescending(x => x.Id)
                    .Include(x => x.Locations)
                    .Include(x => x.Supplier)
                    .Include(x => x.ReceivingItems)
                    .Where(r => r.Status == "Received" || r.Status == "Complete")
                    .ToList();

                return View(receiving);
            }
            else if (User.IsInRole("salesmgr") || User.IsInRole("wrhmgr"))
                ViewBag.Message = "purchase manager can access receiving approval";

            return View();
        }

        public ActionResult ViewDetail(int Id)
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                var receiving = _db.Receivings
                    .Where(x => x.Id == Id)
                    .Include(x => x.Locations)
                    .Include(x => x.Supplier)
                    .Include(x => x.ReceivingItems)
                    .OrderByDescending(x => x.Id)
                    .SingleOrDefault();

                var sup = _db.ProductSuppliers.Include(x => x.Product).ToList();
                var brand = _db.Brands.ToList();

                if (receiving == null)
                {
                    TempData["alertbox"] = "Receiving Not Exist";
                    return RedirectToAction("ManageApproval");
                }
                else if (receiving.Status.Contains("Complete"))
                {
                    TempData["alertbox"] = "Complete | Cannot Visit this page";
                    return RedirectToAction("ManageApproval");
                }

                return View(receiving);
            }
            else if (User.IsInRole("salesmgr") || User.IsInRole("wrhmgr"))
                ViewBag.Message = "purchase manager can access receiving approval";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int Id)
        {
            if (ModelState.IsValid)
            {
                var receive = _db.Receivings
                    .Where(x => x.Id == Id)
                    .Include(x => x.ReceivingItems)
                    .FirstOrDefault();

                //if (receive.LocationId == 1)
                //{
                //    if (receive != null)
                //    {
                //        foreach (var receiveItem in receive.ReceivingItems)
                //        {
                //            ReceiveApproveItem approves = new ReceiveApproveItem
                //            {
                //                ReceiveId = receive.Id,
                //                ProductId = receiveItem.ProductId,
                //                WarehouseId = null,
                //                Quantity = receiveItem.Quantity,
                //                PurchasePrice = receiveItem.PurchasePrice
                //            };
                //            _db.ReceiveApproveItems.Add(approves);
                //        }

                //        receive.Status = "Complete";
                //        _db.Entry(receive).State = EntityState.Modified;
                //        _db.SaveChanges();
                //        TempData["alertbox"] = "Receive Approval will be sent to main store";
                //    }
                //}
                if (receive.LocationId == 2)
                {
                    if (receive != null)
                    {
                        foreach (var receiveItem in receive.ReceivingItems)
                        {
                            var product = _db.Products.Find(receiveItem.ProductId);

                            if (product != null)
                            {
                                var existingWarehouse = _db.ProductWarehouses.FirstOrDefault(pw => pw.Name == product.Name);

                                if (existingWarehouse != null)
                                {
                                    // Use existing warehouse
                                    var receivingItem = new ReceiveApproveItem
                                    {
                                        ReceiveId = receive.Id,
                                        ProductId = null, // Adjust this if necessary
                                        WarehouseId = existingWarehouse.Id,
                                        Quantity = receiveItem.Quantity,
                                        PurchasePrice = receiveItem.PurchasePrice
                                    };

                                    receive.Status = "Complete";
                                    _db.Entry(receive).State = EntityState.Modified;
                                    string username = Session["user"] as string;
                                    LogActivity(username, "Receiving", "Approval Complete");
                                    TransactionActivity("Receiving", receivingItem.Quantity);
                                    _db.ReceiveApproveItems.Add(receivingItem);
                                    _db.SaveChanges();
                                    TempData["alertbox"] = "Receive Approval will be sent to warehouse";
                                }
                                else
                                {
                                    // Create new warehouse
                                    var productWarehouse = new ProductWarehouse
                                    {
                                        Name = product.Name,
                                        BrandId = product.BrandId,
                                        CategoryId = product.CategoryId,
                                        PurchasePrice = product.PurchasePrice,
                                        Unit = product.Unit,
                                        MinQty = product.MinQty,
                                        CeilingQty = product.CeilingQty
                                    };

                                    _db.ProductWarehouses.Add(productWarehouse);
                                    _db.SaveChanges();

                                    // Use newly created warehouse
                                    var receivingItem = new ReceiveApproveItem
                                    {
                                        ReceiveId = receive.Id,
                                        ProductId = null, // Adjust this if necessary
                                        WarehouseId = productWarehouse.Id,
                                        Quantity = receiveItem.Quantity,
                                        PurchasePrice = receiveItem.PurchasePrice
                                    };

                                    receive.Status = "Complete";
                                    _db.Entry(receive).State = EntityState.Modified;
                                    string username = Session["user"] as string;
                                    LogActivity(username, "Receiving", "Approval Complete");
                                    TransactionActivity("Receiving", receivingItem.Quantity);
                                    _db.ReceiveApproveItems.Add(receivingItem);
                                    _db.SaveChanges();
                                    TempData["alertbox"] = "Receive Approval will be sent to warehouse";
                                }
                            }
                            else
                            {
                                TempData["alertbox"] = "Product with ID " + receiveItem.ProductId + " does not exist.";
                            }
                        }
                    }
                }
                else
                    TempData["alertcard"] = "Order not found.";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("ManageApproval");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Closed(int Id)
        //{
        //    Receiving receiving = _db.Receivings.Find(Id);
        //    receiving.Status = "Closed";
        //    string username = Session["user"] as string;
        //    LogActivity(username, "Receive", "Receive Approval Closed");
        //    _db.Entry(receiving).State = EntityState.Modified;
        //    _db.SaveChanges();
        //    TempData["alertbox"] = "Receive Approval will be Closed";
        //    return RedirectToAction("ManageApproval");
        //}

        //public ActionResult Explain(int Id)
        //{
        //    if (User.IsInRole("admin") || User.IsInRole("wrhmgr"))
        //    {
        //        var receiving = _db.Receivings
        //            .Where(x => x.Id == Id)
        //            .Include(x => x.Locations)
        //            .Include(x => x.Supplier)
        //            .Include(x => x.ReceivingItems)
        //            .SingleOrDefault();

        //        return View(receiving);
        //    }
        //    else if (User.IsInRole("purchmgr") || User.IsInRole("salesmgr"))
        //        ViewBag.Message = "Warehouse manager can access stock receiving";

        //    return View();
        //}

        //public ActionResult ReceivePrint(int Id) //for pdf
        //{
        //    var receiving = _db.Receivings
        //            .Where(x => x.Id == Id)
        //            .Include(x => x.Locations)
        //            .Include(x => x.Supplier)
        //            .Include(x => x.ReceivingItems)
        //            .OrderByDescending(x => x.Id)
        //            .SingleOrDefault();

        //    var sup = _db.ProductSuppliers.Include(x => x.Product).ToList();
        //    var brand = _db.Brands.ToList();

        //    return new Rotativa.ViewAsPdf(receiving);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Pending(int Id, string Reason)
        //{
        //    var receiving = _db.Receivings
        //            .Where(x => x.Id == Id)
        //            .Include(x => x.Locations)
        //            .Include(x => x.Supplier)
        //            .Include(x => x.ReceivingItems)
        //            .OrderByDescending(x => x.Id)
        //            .SingleOrDefault();

        //    var sup = _db.ProductSuppliers.Include(x => x.Product).ToList();
        //    var brand = _db.Brands.ToList();

        //    //Receiving receiving = _db.Receivings.Find(Id);
        //    receiving.Reason = Reason;
        //    receiving.Status = "Pending";
        //    string username = Session["user"] as string;
        //    LogActivity(username, "Receive", "Receive Approval Pending");
        //    _db.Entry(receiving).State = EntityState.Modified;
        //    _db.SaveChanges();

        //    var pdfViewResult = new Rotativa.ViewAsPdf("ReceivePrint", receiving)
        //    {
        //        FileName = "Receive.pdf"
        //    };

        //    var pdfAttachment = new EmailService.EmailAttachment()
        //    {
        //        Data = pdfViewResult.BuildPdf(this.ControllerContext),
        //        ContentType = "application/pdf",
        //        FileName = pdfViewResult.FileName
        //    };

        //    bool result = EmailService.SendEmail("joseduke325@gmail.com", "- Receive PDF Email", "Hello! This is Attachment for receive pdf file.", pdfAttachment);
        //    if (result)
        //        TempData["alertbox"] = "the pdf file has been sent";
        //    else
        //        TempData["alertbox"] = "Sending failed.";

        //    return RedirectToAction("ManageApproval");
        //}

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
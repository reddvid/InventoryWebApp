using ASPNETWebApp48.Models;
using ASPNETWebApp48.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ASPNETWebApp48.Controllers
{
    [Authorize]
    public class ReleaseController : Controller
    {
        InventoryDbContext _db = new InventoryDbContext();
        public ActionResult Manage()
        {
            if (User.IsInRole("admin") || User.IsInRole("salesmgr"))
            {
                var order = _db.Releasings
                    .Include(x => x.ReleaseItems)
                    .OrderByDescending(x => x.Id)
                    .ToList();

                return View(order);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("purchmgr"))
                ViewBag.Message = "releaser manager can access releasing";

            return View();
        }

        public ActionResult Create()
        {
            if (User.IsInRole("admin") || User.IsInRole("salesmgr"))
            {
                ViewBag.OrderDate = DateTime.Now;
                ViewBag.PurchaseDate = DateTime.Now;

                var product = _db.Products
                    .Include(x => x.ReleaseItems)
                    .Include(x => x.TransferItemStores)
                    .Include(x => x.TransferItemWarehouses)
                    .Include(x => x.AdjustItems)
                    .ToList();

                // Filter the product with stock on hand
                var productWithStock = product
                    .Where(x => x.StockOnHand > 10)
                    .ToList();

                ViewBag.ForCreatePartial = new Dictionary<string, object>
                {
                    { "ProductLookup", productWithStock }
                };

                return View();
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("purchmgr"))
                ViewBag.Message = "releaser manager can access releasing";

            return View();
        }

        public ActionResult GetProductPrice(int Id) // Product.Id
        {
            var unitPrice = _db.Products.Find(Id).SellingPrice;
            return Json(unitPrice, JsonRequestBehavior.AllowGet);
        }

        public class SessionOrderItem
        {
            public int OrderId { get; set; }
            public ReleaseItem ReleaseItems { get; set; }
            public string ProductName { get; set; }
            public string ProductSKU { get; set; }
        }

        [HttpPost]
        public ActionResult CreateOrderItem(ReleaseItem orderItem)
        {
            if (ModelState.IsValid)
            {
                var product = _db.Products.FirstOrDefault(p => p.Id == orderItem.ProductId);

                if (product != null)
                {
                    orderItem.ReleaseId = 0;

                    int createdItemId = orderItem.Id;
                    SessionOrderItem sessionOrderItem = new SessionOrderItem
                    {
                        OrderId = orderItem.ReleaseId,
                        ReleaseItems = orderItem
                    };

                    List<SessionOrderItem> sessionOrderItems = Session["sessionOrderItems"] as List<SessionOrderItem> ?? new List<SessionOrderItem>();
                    if (product != null)
                    {
                        sessionOrderItem.ProductName = product.Name;
                        sessionOrderItem.ProductSKU = product.Unit;
                    }
                    sessionOrderItems.Add(sessionOrderItem);

                    Session["sessionOrderItems"] = sessionOrderItems;

                    string orderItemsString = string.Join("<br/>", sessionOrderItems.Select(item =>
                        $"Order ID: {item.OrderId}, Product Name: {item.ProductName}, Product SKU: {item.ProductSKU}, Quantity: {item.ReleaseItems.Quantity}, Selling Price: {item.ReleaseItems.SellingPrice}"));

                    return RedirectToAction("Create");
                }
                else
                {
                    TempData["alertcard"] = "Product with the provided ID does not exist.";
                    return RedirectToAction("Create");
                }
            }
            else
            {
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";
                return RedirectToAction("Create");
            }
        }

        public ActionResult delSession(int productId)
        {
            List<SessionOrderItem> sessionOrderItems = Session["sessionOrderItems"] as List<SessionOrderItem>;
            if (sessionOrderItems != null)
            {
                int indexToRemove = sessionOrderItems.FindIndex(item => item.ReleaseItems.ProductId == productId);
                if (indexToRemove != -1)
                {
                    sessionOrderItems.RemoveAt(indexToRemove);
                    Session["sessionOrderItems"] = sessionOrderItems;
                }
            }

            return RedirectToAction("Create");
        }

        [HttpPost]
        public ActionResult CreateOrderAndItem(Releasing order)
        {
            if (ModelState.IsValid)
            {
                string username = Session["user"] as string;
                LogActivity(username, "Releasing", "Create Order");
                _db.Releasings.Add(order);
                _db.SaveChanges();
                int orderId = order.Id;

                List<SessionOrderItem> sessionOrderItems = Session["sessionOrderItems"] as List<SessionOrderItem>;
                if (sessionOrderItems != null)
                {
                    foreach (var sessionOrderItem in sessionOrderItems)
                    {
                        ReleaseItem orderItem = new ReleaseItem
                        {
                            ProductId = sessionOrderItem.ReleaseItems.ProductId,
                            Quantity = sessionOrderItem.ReleaseItems.Quantity,
                            SellingPrice = sessionOrderItem.ReleaseItems.SellingPrice,
                            ReleaseId = orderId
                        };

                        _db.ReleaseItems.Add(orderItem);
                        TransactionActivity("Releasing", orderItem.Quantity);
                    }

                    _db.SaveChanges();
                    TempData["alertbox"] = "Releasing order and items created successfully.";
                    Session.Remove("sessionOrderItems");
                }
                else
                    TempData["alertbox"] = "No items in the sessionOrderItems.";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("InvoiceDetail", new { Id = order.Id });
        }

        [HttpPost]
        public ActionResult CreateItem(ReleaseItem releaseItem)
        {
            if (ModelState.IsValid)
            {
                // Get the product associated with the order item
                var product = _db.Products
                    .Include(x => x.ReleaseItems)
                    .Include(x => x.TransferItemStores)
                    .Include(x => x.TransferItemWarehouses)
                    .Include(x => x.AdjustItems)
                    .FirstOrDefault(x => x.Id == releaseItem.ProductId);

                if (product != null)
                {
                    // Check if there is enough stock
                    if (product.StockOnHand >= releaseItem.Quantity)
                    {
                        string username = Session["user"] as string;
                        LogActivity(username, "Releasing", "Add Item");
                        TransactionActivity("Releasing", releaseItem.Quantity);
                        _db.ReleaseItems.Add(releaseItem);
                        _db.SaveChanges();
                        // Retrieve the ID of the newly created order item
                        int createdItemId = releaseItem.Id;
                        // Retrieve the existing array from session, or create a new one if it doesn't exist
                        List<int> orderItemIds = Session["orderItemIds"] as List<int> ?? new List<int>();
                        // Add the newly created order item's ID to the array
                        orderItemIds.Add(createdItemId);
                        // Update the array in session
                        Session["orderItemIds"] = orderItemIds;
                        // Convert the list to a string for display in the alert message
                        string orderItemIdsString = string.Join(", ", orderItemIds);
                    }
                    else
                        TempData["alertbox"] = "There is insufficient stock for this product.";
                }
                else
                    TempData["alertbox"] = "Product not found.";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Get", new { Id = releaseItem.ReleaseId });
        }

        public ActionResult DelItem(int id)
        {
            var orderItem = _db.ReleaseItems.Find(id);
            if (orderItem != null)
            {
                string username = Session["user"] as string;
                LogActivity(username, "Releasing", "Delete Item");
                TransactionActivity("Releasing", orderItem.Quantity);
                _db.ReleaseItems.Remove(orderItem);
                _db.SaveChanges();
            }

            return RedirectToAction("Get", new { Id = orderItem.ReleaseId });
        }

        [HttpPost]
        public ActionResult Create(Releasing updatedOrder)
        {
            if (Session["orderItemIds"] != null)
            {
                // If orderItemIds exist in the session, log the activity
                string username = Session["user"] as string;
                LogActivity(username, "Releasing", "Update Detail");
                _db.Entry(updatedOrder).State = EntityState.Modified;
                _db.SaveChanges();
            }
            else
            {
                // If orderItemIds do not exist in the session, update the order status
                _db.Entry(updatedOrder).State = EntityState.Modified;
                _db.SaveChanges();
            }
            Session.Remove("orderItemIds");
            TempData["alertbox"] = "Releasing details changes have been successfully saved.";
            return RedirectToAction("InvoiceDetail", new { Id = updatedOrder.Id });
        }

        public ActionResult Cancel()
        {
            // Retrieve order item IDs from the session, if available
            List<int> orderItemIds = Session["orderItemIds"] as List<int>;

            if (orderItemIds != null && orderItemIds.Any())
            {
                foreach (int itemId in orderItemIds)
                {
                    var orderItem = _db.ReleaseItems.Find(itemId);
                    if (orderItem != null)
                    {
                        TransactionActivity("Releasing", orderItem.Quantity);
                        _db.ReleaseItems.Remove(orderItem);
                    }
                }

                Session.Remove("orderItemIds");
                _db.SaveChanges();
                TempData["alertbox"] = "Releasing canceled." + Session["orderItemIds"];
            }
            else
                TempData["alertbox"] = "There are no releasing items to cancel." + Session["orderItemIds"];

            return RedirectToAction("Manage");
        }

        public ActionResult InvoiceDetail(int Id) //for invoice detail
        {
            if (User.IsInRole("admin") || User.IsInRole("salesmgr"))
            {
                var order = _db.Releasings.Include(x => x.ReleaseItems).Where(x => x.Id == Id).SingleOrDefault();

                if (order == null)
                {
                    TempData["alertbox"] = "Releasing Not Exist";
                    return RedirectToAction("Manage");
                }

                return View(order);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("purchmgr"))
                ViewBag.Message = "releaser can access invoice detail of releasing";

            return View();
        }

        //public ActionResult Print(int Id) //for pdf
        //{
        //    // Find the order with the specified Id along with its associated OrderItemWalkIns
        //    var order = _db.Releasings
        //        .Where(x => x.Id == Id)
        //        .Include(x => x.ReleaseItems)
        //        .SingleOrDefault();

        //    var prod = _db.Products.ToList();

        //    if (order == null)
        //    {
        //        TempData["alertbox"] = "Order not found";
        //        return RedirectToAction("Manage");
        //    }
        //    else if (order.ReleaseItems == null || order.ReleaseItems.Count == 0)
        //    {
        //        TempData["alertbox"] = "No items in the order";
        //        return RedirectToAction("Manage");
        //    }

        //    return new Rotativa.ViewAsPdf(order);
        //}

        public ActionResult Get(int Id) // Order.Id
        {
            if (User.IsInRole("admin") || User.IsInRole("salesmgr"))
            {
                var order = _db.Releasings.Where(x => x.Id == Id).Include(x => x.ReleaseItems).SingleOrDefault();

                if (order == null)
                {
                    TempData["alertbox"] = "Order Not Exist";
                    return RedirectToAction("Manage");
                }

                ViewBag.PurchaseDate = DateTime.Now;

                var product = _db.Products
                    .Include(x => x.ReleaseItems)
                    .Include(x => x.TransferItemStores)
                    .Include(x => x.AdjustItems)
                    .Include(x => x.PurchaseReturnItems)
                    .Include(x => x.TransferItemWarehouses)
                    .ToList();

                // Filter the product with stock on hand
                var productWithStock = product
                    .Where(x => x.StockOnHand > 10)
                    .ToList();

                ViewBag.ForCreatePartial = new Dictionary<string, object>
                {
                    { "ProductLookup", productWithStock },
                    { "ReleaseId", order.Id }
                };

                return View(order);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("purchmgr"))
                ViewBag.Message = "releaser can access releasing";

            return View();
        }

        public ActionResult Report()
        {
            var release = _db.Releasings.Include(x => x.ReleaseItems).ToList();

            var viewModel = new ReleaseReportViewModel
            {
                Releasing = release
            };

            return new Rotativa.ViewAsPdf(viewModel);
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
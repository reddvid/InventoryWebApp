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
    public class RestockController : Controller
    {
        InventoryDbContext _db = new InventoryDbContext();
        public ActionResult RequestProd(int id)
        {
            if (User.IsInRole("admin") || User.IsInRole("salesmgr"))
            {
                Product product = _db.Products.Find(id);

                if (product == null)
                {
                    TempData["alertbox"] = "Product does not exist.";
                    return RedirectToAction("Manage");
                }

                ViewBag.RequestDate = DateTime.Now; //for date

                return View(product);
            }
            else if (User.IsInRole("purchmgr") || User.IsInRole("wrhmgr"))
                ViewBag.Message = "sales manager can access product request";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestProd(RestockPending request)
        {
            if (ModelState.IsValid)
            {
                // Check if the product exists
                var product = _db.Products.Find(request.Id);
                if (product == null)
                    TempData["alertbox"] = "Product with the specified ID does not exist.";
                if (_db.RestockPendings.Any(x => x.ProductId == request.ProductId && x.RequestDate == request.RequestDate))
                    TempData["alertbox"] = "A restock request for the selected product on the request date already exists.";
                else
                {
                    var existingWarehouse = _db.ProductWarehouses
                        .Include(x => x.ReceivingApproveItems)
                        .Include(x => x.TransferItemStores)
                        .Include(x => x.WarehouseRestockApprove)
                        .Include(x => x.TransferItemWarehouses)
                        .Include(x => x.RestockApproves)
                        .Include(x => x.PurchaseReturnItems)
                        .FirstOrDefault(pw => pw.Name == product.Name);
                    if (existingWarehouse != null)
                    {
                        if (existingWarehouse.StockOnHand >= request.Quantity && request.Quantity > 0)
                        {
                            // Add restock request to pending
                            request.ProductId = product.Id;
                            _db.RestockPendings.Add(request);
                            string username = Session["user"] as string;
                            LogActivity(username, "Restock", "Product Request Main");
                            _db.SaveChanges();
                            TempData["alertbox"] = "Product Request will be submitted to pending.";
                        }
                        else if (request.Quantity <= 0)
                            TempData["alertbox"] = "Please enter a positive quantity.";
                        else
                            TempData["alertbox"] = "There is insufficient stock for this product.";
                    }
                    else
                        TempData["alertbox"] = "Product not available in warehouse.";
                }
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Homepage", "Home");
        }

        public ActionResult RequestWarehouseProd(int id)
        {
            if (User.IsInRole("admin") || User.IsInRole("wrhmgr"))
            {
                ProductWarehouse product = _db.ProductWarehouses.Find(id);

                if (product == null)
                {
                    TempData["alertbox"] = "Product does not exist.";
                    return RedirectToAction("Manage");
                }

                ViewBag.RequestDate = DateTime.Now; //for date

                return View(product);
            }
            else if (User.IsInRole("purchmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "sales manager can access product request";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestWarehouseProd(WarehouseRestockPending request)
        {
            if (ModelState.IsValid)
            {
                if (_db.WarehouseRestockPendings.Any(x => x.WarehouseId == request.WarehouseId && x.RequestDate == request.RequestDate))
                    TempData["alertbox"] = "A restock request for the selected product on the request date already exists.";
                else
                {
                    var product = _db.ProductWarehouses.Find(request.Id);
                    string username = Session["user"] as string;
                    LogActivity(username, "Restock", "Product Request Warehouse");
                    request.WarehouseId = product.Id;
                    _db.WarehouseRestockPendings.Add(request);
                    _db.SaveChanges();
                    TempData["alertbox"] = "Product Request will be submited to pending.";
                }
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Homepage", "Home");
        }

        public ActionResult Manage()
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                var pendingmain = _db.RestockPendings
                    .Include(x => x.Product)
                    .OrderByDescending(x => x.Id)
                    .ToList();

                var pendingwh = _db.WarehouseRestockPendings
                    .Include(x => x.ProductWarehouse)
                    .OrderByDescending(x => x.Id)
                    .ToList();

                ViewBag.ApproveDate = DateTime.Now; //for date
                ViewBag.RejectDate = DateTime.Now; //for date

                var viewModel = new StockViewModel
                {
                    WarehousePendingLookup = pendingwh,
                    PendingLookup = pendingmain
                };

                return View(viewModel);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "purchase manager can access product request";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int Id, DateTime ApproveDate)
        {
            if (ModelState.IsValid)
            {
                RestockPending requests = _db.RestockPendings.Find(Id);
                var product = _db.Products.Find(requests.ProductId);

                if (requests != null)
                {
                    var whproduct = _db.ProductWarehouses.FirstOrDefault(pw => pw.Name == product.Name);
                    if (whproduct != null)
                    {
                        RestockApprove approves = new RestockApprove
                        {
                            ProductId = requests.ProductId,
                            WarehouseId = whproduct.Id,
                            Quantity = requests.Quantity,
                            ApproveDate = ApproveDate,
                            Notes = requests.Notes
                        };

                        _db.RestockApproves.Add(approves);
                        _db.RestockPendings.Remove(requests);
                        string username = Session["user"] as string;
                        LogActivity(username, "Restock", "Product Request Approved");
                        TempData["alertbox"] = "Product Request will be Approved";
                        _db.SaveChanges();
                    }
                    else
                        TempData["alertcard"] = "Product Warehouse not found";
                }
                else
                    TempData["alertcard"] = "There are some validation errors. Please check and try again.";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reject(int Id, DateTime RejectDate)
        {
            if (ModelState.IsValid)
            {
                RestockPending requests = _db.RestockPendings.Find(Id);
                if (requests != null)
                {
                    RestockReject rejects = new RestockReject
                    {
                        ProductId = requests.ProductId,
                        Quantity = requests.Quantity,
                        RejectDate = RejectDate,
                        Notes = requests.Notes
                    };

                    _db.RestockRejects.Add(rejects);
                    _db.RestockPendings.Remove(requests);
                    string username = Session["user"] as string;
                    LogActivity(username, "Restock", "Product Request Reject");
                    TempData["alertbox"] = "Product Request will be Rejected";
                    _db.SaveChanges();
                }
                else
                    TempData["alertcard"] = "There are some validation errors. Please check and try again.";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveWarehouse(int Id, DateTime ApproveDate)
        {
            if (ModelState.IsValid)
            {
                WarehouseRestockPending requests = _db.WarehouseRestockPendings.Find(Id);
                if (requests != null)
                {
                    WarehouseRestockApprove approve = new WarehouseRestockApprove
                    {
                        WarehouseId = requests.WarehouseId,
                        Quantity = requests.Quantity,
                        ApproveDate = ApproveDate,
                        Notes = requests.Notes
                    };

                    _db.WarehouseRestockApproves.Add(approve);
                    _db.WarehouseRestockPendings.Remove(requests);
                    string username = Session["user"] as string;
                    LogActivity(username, "Restock", "Product Request Approved Warehouse");
                    TempData["alertbox"] = "Product Request will be Approved";
                    _db.SaveChanges();
                }
                else
                    TempData["alertcard"] = "There are some validation errors. Please check and try again.";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Manage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RejectWarehouse(int Id, DateTime RejectDate)
        {
            if (ModelState.IsValid)
            {
                WarehouseRestockPending requests = _db.WarehouseRestockPendings.Find(Id);
                if (requests != null)
                {
                    WarehouseRestockReject reject = new WarehouseRestockReject
                    {
                        WarehouseId = requests.WarehouseId,
                        Quantity = requests.Quantity,
                        RejectDate = RejectDate,
                        Notes = requests.Notes
                    };

                    _db.WarehouseRestockRejects.Add(reject);
                    _db.WarehouseRestockPendings.Remove(requests);
                    string username = Session["user"] as string;
                    LogActivity(username, "Restock", "Product Request Rejected Warehouse");
                    TempData["alertbox"] = "Product Request will be Rejected";
                    _db.SaveChanges();
                }
                else
                    TempData["alertcard"] = "There are some validation errors. Please check and try again.";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Manage");
        }

        public ActionResult ManageApproved()
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                var req = _db.RestockApproves
                    .Include(x => x.Product)
                    .OrderByDescending(x => x.Id)
                    .ToList();

                return View(req);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "purchase manager can access request approval";

            return View();
        }

        public ActionResult ManageReject()
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                var req = _db.RestockRejects
                    .Include(x => x.Product)
                    .OrderByDescending(x => x.Id)
                    .ToList();

                return View(req);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "purchase manager can access request reject";

            return View();
        }

        public ActionResult ManageWarehouseApproved()
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                var req = _db.WarehouseRestockApproves
                    .Include(x => x.ProductWarehouse)
                    .OrderByDescending(x => x.Id)
                    .ToList();

                return View(req);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "purchase manager can access request approval";

            return View();
        }

        public ActionResult ManageWarehouseReject()
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr"))
            {
                var req = _db.WarehouseRestockRejects
                    .Include(x => x.ProductWarehouse)
                    .OrderByDescending(x => x.Id)
                    .ToList();

                return View(req);
            }
            else if (User.IsInRole("wrhmgr") || User.IsInRole("salesmgr"))
                ViewBag.Message = "purchase manager can access request reject";

            return View();
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
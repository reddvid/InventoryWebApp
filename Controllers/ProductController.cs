using ASPNETWebApp48.Models;
using ASPNETWebApp48.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPNETWebApp48.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        InventoryDbContext _db = new InventoryDbContext();
        public ActionResult Manage()
        {
            if (User.IsInRole("admin") || User.IsInRole("salesmgr") || User.IsInRole("purchmgr"))
            {
                var products = _db.Products
                    .OrderBy(x => x.Name)
                    .ToList();

                ViewBag.ForCreatePartial = new Dictionary<string, object>
                {
                    { "CategoryLookup", _db.Categories.OrderBy(x => x.Name).ToList() },
                    { "SupplierLookup", _db.Suppliers.OrderBy(x => x.Name).ToList() },
                    { "BrandLookup", _db.Brands.OrderBy(x => x.Name).ToList() }
                };

                return View(products);
            }
            else if (User.IsInRole("wrhmgr"))
                ViewBag.Message = "purchase, releaser and admin manager can access product";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, List<int> SupplierId, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                if (_db.Products.Any(x => x.Name == product.Name))
                    TempData["alertbox"] = "Product already exists.";
                else
                {
                    if (fileUpload != null)
                        product.PictureFilename = fileUpload.SaveAsImageFile(product.Name);
                    _db.Products.Add(product);
                    _db.SaveChanges(); // Save product to get its ID

                    string username = Session["user"] as string;
                    LogActivity(username, "Product Main", "Create Product");
                    // Now, add each selected SupplierId with the corresponding ProductId
                    foreach (var supplierId in SupplierId)
                    {
                        _db.ProductSuppliers.Add(new ProductSupplier { ProductId = product.Id, SupplierId = supplierId });
                    }
                    _db.SaveChanges(); // Save changes to the database
                    TempData["alertbox"] = "Product added successfully!";
                }
            }
            
            return RedirectToAction("Manage");
        }

        public ActionResult Edit(int id)
        {
            if (User.IsInRole("admin") || User.IsInRole("purchmgr") || User.IsInRole("salesmgr"))
            {
                Product product = _db.Products.Find(id);

                if (product == null)
                {
                    TempData["alertbox"] = "Product does not exist.";
                    return RedirectToAction("Manage");
                }

                var viewModel = new ProductEditViewModel
                {
                    Product = product,
                    Categories = _db.Categories.ToList(),
                    Brands = _db.Brands.ToList(),
                    ProductSuppliers = _db.ProductSuppliers.Where(ps => ps.ProductId == id).ToList(), // Filter ProductSuppliers by ProductId
                    Suppliers = _db.Suppliers.ToList(), // Populate Suppliers list
                    SelectedSupplierIds = _db.ProductSuppliers.Where(ps => ps.ProductId == id).Select(ps => ps.SupplierId).ToList()
                };

                return View(viewModel);
            }
            else if (User.IsInRole("wrhmgr"))
                ViewBag.Message = "purchase, releaser and admin manager can access edit product";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product updatedProduct, List<int> SupplierId, HttpPostedFileBase fileUpload)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(updatedProduct).State = EntityState.Modified;

                if (fileUpload != null) // Update picture
                    updatedProduct.PictureFilename = fileUpload.SaveAsImageFile(updatedProduct.Name);
                else // Retain the current picture
                    _db.Entry(updatedProduct).Property(x => x.PictureFilename).IsModified = false;

                // Update existing ProductSuppliers based on the selected SupplierId list
                foreach (var supplierId in SupplierId)
                {
                    // Check if the ProductSupplier already exists
                    var existingProductSupplier = _db.ProductSuppliers.FirstOrDefault(ps => ps.ProductId == updatedProduct.Id && ps.SupplierId == supplierId);

                    // If it exists, no need to add a new one
                    if (existingProductSupplier == null)
                    {
                        // Add new ProductSupplier
                        _db.ProductSuppliers.Add(new ProductSupplier { ProductId = updatedProduct.Id, SupplierId = supplierId });
                    }
                }

                string username = Session["user"] as string;
                LogActivity(username, "Product Main", "Edit Product");
                _db.SaveChanges(); // Save changes to the database
                TempData["alertbox"] = "Product changes have been successfully saved.";
            }
            else
                TempData["alertcard"] = "There are some validation errors. Please check and try again.";

            return RedirectToAction("Manage");
        }

        public ActionResult Inventory()
        {
            if (User.IsInRole("admin") || User.IsInRole("wrhmgr") || User.IsInRole("purchmgr") || User.IsInRole("salesmgr"))
            {
                var items = _db.Products
                    .Include(x => x.ReleaseItems)
                    .Include(x => x.TransferItemStores)
                    .Include(x => x.TransferItemWarehouses)
                    .Include(x => x.AdjustItems)
                    .Include(x => x.PurchaseReturnItems)//returned
                    .OrderBy(x => x.Name)
                    .ToList();

                return View(items);
            }

            return View();
        }

        public ActionResult Print()
        {
            var items = _db.Products
                .Include(x => x.ReleaseItems)
                .Include(x => x.TransferItemStores)
                .Include(x => x.TransferItemWarehouses)
                .Include(x => x.AdjustItems)
                .Include(x => x.PurchaseReturnItems)//returned
                .OrderBy(x => x.Name)
                .ToList();

            return new Rotativa.ViewAsPdf(items);
        }

        public ActionResult AllLocation()
        {
            if (User.IsInRole("admin") || User.IsInRole("wrhmgr") || User.IsInRole("purchmgr"))
            {
                var items = _db.Products
                .Include(x => x.ReleaseItems)
                .Include(x => x.TransferItemStores)
                .Include(x => x.TransferItemWarehouses)
                .Include(x => x.AdjustItems)
                .OrderBy(x => x.Name)
                .ToList();

                var itemwhs = _db.ProductWarehouses
                    .Include(x => x.ReceivingApproveItems)
                    .Include(x => x.TransferItemStores)
                    .Include(x => x.TransferItemWarehouses)
                    .Include(x => x.PurchaseReturnItems)
                    .OrderBy(x => x.Name)
                    .ToList();

                // Merge ProductWarehouse and Product based on product names
                var mergedProducts = items.Select(p => new MergedProduct
                {
                    Name = p.Name,
                    StockOnHandMain = p.StockOnHand,
                    TotalStockOnHand = p.StockOnHand,
                    // Initialize StockOnHandWarehouse to 0
                    StockOnHandWarehouse = 0
                }).ToList();

                foreach (var item in itemwhs)
                {
                    var existingProduct = mergedProducts.FirstOrDefault(p => p.Name == item.Name);
                    if (existingProduct != null)
                    {
                        // Update StockOnHandWarehouse
                        existingProduct.StockOnHandWarehouse += item.StockOnHand;
                        // Update TotalStockOnHand
                        existingProduct.TotalStockOnHand += item.StockOnHand;
                    }
                    else
                    {
                        mergedProducts.Add(new MergedProduct
                        {
                            Name = item.Name,
                            StockOnHandMain = 0,
                            StockOnHandWarehouse = item.StockOnHand,
                            TotalStockOnHand = item.StockOnHand
                        });
                    }
                }

                var viewModel = new QtyOnHandViewModel
                {
                    Products = items,
                    ProductWarehouses = itemwhs,
                    MergedProducts = mergedProducts
                };

                return View(viewModel);
            }
            else if (User.IsInRole("salesmgr"))
                ViewBag.Message = "purchase & warehouse & admin can access all location";

            return View();
        }

        public ActionResult QtyOnHandPrint()
        {
            var items = _db.Products
                .Include(x => x.ReleaseItems)
                .Include(x => x.TransferItemStores)
                .Include(x => x.AdjustItems)
                .Include(x => x.TransferItemWarehouses)
                .OrderBy(x => x.Name)
                .ToList();

            var itemwhs = _db.ProductWarehouses
                .Include(x => x.ReceivingApproveItems)
                .Include(x => x.TransferItemStores)
                .Include(x => x.TransferItemWarehouses)
                .Include(x => x.PurchaseReturnItems)
                .OrderBy(x => x.Name)
                .ToList();

            // Merge ProductWarehouse and Product based on product names
            var mergedProducts = items.Select(p => new MergedProduct
            {
                Name = p.Name,
                StockOnHandMain = p.StockOnHand,
                TotalStockOnHand = p.StockOnHand,
                // Initialize StockOnHandWarehouse to 0
                StockOnHandWarehouse = 0
            }).ToList();

            foreach (var item in itemwhs)
            {
                var existingProduct = mergedProducts.FirstOrDefault(p => p.Name == item.Name);
                if (existingProduct != null)
                {
                    // Update StockOnHandWarehouse
                    existingProduct.StockOnHandWarehouse += item.StockOnHand;
                    // Update TotalStockOnHand
                    existingProduct.TotalStockOnHand += item.StockOnHand;
                }
                else
                {
                    mergedProducts.Add(new MergedProduct
                    {
                        Name = item.Name,
                        StockOnHandMain = 0,
                        StockOnHandWarehouse = item.StockOnHand,
                        TotalStockOnHand = item.StockOnHand
                    });
                }
            }

            var viewModel = new QtyOnHandViewModel
            {
                Products = items,
                ProductWarehouses = itemwhs,
                MergedProducts = mergedProducts
            };

            return new Rotativa.ViewAsPdf(viewModel);
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
using ASPNETWebApp48.Models;
using ASPNETWebApp48.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ASPNETWebApp48.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        InventoryDbContext _db = new InventoryDbContext();
        public ActionResult Homepage()
        {
            var products = _db.Products.Include(x => x.Category).OrderByDescending(p => p.Id).Take(7).ToList();
            var orders = _db.Releasings.ToList();
            var purchases = _db.Purchases.ToList();
            //var restocks = _db.RestockApproves.Include(x => x.Product).OrderBy(p => p.Product.Name).Take(7).ToList();
            var receivings = _db.Receivings.ToList();

            ViewBag.ProductCount = _db.Products.Count();
            ViewBag.ReleaseCount = _db.Releasings.Count();
            //ViewBag.ReleaseReturnCount = _db.ReleaseReturns.Count();
            //ViewBag.ReleaseReturnDmgCount = _db.ReleaseReturnDmgs.Count();
            ViewBag.TransferCount = _db.TransferDetails.Count();
            ViewBag.ReceiveApproveCount = _db.Receivings.Where(r => r.Status == "Complete" ).Count();
            ViewBag.PurchaseReturnCount = _db.PurchaseReturns.Count();
            //ViewBag.DamageReturnCount = _db.DamageReturns.Count();
            ViewBag.productwhCount = _db.ProductWarehouses.Count();            
            ViewBag.purchaseCount = _db.Purchases.Count();
            ViewBag.supplierCount = _db.Suppliers.Count();

            ViewBag.ProductLookup = _db.Products
                    .Include(x => x.ReleaseItems)
                    .Include(x => x.TransferItemStores)
                    .Include(x => x.AdjustItems)
                    .Include(x => x.PurchaseReturnItems)
                    .ToList();

            ViewBag.WarehouseProductLookup = _db.ProductWarehouses
                .Include(x => x.ReceivingApproveItems)
                .Include(x => x.TransferItemStores)
                .Include(x => x.TransferItemWarehouses)
                //.Include(x => x.PurchaseReturns)
                .ToList();

            var viewModel = new DashboardViewModel
            {
                Products = products,
                //RestockApproves = restocks,
                Releasings = orders,
                Purchases = purchases,
                Receivings = receivings
            };

            return View(viewModel);
        }

        public ActionResult PageNotFound()
        {
            return View();
        }

        [AllowAnonymous]
        [OutputCache(Duration = 300, VaryByParam = "none")] //cached for 300 seconds  
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        // DONT DELETE
        public ActionResult Pinger()
        {
            return Content(DateTime.Now.ToString());
        }

    }

    // Don't delete
    public class PingerController : Controller
    {
        public ActionResult Index()
        {
            return Content(DateTime.Now.ToString());
        }

    }
}
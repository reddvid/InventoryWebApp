using ASPNETWebApp48.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETWebApp48.ViewModels
{
    public class ProductEditViewModel
    {
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }
        public List<Brand> Brands { get; set; }
        public List<ProductSupplier> ProductSuppliers { get; set; }
        public List<Supplier> Suppliers { get; set; } // Include a list of suppliers

        // Assuming you want to select SupplierIds
        public List<int> SelectedSupplierIds { get; set; }
    }
}
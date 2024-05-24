namespace ASPNETWebApp48.Migrations
{
    using ASPNETWebApp48.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Globalization;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ASPNETWebApp48.Models.InventoryDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ASPNETWebApp48.Models.InventoryDbContext context)
        {
            //context.Categories.AddOrUpdate(
            //  c => c.Name,
            //    new Category { Name = "Building Materials" },
            //    new Category { Name = "Tools and Equipment" },
            //    new Category { Name = "Fasteners and Hardware" },
            //    new Category { Name = "Electrical Supplies" },
            //    new Category { Name = "Plumbing Supplies" },
            //    new Category { Name = "Safety and Protective Gear" },
            //    new Category { Name = "Paint and Finishes" },
            //    new Category { Name = "Hardware and Accessories" },
            //    new Category { Name = "HVAC (Heating, Ventilation, and Air Conditioning)" },
            //    new Category { Name = "Landscaping and Outdoor" },
            //    new Category { Name = "Flooring and Tiles" },
            //    new Category { Name = "Windows and Doors" },
            //    new Category { Name = "Interior Fixtures" },
            //    new Category { Name = "Roofing and Gutters" },
            //    new Category { Name = "Site and Erosion Control" },
            //    new Category { Name = "Miscellaneous Supplies" }
            //);

            //context.Brands.AddOrUpdate(
            //  c => c.Name,
            //    new Brand { Name = "Republic" },
            //    new Brand { Name = "Nihonweld" },
            //    new Brand { Name = "Eveready" },
            //    new Brand { Name = "Makita" },
            //    new Brand { Name = "Armak" },
            //    new Brand { Name = "3M" },
            //    new Brand { Name = "Boysen" },
            //    new Brand { Name = "G.I." },
            //    new Brand { Name = "Tumbo" },
            //    new Brand { Name = "Clover" },
            //    new Brand { Name = "Eagle" }
            //);

            //context.Products.AddOrUpdate(
            //  p => p.Name,
            //    new Product { Name = "Small Black AA", BrandId = 3, SKU = "ESBAA", CategoryId = 4, PurchasePrice = 44.00m, SellingPrice = 49m, Unit = "Pack", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Small Red AA", BrandId = 3, SKU = "ESRAA", CategoryId = 4, PurchasePrice = 38.00m, SellingPrice = 43m, Unit = "Pack", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "AAA Black", BrandId = 3, SKU = "", CategoryId = 4, PurchasePrice = 66.00m, SellingPrice = 71m, Unit = "Pack", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Med Black", BrandId = 3, SKU = "", CategoryId = 4, PurchasePrice = 80.00m, SellingPrice = 85m, Unit = "Pack", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Big Black", BrandId = 3, SKU = "", CategoryId = 4, PurchasePrice = 82.00m, SellingPrice = 87m, Unit = "Pack", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Big Red", BrandId = 3, SKU = "", CategoryId = 4, PurchasePrice = 68.00m, SellingPrice = 73m, Unit = "Pack", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Battery AA-Blue", BrandId = 3, SKU = "", CategoryId = 4, PurchasePrice = 15.00m, SellingPrice = 20m, Unit = "Pack", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Fuse 30A", BrandId = 11, SKU = "", CategoryId = 4, PurchasePrice = 35.00m, SellingPrice = 40m, Unit = "Piece", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Fuse 60A", BrandId = 11, SKU = "", CategoryId = 4, PurchasePrice = 75.00m, SellingPrice = 80m, Unit = "Piece", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Electrical Tape Small", BrandId = 5, SKU = "", CategoryId = 8, PurchasePrice = 20.00m, SellingPrice = 25m, Unit = "Piece", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Electrical Tape Big", BrandId = 5, SKU = "", CategoryId = 8, PurchasePrice = 45.00m, SellingPrice = 50m, Unit = "Piece", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Threebond 25g", SKU = "", BrandId = 3, CategoryId = 16, PurchasePrice = 70.00m, SellingPrice = 75m, Unit = "Gram", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Threebond 80g", SKU = "", BrandId = 3, CategoryId = 16, PurchasePrice = 115.00m, SellingPrice = 120m, Unit = "Gram", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Butane", SKU = "", BrandId = 3, CategoryId = 16, PurchasePrice = 100.00m, SellingPrice = 105m, Unit = "Can", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Staple Nail 1/2", SKU = "", BrandId = 3, CategoryId = 3, PurchasePrice = 35.00m, SellingPrice = 40m, Unit = "Pack", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Staple Nail 3/4", SKU = "", BrandId = 3, CategoryId = 3, PurchasePrice = 45.00m, SellingPrice = 50m, Unit = "Pack", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Staple Nail 1", SKU = "", BrandId = 3, CategoryId = 3, PurchasePrice = 50.00m, SellingPrice = 55m, Unit = "Pack", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Super Strength Molding Tape", BrandId = 6, SKU = "", CategoryId = 16, PurchasePrice = 910.00m, SellingPrice = 915m, Unit = "Piece", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Smooth Nails", SKU = "", BrandId = 3, CategoryId = 3, PurchasePrice = 25.00m, SellingPrice = 30m, Unit = "Grams", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Smooth Nails", SKU = "", BrandId = 3, CategoryId = 3, PurchasePrice = 130.00m, SellingPrice = 135m, Unit = "Box", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Shoe Tacks Gram", BrandId = 1, SKU = "", CategoryId = 3, PurchasePrice = 25.00m, SellingPrice = 30m, Unit = "Grams", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Shoe Tacks Box", BrandId = 1, SKU = "", CategoryId = 3, PurchasePrice = 130.00m, SellingPrice = 135m, Unit = "Box", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "4 & 1 Oil", BrandId = 1, SKU = "", CategoryId = 16, PurchasePrice = 50.00m, SellingPrice = 55m, Unit = "Bottle", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Singer Oil", BrandId = 1, SKU = "", CategoryId = 16, PurchasePrice = 60.00m, SellingPrice = 65m, Unit = "Bottle", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Soldering Lead", BrandId = 1, SKU = "", CategoryId = 8, PurchasePrice = 435.00m, SellingPrice = 440m, Unit = "Piece", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Soldering Paste", BrandId = 1, SKU = "", CategoryId = 16, PurchasePrice = 120.00m, SellingPrice = 125m, Unit = "Piece", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Metal Polish GLO", BrandId = 1, SKU = "", CategoryId = 16, PurchasePrice = 185.00m, SellingPrice = 190m, Unit = "Bottle", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Metal Polish KOWI", BrandId = 1, SKU = "", CategoryId = 16, PurchasePrice = 95.00m, SellingPrice = 100m, Unit = "Bottle", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Metal Polish A-1", BrandId = 1, SKU = "", CategoryId = 16, PurchasePrice = 130.00m, SellingPrice = 135m, Unit = "Bottle", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Compound", BrandId = 10, SKU = "CLCMP", CategoryId = 8, PurchasePrice = 125.00m, SellingPrice = 130m, Unit = "Piece", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Grease Mix", BrandId = 10, SKU = "CGMIX", CategoryId = 8, PurchasePrice = 575.00m, SellingPrice = 580m, Unit = "Piece", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Teflon 1/2", BrandId = 9, SKU = "TTF12", CategoryId = 8, PurchasePrice = 22.00m, SellingPrice = 27m, Unit = "Piece", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Teflon 3/4", BrandId = 9, SKU = "TTF34", CategoryId = 8, PurchasePrice = 38.00m, SellingPrice = 43m, Unit = "Piece", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Teflon 1", BrandId = 9, SKU = "TTF1", CategoryId = 8, PurchasePrice = 45.00m, SellingPrice = 50m, Unit = "Piece", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Rubber Tape", BrandId = 5, SKU = "ART", CategoryId = 8, PurchasePrice = 130.00m, SellingPrice = 135m, Unit = "Piece", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Nitto Rubber Tape", BrandId = 1, SKU = "NRT", CategoryId = 8, PurchasePrice = 310.00m, SellingPrice = 315m, Unit = "Piece", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Electrical Tape", BrandId = 6, SKU = "3MET", CategoryId = 8, PurchasePrice = 60.00m, SellingPrice = 65m, Unit = "Piece", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Trapal Dark Blue", BrandId = 1, SKU = "TRDB", CategoryId = 9, PurchasePrice = 90.00m, SellingPrice = 95m, Unit = "Meter", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Trapal Light Blue", BrandId = 1, SKU = "TRLB", CategoryId = 9, PurchasePrice = 75.00m, SellingPrice = 80m, Unit = "Meter", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "Trapal blu orange", BrandId = 1, SKU = "TRBLU", CategoryId = 9, PurchasePrice = 60.00m, SellingPrice = 65m, Unit = "Meter", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "NIPPLE CLOSE 1/4", BrandId = 8, SKU = "SCH40", CategoryId = 5, PurchasePrice = 18.00m, SellingPrice = 23m, Unit = "Length", MinQty = 10, CeilingQty = 100 }, //gi section
            //    new Product { Name = "NIPPLE CLOSE 3/8", BrandId = 8, SKU = "SCH40", CategoryId = 5, PurchasePrice = 16.00m, SellingPrice = 21m, Unit = "Length", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "NIPPLE CLOSE 1/2", BrandId = 8, SKU = "SCH40", CategoryId = 5, PurchasePrice = 12.00m, SellingPrice = 17m, Unit = "Length", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "NIPPLE CLOSE 3/4", BrandId = 8, SKU = "SCH40", CategoryId = 5, PurchasePrice = 14.00m, SellingPrice = 19m, Unit = "Length", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "NIPPLE CLOSE 1", BrandId = 8, SKU = "SCH40", CategoryId = 5, PurchasePrice = 27.00m, SellingPrice = 32m, Unit = "Length", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "NIPPLE CLOSE 1 1/4", BrandId = 8, SKU = "SCH40", CategoryId = 5, PurchasePrice = 30.00m, SellingPrice = 35m, Unit = "Length", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "NIPPLE CLOSE 1 1/2", BrandId = 8, SKU = "SCH40", CategoryId = 5, PurchasePrice = 42.00m, SellingPrice = 47m, Unit = "Length", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "NIPPLE 1 1/2* - 1/4", BrandId = 8, SKU = "SCH40", CategoryId = 5, PurchasePrice = 20.00m, SellingPrice = 25m, Unit = "Length", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "NIPPLE 1 1/2* - 3/8", BrandId = 8, SKU = "SCH40", CategoryId = 5, PurchasePrice = 17.00m, SellingPrice = 22m, Unit = "Length", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "NIPPLE 1 1/2* - 1/2", BrandId = 8, SKU = "SCH40", CategoryId = 5, PurchasePrice = 12.00m, SellingPrice = 17m, Unit = "Length", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "NIPPLE 1 1/2* - 3/4", BrandId = 8, SKU = "SCH40", CategoryId = 5, PurchasePrice = 15.00m, SellingPrice = 20m, Unit = "Length", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "NIPPLE 1 1/2* - 1", BrandId = 8, SKU = "SCH40", CategoryId = 5, PurchasePrice = 28.00m, SellingPrice = 33m, Unit = "Length", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "NIPPLE 1 1/2* - 1 1/4", BrandId = 8, SKU = "SCH40", CategoryId = 5, PurchasePrice = 30.00m, SellingPrice = 35m, Unit = "Length", MinQty = 10, CeilingQty = 100 },
            //    new Product { Name = "NIPPLE 1 1/2* - 1 1/2", BrandId = 8, SKU = "SCH40", CategoryId = 5, PurchasePrice = 42.00m, SellingPrice = 47m, Unit = "Length", MinQty = 10, CeilingQty = 100 }
            //    //new Product { Name = "", SKU = "", CategoryId = 4, UnitPrice = .00m, Unit = "" },
            //);

            //context.Releasings.AddOrUpdate(
            //    o => o.FirstName,
            //        new Releasing { FirstName = "Harold", LastName = "Sahagon", Status = "Not Receive", Notes = "none", OrderDate = DateTime.ParseExact("January 1 2024", "MMMM d yyyy", CultureInfo.InvariantCulture) },
            //        new Releasing { FirstName = "Tes", LastName = "Maandig", Status = "Not Receive", Notes = "none", OrderDate = DateTime.ParseExact("January 4 2024", "MMMM d yyyy", CultureInfo.InvariantCulture) },
            //        new Releasing { FirstName = "Novelito", LastName = "Yabo", Status = "Not Receive", Notes = "none", OrderDate = DateTime.ParseExact("January 7 2024", "MMMM d yyyy", CultureInfo.InvariantCulture) }
            //    );

            //context.Suppliers.AddOrUpdate(
            //    s => s.Name,
            //        new Supplier { Name = "MIGHTY STEELHOUSE MARKETING", Address = "625 M. De Binondo Street 1006 Manila Metro Manila", PhoneNo = "(02) 8734 0718", Email = "mightysteelhouse@gmail.com", Status = "Active" },
            //        new Supplier { Name = "HAN'S INFINITE TOOLS", Address = "801 TAYUMAN ST., TONDO MANILA", PhoneNo = "(02) 252-6141", Email = "", Status = "Active" },
            //        new Supplier { Name = "CLK SUPERTOOLS DEPOT", Address = "", PhoneNo = "", Email = "", Status = "Active" },
            //        new Supplier { Name = "FABRIMETRICS PHILS, INC.", Address = "1 Candido St. San Agustin Village, Brgy. Talipapa, 1116 Quezon City", PhoneNo = "(63998) 994-4056", Email = "boyet_mina@fabphils.com", Status = "Active" },
            //        new Supplier { Name = "Republic Cement", Address = "32nd Street, Bonifacio Global City Taguig City", PhoneNo = "​(+632) 8885-4599", Email = "sales@republicement.com", Status = "Active" },
            //        new Supplier { Name = "Nihonweld Company", Address = "17 Mac Arthur Hi-way, Potrero, Malabon, Manila", PhoneNo = "(+632) 8 361-0255", Email = "sales@nihonweld.com", Status = "Active" },
            //        new Supplier { Name = "Prostech Philippines", Address = "1709 Investment Dr, Alabang, Muntinlupa, 1780 Metro Manila", PhoneNo = "(+63) 960 881 8409", Email = "info@prostech.ph", Status = "Active" },
            //        new Supplier { Name = "Pacific Paint (BOYSEN®)", Address = "292 D. Tuazon Ave Quezon City, NCR", PhoneNo = "(02) 8364-9999", Email = "marketing@boysen.com.ph", Status = "Active" },
            //        new Supplier { Name = "Eveready Philippines", Address = "11 Mcarthur Highway Cor Lanzones Street, Potrero 1475 Malabon Manila", PhoneNo = "(02) 8361 2345", Email = "sales@evereadyph.com", Status = "Active" }
            //    );

            //context.Locations.AddOrUpdate(
            //  c => c.Name,
            //    new Location { Name = "Main Store" },
            //    new Location { Name = "Pacana St. Warehouse" }
            //);

            //unused
            //context.Districts.AddOrUpdate(
            //    d => d.Name,
            //        new District { Name = "Agusan", Km = 12m },
            //        new District { Name = "Baikingon", Km = 12m },
            //        new District { Name = "Balulang", Km = 0m },
            //        new District { Name = "Barangay 1(Pob)", Km = .650m },
            //        new District { Name = "Barangay 10(Pob)", Km = .550m },
            //        new District { Name = "Barangay 11(Pob)", Km = .280m },
            //        new District { Name = "Barangay 12(Pob)", Km = .350m },
            //        new District { Name = "Barangay 13(Pob)", Km = .800m },
            //        new District { Name = "Barangay 14(Pob)", Km = .500m },
            //        new District { Name = "Barangay 15(Pob)", Km = 1.1m },
            //        new District { Name = "Barangay 16(Pob)", Km = .700m },
            //        new District { Name = "Barangay 17(Pob)", Km = 1.1m },
            //        new District { Name = "Barangay 18(Pob)", Km = 1.2m },
            //        new District { Name = "Barangay 19(Pob)", Km = .800m },
            //        new District { Name = "Barangay 2(Pob)", Km = .500m },
            //        new District { Name = "Barangay 20(Pob)", Km = .950m },
            //        new District { Name = "Barangay 21(Pob)", Km = 1.7m },
            //        new District { Name = "Barangay 22(Pob)", Km = 2.2m },
            //        new District { Name = "Barangay 23(Pob)", Km = 1.4m },
            //        new District { Name = "Barangay 24(Pob)", Km = 1.8m },
            //        new District { Name = "Barangay 25(Pob)", Km = 1.2m },
            //        new District { Name = "Barangay 26(Pob)", Km = 1.9m },
            //        new District { Name = "Barangay 27(Pob)", Km = 1.1m },
            //        new District { Name = "Barangay 28(Pob)", Km = 1.4m },
            //        new District { Name = "Barangay 29(Pob)", Km = .950m },
            //        new District { Name = "Barangay 3(Pob)", Km = .400m },
            //        new District { Name = "Barangay 30(Pob)", Km = .950m },
            //        new District { Name = "Barangay 31(Pob)", Km = 1.6m },
            //        new District { Name = "Barangay 32(Pob)", Km = .800m },
            //        new District { Name = "Barangay 33(Pob)", Km = 1.2m },
            //        new District { Name = "Barangay 34(Pob)", Km = 1.1m },
            //        new District { Name = "Barangay 35(Pob)", Km = 1.4m },
            //        new District { Name = "Barangay 36(Pob)", Km = 1.3m },
            //        new District { Name = "Barangay 37(Pob)", Km = 1.2m },
            //        new District { Name = "Barangay 38(Pob)", Km = .900m },
            //        new District { Name = "Barangay 39(Pob)", Km = .700m },
            //        new District { Name = "Barangay 4(Pob)", Km = .450m },
            //        new District { Name = "Barangay 40(Pob)", Km = .650m },
            //        new District { Name = "Barangay 5(Pob)", Km = .400m },
            //        new District { Name = "Barangay 6(Pob)", Km = .650m },
            //        new District { Name = "Barangay 7(Pob)", Km = .450m },
            //        new District { Name = "Barangay 8(Pob)", Km = .200m },
            //        new District { Name = "Barangay 9(Pob)", Km = .130m },
            //        new District { Name = "Bayabas", Km = 5.0m },
            //        new District { Name = "Bayanga", Km = 17m },
            //        new District { Name = "Bonbon", Km = 3.8m },
            //        new District { Name = "Bugo", Km = 15m },
            //        new District { Name = "Bulua", Km = 5m },
            //        new District { Name = "Camaman-An", Km = 3.4m },
            //        new District { Name = "Carmen", Km = 0m },
            //        new District { Name = "Consolacion", Km = 0m },
            //        new District { Name = "Cugman", Km = 9.6m },
            //        new District { Name = "Dansolihon", Km = 25m },
            //        new District { Name = "F.S. Catanico", Km = 12m },
            //        new District { Name = "Gusa", Km = 4.6m },
            //        new District { Name = "Indahag", Km = 8.1m },
            //        new District { Name = "Iponan", Km = 9.7m },
            //        new District { Name = "Kauswagan", Km = 0m },
            //        new District { Name = "Lapasan", Km = 0m },
            //        new District { Name = "Lumbia", Km = 9.8m },
            //        new District { Name = "Macabalan", Km = 0m },
            //        new District { Name = "Macasandig", Km = 0m },
            //        new District { Name = "Mambuaya", Km = 21m },
            //        new District { Name = "Nazareth", Km = 0m },
            //        new District { Name = "Pagalungan", Km = 19m },
            //        new District { Name = "Pagatpat", Km = 12m },
            //        new District { Name = "Patag", Km = 0m },
            //        new District { Name = "Puerto", Km = 16m },
            //        new District { Name = "Puntod", Km = 2.5m },
            //        new District { Name = "San Simon", Km = 17m },
            //        new District { Name = "Tablon", Km = 9.6m },
            //        new District { Name = "Taglimao", Km = 22m },
            //        new District { Name = "Tagpangi", Km = 24m },
            //        new District { Name = "Tignapoloan", Km = 27m },
            //        new District { Name = "Tuburan", Km = 29m }
            //    );

            //STEP 2 RE UPDATE DB
            //AFTER UPDATE THEN COMMENT THIS 
            //context.ProductSuppliers.AddOrUpdate(
            //    o => o.ProductId,
            //        new ProductSupplier { ProductId = 1, SupplierId = 9 },
            //        new ProductSupplier { ProductId = 2, SupplierId = 9 },
            //        new ProductSupplier { ProductId = 3, SupplierId = 9 },
            //        new ProductSupplier { ProductId = 4, SupplierId = 9 },
            //        new ProductSupplier { ProductId = 5, SupplierId = 9 },
            //        new ProductSupplier { ProductId = 6, SupplierId = 9 },
            //        new ProductSupplier { ProductId = 7, SupplierId = 9 },
            //        new ProductSupplier { ProductId = 38, SupplierId = 4 },
            //        new ProductSupplier { ProductId = 39, SupplierId = 4 },
            //        new ProductSupplier { ProductId = 40, SupplierId = 4 },
            //        new ProductSupplier { ProductId = 41, SupplierId = 1 },
            //        new ProductSupplier { ProductId = 42, SupplierId = 1 },
            //        new ProductSupplier { ProductId = 43, SupplierId = 1 },
            //        new ProductSupplier { ProductId = 44, SupplierId = 1 },
            //        new ProductSupplier { ProductId = 45, SupplierId = 1 },
            //        new ProductSupplier { ProductId = 46, SupplierId = 1 },
            //        new ProductSupplier { ProductId = 47, SupplierId = 1 },
            //        new ProductSupplier { ProductId = 48, SupplierId = 1 },
            //        new ProductSupplier { ProductId = 49, SupplierId = 1 },
            //        new ProductSupplier { ProductId = 50, SupplierId = 1 },
            //        new ProductSupplier { ProductId = 51, SupplierId = 1 },
            //        new ProductSupplier { ProductId = 52, SupplierId = 1 },
            //        new ProductSupplier { ProductId = 52, SupplierId = 1 },
            //        new ProductSupplier { ProductId = 53, SupplierId = 1 },
            //        new ProductSupplier { ProductId = 54, SupplierId = 1 }
            //    );
        }
    }
}
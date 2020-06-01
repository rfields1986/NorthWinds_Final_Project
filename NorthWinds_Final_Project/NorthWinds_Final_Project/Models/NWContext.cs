using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using NorthWinds_Final_Project.Migrations;


namespace NorthWinds_Final_Project.Models
{
    public class NwContext : DbContext
    {
        public NwContext() : base("name=NwContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<NwContext,Configuration>());
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }



        private static NLog.Logger nLogger = NLog.LogManager.GetCurrentClassLogger();


        public Product GetProductInfo()
        {

            var database = new NwContext();
            Product newProduct = new Product();

            Console.Write("Enter Product Name--> ");
            newProduct.ProductName = Console.ReadLine();
            Console.Clear();

            Console.Write("Enter Quantity Per Unit--> ");
            newProduct.QuantityPerUnit = Console.ReadLine();
            Console.Clear();

            Console.Write("Enter Unit Price--> ");
            Decimal.TryParse(Console.ReadLine(), out decimal unitPrice);
            newProduct.UnitPrice = unitPrice;
            Console.Clear();

            Console.Write("Enter Wholesale Price--> ");
            Decimal.TryParse(Console.ReadLine(), out decimal wholesalePrice);
            newProduct.WholeSalePrice = wholesalePrice;
            Console.Clear();

            Console.Write("Enter Units In Stock--> ");
            Int16.TryParse(Console.ReadLine(), out Int16 unitsInStock);
            newProduct.UnitsInStock = unitsInStock;
            Console.Clear();

            Console.Write("Enter Units On Order--> ");
            Int16.TryParse(Console.ReadLine(), out Int16 unitsOnOrder);
            newProduct.UnitsOnOrder = unitsOnOrder;
            Console.Clear();

            Console.Write("Enter the Re-Order Level--> ");
            Int16.TryParse(Console.ReadLine(), out Int16 reOrderLevel);
            newProduct.ReorderLevel = reOrderLevel;
            Console.Clear();

            Console.Write("Is This Product Discontinued?\n1. Yes\n2. No\nEnter Number Here-->  ");
            Int32.TryParse(Console.ReadLine(), out int discontinuedChoice);

            switch (discontinuedChoice)
            {
                case 1:
                    newProduct.Discontinued = true;
                    Console.Clear();
                    break;
                case 2:
                    newProduct.Discontinued = false;
                    Console.Clear();
                    break;
            }
            var enteredCategory = new Category();
            var categoryList = database.Categories.OrderBy(c => c.CategoryName);
            foreach (var category in categoryList)
            {
                Console.WriteLine($"{"Name:",-7}{category.CategoryName,-15}{"CategoryID:",-13}{category.CategoryId,-15}");
            }

            Console.Write("\n\nPlease Select A Category From The List" +
                          "\nIf A New Category Is Needed, Enter The Name, And The Category Creation Operation Will Begin" +
                          "\nEnter Name Here--> ");
            enteredCategory.CategoryName = Console.ReadLine();
            var categorySearch = database.Categories.Any(c => c.CategoryName == enteredCategory.CategoryName);
            if (categorySearch == false)
            {
                Console.WriteLine($"\nA New Category Will Be Created Named: {enteredCategory.CategoryName}");
                Console.Write("Enter the Category Description[Required]--> ");
                enteredCategory.Description = Console.ReadLine();
                Console.Clear();

                ValidationContext context = new ValidationContext(enteredCategory, null, null);
                List<ValidationResult> results = new List<ValidationResult>();
                var notValid = Validator.TryValidateObject(enteredCategory, context, results, true);
                database.Categories.Add(enteredCategory);
                database.SaveChanges();
                nLogger.Info($"New Category {enteredCategory.CategoryName} Has Been Validated And Added To The Database.");

                var category = database.Categories.Where(c => c.CategoryName == enteredCategory.CategoryName);
                foreach (var c in category)
                {
                    newProduct.CategoryId = c.CategoryId;
                }
                Console.WriteLine($"The Category ID Is: {newProduct.CategoryId}");
                Console.ReadKey();
                Console.Clear();

            }
            else
            {
                var category = database.Categories.Where(c => c.CategoryName == enteredCategory.CategoryName);
                foreach (var c in category)
                {
                    newProduct.CategoryId = c.CategoryId;
                }

                Console.WriteLine($"The Category ID Is: {newProduct.CategoryId}"); 
                Console.Clear();
            }

            var enteredSupplier = new Supplier();
            var SupplierList = database.Suppliers.OrderBy(s => s.CompanyName);
            foreach (var supplier in SupplierList)
            {
                Console.WriteLine($"{"Supplier Name:",-17}{supplier.CompanyName,-10}");
            }
            
            Console.Write("\n\nPlease Select A Supplier From The List Above" +
                          "\nIf A New Supplier Is Needed, Enter A Name Not On The List, And The Supplier Creation Operation Will Begin" +
                          "\nEnter Name Here--> ");
            enteredSupplier.CompanyName = Console.ReadLine();
            var supplierSearch = database.Suppliers.Any(s => s.CompanyName == enteredSupplier.CompanyName);
            if (supplierSearch == false)
            {
                Console.Clear();
                Console.WriteLine($"\nA New Supplier Will Be Created Named: {enteredSupplier.CompanyName}");

                Console.Write("\nEnter the Supplier Contact Name[Required]--> ");
                enteredSupplier.ContactName = Console.ReadLine();
                Console.Clear();

                Console.Write("Enter the Supplier Contact Title[Required]--> ");
                enteredSupplier.ContactTitle = Console.ReadLine();
                Console.Clear();

                Console.Write("Enter the Supplier Address[Required]--> ");
                enteredSupplier.Address = Console.ReadLine();
                Console.Clear();

                Console.Write("Enter the Supplier City[Required]--> ");
                enteredSupplier.City = Console.ReadLine();
                Console.Clear();

                Console.Write("Enter the Supplier Region[Required]--> ");
                enteredSupplier.Region = Console.ReadLine();
                Console.Clear();

                Console.Write("Enter the Supplier Postal Code[Required]--> ");
                enteredSupplier.PostalCode = Console.ReadLine();
                Console.Clear();

                Console.Write("Enter the Supplier Country[Required]--> ");
                enteredSupplier.Country = Console.ReadLine();
                Console.Clear();

                Console.Write("Enter the Supplier Phone Number[Required]--> ");
                enteredSupplier.Phone = Console.ReadLine();
                Console.Clear();

                Console.Write("Enter the Supplier Fax Number[Required]--> ");
                enteredSupplier.Fax = Console.ReadLine();
                Console.Clear();

                ValidationContext context = new ValidationContext(enteredSupplier, null, null);
                List<ValidationResult> results = new List<ValidationResult>();
                var notValid = Validator.TryValidateObject(enteredSupplier, context, results, true);
                database.Suppliers.Add(enteredSupplier);
                database.SaveChanges();
                nLogger.Info($"New Supplier: { enteredSupplier.CompanyName} Has Been Validated And Added To The Database.");

                var supplier = database.Suppliers.Where(s => s.CompanyName == enteredSupplier.CompanyName);
                foreach (var s in supplier)
                {
                    newProduct.SupplierId = s.SupplierId;
                }
                Console.WriteLine($"The Supplier ID Is: {newProduct.SupplierId}");
                Console.Clear();

            }
            else
            {

                var supplier = database.Suppliers.Where(s => s.CompanyName == enteredSupplier.CompanyName);
                foreach (var s in supplier)
                {
                    newProduct.SupplierId = s.SupplierId;
                }
                Console.WriteLine($"The Supplier ID Is: {newProduct.SupplierId}");
                Console.Clear();
            }

            return newProduct;

        }

        public void AddOrUpdateProductToDatabase(Product product)
        {

            ValidationContext context = new ValidationContext(product, null, null);
            List<ValidationResult> results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(product, context, results, true);
            if(isValid)
            {
                var database = new NwContext();
                database.Products.AddOrUpdate(product);
                database.SaveChanges();
                nLogger.Info($"New or Updated Product: {product} Vaildated And Added To The Database @ {DateTime.Now}");
            }
            else
            {
                foreach (var result in results)
                {
                    nLogger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }


        }

        public Product GetEditedProductInfo()
        {
            var database = new NwContext();
            var productToEdit = new Product();
            database.DisplayAllProducts();

            Console.Write("\n\nPlease Select A Product From The List By Name" +
                          "\nEnter Name Here--> ");
            productToEdit.ProductName = Console.ReadLine();
            var productSearch = database.Products.Any(p => p.ProductName == productToEdit.ProductName);
            if (productSearch == false)
            {
                nLogger.Error("User Searched For Product That Does Not Exist In Database");
            }
            else
            {
                var editedProductList = database.Products.Where(p => p.ProductName == productToEdit.ProductName);
                Console.Clear();
                foreach (var p in editedProductList)
                {
                    
                    Console.WriteLine($"Product Selected To Edit:" +
                                      $"\n\n{"Name--------------->",-22}{p.ProductName,-10}" +
                                      $"\n{"Unit Price -------->",-22}{p.UnitPrice,-10}" +
                                      $"\n{"Quantity Per Unit-->",-22}{p.QuantityPerUnit,-10}" +
                                      $"\n{"Units On Order----->",-22}{p.UnitsOnOrder,-10}" +
                                      $"\n{"Wholesale Price---->",-22}{p.WholeSalePrice,-10}" +
                                      $"\n{"Units In Stock----->",-22}{p.UnitsInStock,-10}" +
                                      $"\n{"Units On Order----->",-22}{p.UnitsOnOrder,-10}" +
                                      $"\n{"Reorder Level------>",-22}{p.ReorderLevel,-10}" +
                                      $"\n{"Discontinued?------>",-22}{p.Discontinued,-10}" +
                                      $"\n{"CategoryID--------->",-22}{p.CategoryId,-10}" +
                                      $"\n{"SupplierID--------->",-22}{p.SupplierId,-10}");

                    productToEdit.ProductID = p.ProductID;
                    productToEdit.ProductName = p.ProductName;
                    productToEdit.UnitPrice = p.UnitPrice;
                    productToEdit.QuantityPerUnit = p.QuantityPerUnit;
                    productToEdit.UnitsOnOrder = p.UnitsOnOrder;
                    productToEdit.WholeSalePrice = p.WholeSalePrice;
                    productToEdit.UnitsInStock = p.UnitsInStock;
                    productToEdit.ReorderLevel = p.ReorderLevel;
                    productToEdit.Discontinued = p.Discontinued;
                    productToEdit.CategoryId = p.CategoryId;
                    productToEdit.SupplierId = p.SupplierId;
                    

                }

                Console.Write("\n\nWhat Column Would You Like To Edit?" +
                              "\n1.  Product Name" +
                              "\n2.  Unit Price" +
                              "\n3.  Quantity Per Unit" +
                              "\n4.  Units On Order" +
                              "\n5.  Wholesale Price" +
                              "\n6.  Units In Stock" +
                              "\n7.  Reorder Level" +
                              "\n8.  Discontinued Status" +
                              "\n9.  CategoryID" +
                              "\n10. SupplierID" +
                              "\n11. Edit All Columns" +
                              "\nEnter Number Here--> ");
                Int32.TryParse(Console.ReadLine(), out var editColumnChoice);
                Console.Clear();

                switch (editColumnChoice)
                {
                    case 1:
                        nLogger.Info($"User Choice:{editColumnChoice} Editing Product Name");
                        Console.Write($"Old Value: {productToEdit.ProductName}" +
                                      $"\nEnter New Value: ");
                        productToEdit.ProductName = Console.ReadLine();
                        break;
                    case 2:
                        nLogger.Info($"User Choice:{editColumnChoice} Editing Unit Price");
                        Console.Write($"Old Value: {productToEdit.UnitPrice}" +
                                      $"\nEnter New Value: ");
                        decimal.TryParse(Console.ReadLine(), out var newUnitPrice);
                        productToEdit.UnitPrice = newUnitPrice;
                        break;
                    case 3:
                        nLogger.Info($"User Choice:{editColumnChoice} Editing Quantity Per Unit");
                        Console.Write($"Old Value: {productToEdit.QuantityPerUnit}" +
                                      $"\nEnter New Value: ");
                        productToEdit.QuantityPerUnit = Console.ReadLine();
                        break;
                    case 4:
                        nLogger.Info($"User Choice:{editColumnChoice} Editing Units On Order");
                        Console.Write($"Old Value: {productToEdit.UnitsOnOrder}" +
                                      $"\nEnter New Value: ");
                        Int16.TryParse(Console.ReadLine(), out var newUnitsOnOrder);
                        productToEdit.UnitsOnOrder = newUnitsOnOrder;
                        break;
                    case 5:
                        nLogger.Info($"User Choice:{editColumnChoice} Editing Wholesale Price");
                        Console.Write($"Old Value: {productToEdit.WholeSalePrice}" +
                                      $"\nEnter New Value: ");
                        decimal.TryParse(Console.ReadLine(), out var newWholeSalePrice);
                        productToEdit.WholeSalePrice = newWholeSalePrice;
                        break;
                    case 6:
                        nLogger.Info($"User Choice:{editColumnChoice} Editing Units In Stock");
                        Console.Write($"Old Value: {productToEdit.UnitsInStock}" +
                                      $"\nEnter New Value: ");
                        Int16.TryParse(Console.ReadLine(), out var newUnitsInStock);
                        productToEdit.UnitsInStock = newUnitsInStock;
                        break;
                    case 7:
                        nLogger.Info($"User Choice:{editColumnChoice} Editing Reorder Level");
                        Console.Write($"Old Value: {productToEdit.ReorderLevel}" +
                                      $"\nEnter New Value: ");
                        Int16.TryParse(Console.ReadLine(), out var newReorderLevel);
                        productToEdit.ReorderLevel = newReorderLevel;
                        break;
                    case 8:
                        nLogger.Info($"User Choice:{editColumnChoice} Editing Discontinued Status");
                        Console.Write($"Old Value: {productToEdit.Discontinued}" +
                                      $"\nEnter New Value: ");
                        bool.TryParse(Console.ReadLine(), out var newDiscontinuedStatus);
                        productToEdit.Discontinued = newDiscontinuedStatus;
                        break;
                    case 9:
                        nLogger.Info($"User Choice:{editColumnChoice} Editing SupplierID");
                        var SupplierList = database.Suppliers.OrderBy(s => s.CompanyName);
                        foreach (var supplier in SupplierList)
                        {
                            Console.WriteLine($"{"Name:",-10}{supplier.CompanyName,-10}");
                        }
                        Console.Write($"\n\nOld Value: {productToEdit.SupplierId}" +
                                      $"\nEnter New Value From List Above: ");
                        Int32.TryParse(Console.ReadLine(), out var newSupplierId);
                        productToEdit.SupplierId = newSupplierId;
                        break;
                    case 10:
                        nLogger.Info($"User Choice:{editColumnChoice} Editing CategoryID");
                        var categoryList = database.Categories.OrderBy(c => c.CategoryName);
                        foreach (var category in categoryList)
                        {
                            Console.WriteLine($"{"Name:",-7}{category.CategoryName,-15}{"CategoryID:",-13}{category.CategoryId,-15}");
                        }
                        Console.Write($"\n\nOld Value: {productToEdit.CategoryId}" +
                                      $"\nEnter New Value From List Above: ");
                        Int32.TryParse(Console.ReadLine(), out var newCategoryId);
                        productToEdit.CategoryId = newCategoryId;
                        break;
                    case 11:
                        nLogger.Info($"User Choice:{editColumnChoice} Editing All Columns");
                        Console.Write($"Old Value: {productToEdit.ProductName}" +
                                      $"\nEnter New Value: ");
                        productToEdit.ProductName = Console.ReadLine();
                        Console.Clear();
                        Console.Write($"Old Value: {productToEdit.UnitPrice}" +
                                      $"\nEnter New Value: ");
                        decimal.TryParse(Console.ReadLine(), out var newUnitPrice11);
                        productToEdit.UnitPrice = newUnitPrice11;
                        Console.Clear();
                        Console.Write($"Old Value: {productToEdit.QuantityPerUnit}" +
                                      $"\nEnter New Value: ");
                        productToEdit.QuantityPerUnit = Console.ReadLine();
                        Console.Clear();
                        Console.Write($"Old Value: {productToEdit.UnitsOnOrder}" +
                                      $"\nEnter New Value: ");
                        Int16.TryParse(Console.ReadLine(), out var newUnitsOnOrder11);
                        productToEdit.UnitsOnOrder = newUnitsOnOrder11;
                        Console.Clear();
                        Console.Write($"Old Value: {productToEdit.WholeSalePrice}" +
                                      $"\nEnter New Value: ");
                        decimal.TryParse(Console.ReadLine(), out var newWholeSalePrice11);
                        productToEdit.WholeSalePrice = newWholeSalePrice11;
                        Console.Clear();
                        Console.Write($"Old Value: {productToEdit.UnitsInStock}" +
                                      $"\nEnter New Value: ");
                        Int16.TryParse(Console.ReadLine(), out var newUnitsInStock11);
                        productToEdit.UnitsInStock = newUnitsInStock11;
                        Console.Clear();
                        Console.Write($"Old Value: {productToEdit.ReorderLevel}" +
                                      $"\nEnter New Value: ");
                        Int16.TryParse(Console.ReadLine(), out var newReorderLevel11);
                        productToEdit.ReorderLevel = newReorderLevel11;
                        Console.Clear();
                        Console.Write($"Old Value: {productToEdit.Discontinued}" +
                                      $"\nEnter New Value: ");
                        bool.TryParse(Console.ReadLine(), out var newDiscontinuedStatus11);
                        productToEdit.Discontinued = newDiscontinuedStatus11;
                        Console.Clear();
                        var SupplierList11 = database.Suppliers.OrderBy(s => s.CompanyName);
                        foreach (var supplier in SupplierList11)
                        {
                            Console.WriteLine($"{"Name:",-10}{supplier.CompanyName,-20}{"SupplierID:",-15}{supplier.SupplierId,-10}");
                        }
                        Console.Write($"\n\nOld Value: {productToEdit.SupplierId}" +
                                      $"\nEnter New Value From List Above: ");
                        Int32.TryParse(Console.ReadLine(), out var newSupplierId11);
                        productToEdit.SupplierId = newSupplierId11;
                        Console.Clear();
                        var categoryList11 = database.Categories.OrderBy(c => c.CategoryName);
                        foreach (var category in categoryList11)
                        {
                            Console.WriteLine($"{"Name:",-7}{category.CategoryName,-15}{"CategoryID:",-13}{category.CategoryId,-15}");
                        }
                        Console.Write($"\n\nOld Value: {productToEdit.CategoryId}" +
                                      $"\nEnter New Value From List Above: ");
                        Int32.TryParse(Console.ReadLine(), out var newCategoryId11);
                        productToEdit.CategoryId = newCategoryId11;
                        break;
                }
            }

            Console.Clear();
            return productToEdit;
        }

        public void DisplayAllProducts()
        {
            var database = new NwContext();
            var productToEdit = new Product();
            var productList = database.Products.OrderBy(p => p.ProductName);
            Console.WriteLine($"{"Product ID",-20}{"Product Name",-34}{"Status"}\n{"----------",-20}{"-------------",-34}{"------"}");
            foreach (var product in productList)
            {
                Console.Write($"{product.ProductID,-20}{product.ProductName,-32}");
                if(product.Discontinued == true)
                    Console.WriteLine("  DISCONTINUED\n");
                else if(product.Discontinued == false)
                    Console.WriteLine("  ACTIVE\n");
                    
                
            }
        }

        public void DisplayActiveProducts()
        {
            var database = new NwContext();
            var productToEdit = new Product();
            var productList = database.Products.OrderBy(p => p.ProductName);
            Console.WriteLine($"{"Product ID",-20}{"Product Name",-34}{"Status"}\n{"----------",-20}{"-------------",-34}{"------"}");

            foreach (var product in productList)
            {
                if(product.Discontinued == false)
                    Console.WriteLine($"Product ID: {product.ProductID,-20}Product Name: {product.ProductName,-32}   ACTIVE\n");
  
            }
        }

        public void DisplayDiscontinuedProducts()
        {
            var database = new NwContext();
            var productToEdit = new Product();
            var productList = database.Products.OrderBy(p => p.ProductName);
            Console.WriteLine($"{"Product ID",-20}{"Product Name",-34}{"Status"}\n{"----------",-20}{"-------------",-34}{"------"}");
            foreach (var product in productList)
            {
                if(product.Discontinued == true)
                    Console.WriteLine($"Product ID: {product.ProductID,-20}Product Name: {product.ProductName,-32}   DISCONTINUED\n");
            }
        }

        public void DisplayAllColumnsForSelectedProduct()
        {
            
            var database = new NwContext();
            DisplayAllProducts();
            Console.WriteLine("\n\nWhich Product Would You Like To See?\nEnter ID Number From List Above--> ");
            Int32.TryParse(Console.ReadLine(), out var idNumber);

            var productSearch = database.Products.Where(p => p.ProductID == idNumber);
            Console.Clear();
            if (productSearch.Any() == true)
            {
                foreach (var p in productSearch)
                {
                    Console.WriteLine($"Product Selected:" +
                                      $"\n\n{"Name--------------->",-22}{p.ProductName,-10}" +
                                      $"\n{"Unit Price -------->",-22}{p.UnitPrice,-10}" +
                                      $"\n{"Quantity Per Unit-->",-22}{p.QuantityPerUnit,-10}" +
                                      $"\n{"Units On Order----->",-22}{p.UnitsOnOrder,-10}" +
                                      $"\n{"Wholesale Price---->",-22}{p.WholeSalePrice,-10}" +
                                      $"\n{"Units In Stock----->",-22}{p.UnitsInStock,-10}" +
                                      $"\n{"Units On Order----->",-22}{p.UnitsOnOrder,-10}" +
                                      $"\n{"Reorder Level------>",-22}{p.ReorderLevel,-10}" +
                                      $"\n{"Discontinued?------>",-22}{p.Discontinued,-10}" +
                                      $"\n{"CategoryID--------->",-22}{p.CategoryId,-10}" +
                                      $"\n{"SupplierID--------->",-22}{p.SupplierId,-10}");
                    Console.ReadKey();
                }
            }
            else if (productSearch.Any() == false)
            {
                nLogger.Error("User Entered ProductID Not In Database");
            }

        }

        public void DeleteAProduct()
        {

            var database = new NwContext();
            DisplayAllProducts();
            Console.WriteLine("\n\nWhich Product Would You Like To Delete?\nEnter ID Number From List Above--> ");
            Int32.TryParse(Console.ReadLine(), out var idNumber);

            var productSearch = database.Products.Where(p => p.ProductID == idNumber);
            Console.Clear();
            if (productSearch.Any() == true)
            {
                foreach (var p in productSearch)
                {
                    Console.WriteLine($"Product Selected For Deletion:" +
                                      $"\n\n{"Name--------------->",-22}{p.ProductName,-10}" +
                                      $"\n{"Unit Price -------->",-22}{p.UnitPrice,-10}" +
                                      $"\n{"Quantity Per Unit-->",-22}{p.QuantityPerUnit,-10}" +
                                      $"\n{"Units On Order----->",-22}{p.UnitsOnOrder,-10}" +
                                      $"\n{"Wholesale Price---->",-22}{p.WholeSalePrice,-10}" +
                                      $"\n{"Units In Stock----->",-22}{p.UnitsInStock,-10}" +
                                      $"\n{"Units On Order----->",-22}{p.UnitsOnOrder,-10}" +
                                      $"\n{"Reorder Level------>",-22}{p.ReorderLevel,-10}" +
                                      $"\n{"Discontinued?------>",-22}{p.Discontinued,-10}" +
                                      $"\n{"CategoryID--------->",-22}{p.CategoryId,-10}" +
                                      $"\n{"SupplierID--------->",-22}{p.SupplierId,-10}");
                    database.Products.Remove(p);
                    nLogger.Info($"Product: {p.ProductName} Has Been Removed From The Database");
                    Console.ReadKey();
                }
                database.SaveChanges();
            }
            else if (productSearch.Any() == false)
            {
                nLogger.Error("User Entered ProductID Not In Database");
            }

        }
    }


}

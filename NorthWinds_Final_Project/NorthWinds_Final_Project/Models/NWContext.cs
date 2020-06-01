using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.ServiceModel.Syndication;
using Microsoft.AspNetCore.Mvc;
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
            var exit = 0;
            do
            { 
                Console.Write("Enter Product Name--> ");
                var potentialName = Console.ReadLine();
                var productNameSearch = database.Products.Where(p => p.ProductName == potentialName);
                if ((productNameSearch.Any() == true))
                {
                    nLogger.Error("User Entered A Product Name That Already Exists");
                }
                else
                {
                    newProduct.ProductName = potentialName;
                    exit = 1;
                }
            } while(exit == 0);
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
            Console.WriteLine($"{"Name:",-20}{"CategoryID:"}\n{"-----",-20}{"-----------"}");
            foreach (var category in categoryList)
            {
                Console.WriteLine($"{category.CategoryName,-20}{category.CategoryId}");
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

                Console.WriteLine($"The Category ID Is: {newProduct.CategoryId}\nPress Any Key To Continue");
                Console.ReadKey();
                Console.Clear();
            }

            var enteredSupplier = new Supplier();
            var SupplierList = database.Suppliers.OrderBy(s => s.CompanyName);
            Console.WriteLine($"{"Name:",-40}{"SupplierID:"}{"-----",-40}{"-----------"}");
            foreach (var supplier in SupplierList)
            {
                Console.WriteLine($"\n{supplier.CompanyName,-40}{supplier.SupplierId}");
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
                    

                Console.Write("Enter the Supplier Contact Title[Not Required]--> ");
                if (Console.ReadLine() == "")
                {}
                else 
                    enteredSupplier.ContactTitle = Console.ReadLine();
                Console.Clear();

                Console.Write("Enter the Supplier Address[Not Required]--> ");
                if (Console.ReadLine() == "")
                { }
                else
                    enteredSupplier.Address = Console.ReadLine();
                Console.Clear();

                Console.Write("Enter the Supplier City[Not Required]--> ");
                if (Console.ReadLine() == "")
                { }
                else
                    enteredSupplier.City = Console.ReadLine();
                Console.Clear();

                Console.Write("Enter the Supplier Region[Not Required]--> ");
                if (Console.ReadLine() == "")
                { }
                else
                    enteredSupplier.Region = Console.ReadLine();
                Console.Clear();

                Console.Write("Enter the Supplier Postal Code[Not Required]--> ");
                if (Console.ReadLine() == "")
                { }
                else
                    enteredSupplier.PostalCode = Console.ReadLine();
                Console.Clear();

                Console.Write("Enter the Supplier Country[Not Required]--> ");
                if (Console.ReadLine() == "")
                { }
                else
                    enteredSupplier.Country = Console.ReadLine();
                Console.Clear();

                Console.Write("Enter the Supplier Phone Number[Not Required]--> ");
                if (Console.ReadLine() == "")
                { }
                else
                    enteredSupplier.Phone = Console.ReadLine();
                Console.Clear();

                Console.Write("Enter the Supplier Fax Number[Not Required]--> ");
                if (Console.ReadLine() == "")
                { }
                else
                    enteredSupplier.Fax = Console.ReadLine();
                Console.Clear();

                ValidationContext context = new ValidationContext(enteredSupplier, null, null);
                List<ValidationResult> results = new List<ValidationResult>();
                var notValid = Validator.TryValidateObject(enteredSupplier, context, results, true);
                database.Suppliers.Add(enteredSupplier);
                database.SaveChanges();
                nLogger.Info($"New Supplier: {enteredSupplier.CompanyName} Has Been Validated And Added To The Database.");

                var supplier = database.Suppliers.Where(s => s.CompanyName == enteredSupplier.CompanyName);
                foreach (var s in supplier)
                {
                    newProduct.SupplierId = s.SupplierId;
                }
                Console.WriteLine($"The Supplier ID Is: {newProduct.SupplierId}\nPress Any Key To Continue");
                Console.ReadKey();
                Console.Clear();

            }
            else
            {

                var supplier = database.Suppliers.Where(s => s.CompanyName == enteredSupplier.CompanyName);
                foreach (var s in supplier)
                {
                    newProduct.SupplierId = s.SupplierId;
                }
                Console.WriteLine($"The Supplier ID Is: {newProduct.SupplierId}\nPress Any Key To Continue");
                Console.ReadKey();
                Console.Clear();
            }

            return newProduct;

        }

        public void AddOrUpdateProductToDatabase(Product product)
        {


            try
            {
                ValidationContext context = new ValidationContext(product, null, null);
                List<ValidationResult> results = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(product, context, results, true);
                if (isValid == true)
                {
                    var database = new NwContext();
                    database.Products.AddOrUpdate(product);
                    database.SaveChanges();
                    nLogger.Info($"New or Updated Product: {product} Vaildated And Added To The Database @ {DateTime.Now}");
                    Console.WriteLine("Press Any Key To Continue");
                    Console.ReadKey();
                }
                else
                {
                    foreach (var result in results)
                    {
                        nLogger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
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
            while (productSearch == false) 
            {
                
                nLogger.Error("User Searched For Product That Does Not Exist In Database");
                Console.WriteLine("Press Any Key To Continue");
                Console.ReadKey();
                Console.Clear();
                database.DisplayAllProducts();

                Console.Write("\n\nPlease Select A Product From The List By Name" +
                              "\nEnter Name Here--> ");
                productToEdit.ProductName = Console.ReadLine();
                productSearch = database.Products.Any(p => p.ProductName == productToEdit.ProductName);
            }

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
                              "\n9.  SupplierID" +
                              "\n10. CategoryID" +
                              "\n11. Edit All Columns" +
                              "\nEnter Number Here--> ");
                Int32.TryParse(Console.ReadLine(), out var editColumnChoice);
                Console.Clear();

                switch (editColumnChoice)
                {
                    case 1:
                        nLogger.Info($"User Choice:{editColumnChoice} Editing Product Name");
                        var exit111 = 0;
                        do
                        {
                            Console.Write($"Old Value: {productToEdit.ProductName}" +
                                          $"\nEnter New Value: ");
                        var potentialName = Console.ReadLine();
                            var productNameSearch = database.Products.Where(p => p.ProductName == potentialName);
                            if ((productNameSearch.Any() == true))
                            {
                                nLogger.Error("User Entered A Product Name That Already Exists");
                            }
                            else
                            {
                                productToEdit.ProductName = potentialName;
                                exit111 = 1;
                            }
                        } while (exit111 == 0);
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
                        var exit = 0;
                    do
                    {
                        var SupplierList = database.Suppliers.OrderBy(s => s.CompanyName);
                        Console.WriteLine($"{"Name:",-40}{"SupplierID:"}\n{"-----",-40}{"-----------"}");
                        foreach (var supplier in SupplierList)
                        {
                            Console.WriteLine($"\n{supplier.CompanyName,-40}{supplier.SupplierId}");
                        }
                        Console.Write($"\n\nOld Value: {productToEdit.SupplierId}" +
                                          $"\nEnter New Value From List Above: ");
                        Int32.TryParse(Console.ReadLine(), out var newSupplierId);
                        if (database.Suppliers.Any(s => s.SupplierId == newSupplierId) == true)
                        {
                            productToEdit.SupplierId = newSupplierId;
                            Console.WriteLine($"SupplierID is set to:{productToEdit.SupplierId}\nPress Any Key To Continue");
                            Console.ReadKey();
                            exit = 1;
                        }
                        else
                        {
                            nLogger.Error("User Selected SupplierID Not In DataBase");
                            Console.WriteLine("Press Any Key To Continue");
                            Console.ReadKey();
                            Console.Clear();
                        }
                    } while (exit == 0);
                        break;
                    case 10:
                        nLogger.Info($"User Choice:{editColumnChoice} Editing CategoryID");
                        var exit1 = 0;
                    do
                    {
                        var categoryList = database.Categories.OrderBy(c => c.CategoryName);
                        Console.WriteLine($"{"Name:",-20}{"CategoryID:"}\n{"-----",-20}{"-----------"}");
                        foreach (var category in categoryList)
                        {
                            Console.WriteLine($"{category.CategoryName,-20}{category.CategoryId}");
                        }
                        Console.Write($"\n\nOld Value: {productToEdit.CategoryId}" +
                                      $"\nEnter New Value From List Above: ");
                        Int32.TryParse(Console.ReadLine(), out var newCategoryId);
                        if (database.Categories.Any(c => c.CategoryId == newCategoryId) == true)
                        {
                            productToEdit.CategoryId = newCategoryId;
                            Console.WriteLine($"CatergoryID Is Set To:{productToEdit.CategoryId}\nPress Any Key To Continue");
                            Console.ReadKey();
                            exit1 = 1;
                        }
                        else
                        {
                            nLogger.Error("User Selected CategoryID Not In DataBase");
                            Console.WriteLine("Press Any Key To Continue");
                            Console.ReadKey();
                            Console.Clear();
                        }
                    } while (exit1 == 0); 
                        break;
                    case 11:
                        nLogger.Info($"User Choice:{editColumnChoice} Editing All Columns");

                        var exit15 = 0;
                        do
                        {
                            Console.Write($"Old Value: {productToEdit.ProductName}" +
                                          $"\nEnter New Value: ");
                            var potentialName = Console.ReadLine();
                            var productNameSearch = database.Products.Where(p => p.ProductName == potentialName);
                            if ((productNameSearch.Any() == true))
                            {
                                nLogger.Error("User Entered A Product Name That Already Exists");
                            }
                            else
                            {
                                productToEdit.ProductName = potentialName;
                                exit15 = 1;
                            }
                        } while (exit15 == 0);
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
                        var exit11 = 0;
                        do
                        {
                            var SupplierList = database.Suppliers.OrderBy(s => s.CompanyName);
                            Console.WriteLine($"{"Name:",-40}{"SupplierID:"}{"-----",-40}{"-----------"}");
                            foreach (var supplier in SupplierList)
                            {
                                Console.WriteLine($"\n{supplier.CompanyName,-40}{supplier.SupplierId}");
                            }
                            Console.Write($"\n\nOld Value: {productToEdit.SupplierId}" +
                                          $"\nEnter New Value From List Above: ");
                            Int32.TryParse(Console.ReadLine(), out var newSupplierId);
                            if (database.Suppliers.Any(s => s.SupplierId == newSupplierId) == true)
                            {
                                productToEdit.SupplierId = newSupplierId;
                                Console.WriteLine($"SupplierID is set to:{productToEdit.SupplierId}\nPress Any Key To Continue");
                                Console.ReadKey();
                                exit11 = 1;
                            }
                            else
                            {
                                nLogger.Error("User Selected SupplierID Not In DataBase");
                                Console.WriteLine("Press Any Key To Continue");
                                Console.ReadKey();
                                Console.Clear();
                            }
                        } while (exit11 == 0);
                    Console.Clear();
                        var exit12 = 0;
                        do
                        {
                            var categoryList = database.Categories.OrderBy(c => c.CategoryName);
                            Console.WriteLine($"{"Name:",-20}{"CategoryID:"}\n{"-----",-20}{"-----------"}");
                            foreach (var category in categoryList)
                            {
                                Console.WriteLine($"{category.CategoryName,-20}{category.CategoryId}");
                            }
                            Console.Write($"\n\nOld Value: {productToEdit.CategoryId}" +
                                          $"\nEnter New Value From List Above: ");
                            Int32.TryParse(Console.ReadLine(), out var newCategoryId);
                            if (database.Categories.Any(c => c.CategoryId == newCategoryId) == true)
                            {
                                productToEdit.CategoryId = newCategoryId;
                                Console.WriteLine($"CatergoryID Is Set To:{productToEdit.CategoryId}\nPress Any Key To Continue");
                                Console.ReadKey();
                                exit12 = 1;
                            }
                            else
                            {
                                nLogger.Error("User Selected CategoryID Not In DataBase");
                                Console.WriteLine("Press Any Key To Continue");
                                Console.ReadKey();
                                Console.Clear();
                            }
                    } while (exit12 == 0);
                    break;
                }
            

            Console.WriteLine("Press Any Key To Return To Main Menu");
            Console.ReadKey();
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

            Console.WriteLine("Press Any Key To Continue");
            Console.ReadKey();
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
            Console.WriteLine("Press Any Key To Continue");
            Console.ReadKey();
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
            Console.WriteLine("Press Any Key To Continue");
            Console.ReadKey();
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

        public void DisplayAllCategories()
        {
            var database = new NwContext();
            var categoryList = database.Categories.OrderBy(c => c.CategoryName);
            Console.WriteLine($"{"Name:",-20}{"Category Description:"}\n{"-----",-20}{"---------------------"}");
            foreach (var category in categoryList)
            {
                Console.WriteLine($"{category.CategoryName,-20}{category.Description}");
            }

            Console.WriteLine("Press Any Key To Continue");
            Console.ReadKey();

        }

        public void DisplayAllCategoriesAndProducts()
        {
            var database = new NwContext();
            var productList = database.Products.OrderBy(p => p.ProductID);
            List<Product> productName = new List<Product>();
            foreach (var p in productList)
            {
                productName.Add(p);
            }
            var categoryList = database.Categories.OrderBy(c => c.CategoryId);
            List<Category> categories = new List<Category>();
            foreach (var c in categoryList)
            {
                categories.Add(c);
            }
            foreach(var c in categories)
            {
                Console.WriteLine($"\n\n{"Category Name:",-15}\n{"--------------"}\n{c.CategoryName}\n");
                Console.WriteLine($"    {"Product Name:",-20}\n    {"-------------"}");
                foreach (var p in productName)
                {
                    if (c.CategoryId == p.CategoryId)
                    {
                        Console.WriteLine($"    {p.ProductName}");
                    }
                }

            }

            Console.WriteLine("Press Any Key To Continue");
            Console.ReadKey();

        }

        public void DisplayACategoryAndProducts()
        {
            var database = new NwContext();
            var enteredCategory = new Category();
            var categoryList = database.Categories.OrderBy(c => c.CategoryName);
            Console.WriteLine($"{"Name:",-20}{"CategoryID:"}\n{"-----",-20}{"-----------"}");
            foreach (var category in categoryList)
            {
                Console.WriteLine($"{category.CategoryName,-20}{category.CategoryId}");
            }

            Console.Write("\n\nPlease Select A Category From The List" +
                          "\nEnter Name Here--> ");
            enteredCategory.CategoryName = Console.ReadLine();
            var categorySearch = database.Categories.Where(c => c.CategoryName == enteredCategory.CategoryName);
            var productList = database.Products.OrderBy(p1 => p1.ProductID);
            List<Product> productName = new List<Product>();
            foreach (var p in productList)
            {
                productName.Add(p);
            }
            List<Category> categories = new List<Category>();
            foreach (var c in categorySearch)
            {
             categories.Add(c);   
            }
            foreach (var c in categories)
            {
                Console.WriteLine($"\n\n{"Category Name:",-15}\n{"--------------"}\n{c.CategoryName}\n");
                Console.WriteLine($"    {"Product Name:",-20}\n    {"-------------"}");
                foreach (var p in productName)
                {
                    if (c.CategoryId == p.CategoryId)
                    {
                        Console.WriteLine($"    {p.ProductName}");
                    }
                }

            }
            Console.WriteLine("Press Any Key To Continue");
            Console.ReadKey();


        }

        public void DeleteCategoryAndProducts()
        {
            var database = new NwContext();
            var enteredCategory = new Category();
            var categoryList = database.Categories.OrderBy(c => c.CategoryName);
            Console.WriteLine($"{"Name:",-20}{"CategoryID:"}\n{"-----",-20}{"-----------"}");
            foreach (var category in categoryList)
            {
                Console.WriteLine($"{category.CategoryName,-20}{category.CategoryId}");
            }

            Console.Write("\n\nPlease Select A Category From The List" +
                          "\nEnter Name Here--> ");
            enteredCategory.CategoryName = Console.ReadLine();
            var categorySearch = database.Categories.Where(c => c.CategoryName == enteredCategory.CategoryName);
            var productList = database.Products.OrderBy(p1 => p1.ProductID);
            List<Product> productName = new List<Product>();
            foreach (var p in productList)
            {
                productName.Add(p);
            }
            List<Category> categories = new List<Category>();
            foreach (var c in categorySearch)
            {
                categories.Add(c);
            }
            foreach (var c in categories)
            {
                Console.WriteLine($"\n\n{"Category Name:",-15}\n{"--------------"}\n{c.CategoryName}\n");
                Console.WriteLine($"    {"Product Name:",-20}\n    {"-------------"}");
                foreach (var p in productName)
                {
                    if (c.CategoryId == p.CategoryId)
                    {
                        Console.WriteLine($"    {p.ProductName}");
                    }
                }
                Console.Write("The Category And Its Products Will Be Deleted\nAre You Sure?\n1. Yes\n2. No\nEnter Choice Here--> ");
                Int32.TryParse(Console.ReadLine(), out var deletionChoice);
                if (deletionChoice == 1)
                {
                    foreach (var p in productName)
                    {
                        if (c.CategoryId == p.CategoryId)
                        {
                            database.Products.Remove(p);
                        }
                    }

                    database.Categories.Remove(c);
                    database.SaveChanges();
                    nLogger.Info($"User Removed{c.CategoryName} And All Its Products From The Database");
                    Console.WriteLine("Press Any Key To Continue");
                    Console.ReadKey();
                }


            }


        }

        public Category GetCategoryInfo()
        {
            var database = new NwContext();
            var newCategory = new Category();
            var exit = 0;
            do
            {

                Console.Write("\n\nPlease Choose A Name For The New Category" +
                              "\nEnter Name Here--> ");
                newCategory.CategoryName = Console.ReadLine();
                var categorySearch = database.Categories.Any(c => c.CategoryName == newCategory.CategoryName);
                if (categorySearch == false)
                {
                    Console.WriteLine($"\nA New Category Will Be Created Named: {newCategory.CategoryName}");
                    Console.Write("Enter the Category Description[Required]--> ");
                    newCategory.Description = Console.ReadLine();
                    Console.Clear();
                    exit = 1;
                }
                else
                {
                    nLogger.Error("User Entered Category That Already Exists");
                    Console.WriteLine("Press Any Key To Continue");
                } 
            } while (exit == 0);

            return newCategory;
        }

        public Category GetEditedCategoryInfo()
        {
            var database = new NwContext();
            var editedCategory = new Category();
            var exit = 0;
            do
            {

                Console.Write("\n\nPlease Choose A Name For The Category To Be Edited" +
                              "\nEnter Name Here--> ");
                editedCategory.CategoryName = Console.ReadLine();
                var categorySearch = database.Categories.Where(c => c.CategoryName == editedCategory.CategoryName);
                
                if (categorySearch.Any() == true )
                {
                    foreach (var c in categorySearch)
                    {
                        editedCategory.CategoryId = c.CategoryId;
                    }
                    Console.WriteLine($"\nThe Category To Be Edited: {editedCategory.CategoryName}");
                    Console.Write("Enter the Category Description[Required]--> ");
                    editedCategory.Description = Console.ReadLine();
                    Console.Clear();
                    exit = 1;
                }
                else
                {
                    nLogger.Error("User Entered Category That Already Exists");
                    Console.WriteLine("Press Any Key To Continue");
                }
            } while (exit == 0);

            return editedCategory;
        }

        public void AddOrUpdateCategory(Category newCategory)
        {
            var database = new NwContext();
            ValidationContext context = new ValidationContext(newCategory, null, null);
            List<ValidationResult> results = new List<ValidationResult>();
            var notValid = Validator.TryValidateObject(newCategory, context, results, true);
            if (notValid == true)
            {
                database.Categories.AddOrUpdate(newCategory);
                database.SaveChanges();
                nLogger.Info($"New or Updated Category: {newCategory} Vaildated And Added To The Database @ {DateTime.Now}");
                Console.WriteLine("Press Any Key To Continue");
                Console.ReadKey();
            }
            else
            {
                foreach (var result in results)
                {
                    nLogger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
        }
    }


}

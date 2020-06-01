using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using NLog;
using NorthWinds_Final_Project.Models;


namespace NorthWinds_Final_Project
{
    class Program
    {
        private static NLog.Logger nLogger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            nLogger.Info($"Program Started @ {DateTime.Now}");

            try
            {

                bool exit = false;

                do
                {
                    int userChoice = DisplayMenu();
                    nLogger.Info($"User Choice On Display Menu: {userChoice} @ {DateTime.Now}");


                    switch (userChoice)
                    {
                        case 1: //products
                            var userProductsChoice = DisplayProductSubMenu1();
                            nLogger.Info($"User Choice On Product SubMenu 1: {userProductsChoice} @ {DateTime.Now}");
                            var database = new NwContext();

                            switch (userProductsChoice)
                            {
                                case 1: //Add
                                    database.AddOrUpdateProductToDatabase(database.GetProductInfo());
                                    break;
                                case 2: //Edit
                                    database.AddOrUpdateProductToDatabase(database.GetEditedProductInfo());
                                    break;
                                case 3: //Display All
                                    var userProductDisplayChoice = DisplayProductSubMenu2();
                                    nLogger.Info(
                                        $"User Choice On Product SubMenu 2: {userProductDisplayChoice} @ {DateTime.Now}");

                                    switch (userProductDisplayChoice)
                                    {
                                        case 1: //All
                                            database.DisplayAllProducts();
                                            break;
                                        case 2: //Discontinued
                                            database.DisplayDiscontinuedProducts();
                                            break;
                                        case 3: //Active
                                            database.DisplayActiveProducts();
                                            break;
                                        case 0: //Return To Main Menu
                                            break;
                                    }
                                    break;

                                case 4: //Display 1
                                    database.DisplayAllColumnsForSelectedProduct();
                                    break;
                                case 5: //delete
                                    database.DeleteAProduct();
                                    break;
                                case 0: //Return To Main Menu
                                    break;
                            }

                            break;
                        case 2: //categories
                            break;
                        case 0: //quit
                            exit = true;
                            break;
                    }
                } while (exit == false);

                nLogger.Info($"User Ended Program @ {DateTime.Now}");
                Console.WriteLine("Program Closing");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }



        }


        public static int DisplayMenu()
        {
            Console.Clear();
            Console.Write("Please Choose Which Table To Edit" +
                          "\n1. Products" +
                          "\n2. Categories" +
                          "\nOr Press Enter To Quit" +
                          "\n\nEnter Number Here--> ");
            Int32.TryParse(Console.ReadLine(), out var userMenuChoice);
            Console.Clear();
            return userMenuChoice;
        }

        public static int DisplayProductSubMenu1()
        {
            Console.Write("Please Select What Operation To Perform" +
                          "\n1. Add A New Product" +
                          "\n2. Edit A Existing Product" +
                          "\n3. Display All Products" +
                          "\n4. Display A Products Information" +
                          "\n5. Delete A Product" +
                          "\nOr Press Enter To Return To Main Menu" +
                          "\n\nEnter Number Here--> ");
            Int32.TryParse(Console.ReadLine(), out var userSubMenuChoice);
            Console.Clear();
            return userSubMenuChoice;
        }

        public static int DisplayProductSubMenu2()
        {
            Console.Write("Please Choose Which Type Of Product To Display" +
                          "\n1. All Products" +
                          "\n2. Discontinued Products" +
                          "\n3. Active Products" +
                          "\nOr Press Enter To Return To Main Menu" +
                          "\n\nEnter Number Here--> ");
            Int32.TryParse(Console.ReadLine(), out var userSubMenuChoice2);
            Console.Clear();
            return userSubMenuChoice2;
        }

    }
}

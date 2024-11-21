using KASSASYSTEMB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace KASSASYSTEMB
{


    class Program
    {
        public static void writeProducts()
        {
            var outFile = "../../../products.txt";
            Product product = new Product("Banan", 4, 200);
            Product product1 = new Product("Cola", 15, 300);
            Product product2 = new Product("Cafe", 20, 400);
            Product product3 = new Product("Tomat", 5, 500);

            using (StreamWriter sw = File.CreateText(outFile))
            {
                sw.WriteLine(product.getProductLine());
                sw.WriteLine(product1.getProductLine());
                sw.WriteLine(product2.getProductLine());
                sw.WriteLine(product3.getProductLine());

            }
        }

        public static List<Product> readProducts()
        {
            var inFile = "../../../products.txt";
            var products = new List<Product>();

            using (StreamReader sr = File.OpenText(inFile))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var product = new Product(line);
                    products.Add(product);
                }
            }
            return products;
        }

        public static int searchProducts(int productid, List<Product> products)
        {
            int position = -1;

            for (int i = 0; i < products.Count; i++)
            {
                if (productid == products[i].ID)
                {
                    position = i;
                }
            }
            return position;
        }


        public static string checkInputType(string input)
        {
            string inputType = "";
            var inputList = input.Split(' ');
            if ((inputList.Count() == 1) && (input.ToUpper() == ("PAY")))
            {
                inputType = "PAY";
            }
            else if ((inputList.Count() == 2) && (inputList[0].ToUpper() == "RETURN"))
            {
                inputType = "RETURN";
            }
            else if (inputList.Count() == 2)
            {
                inputType = "CONTINUE";
            }
            else
            {
                inputType = "ERROR";
            }
            return inputType;

        }

        public static int checkReturnInput(string input)
        {
            int productID;
            var inputList = input.Split(' ');
            bool result = int.TryParse(inputList[1], out productID);
            if (result == false)
            {
                productID = -1;
            }
            return productID;
        }

        public static Tuple<int, int> checkContinueInput(string input)
        {
            var inputList = input.Split(' ');
            int productID;
            int quantity;

            bool resultProductID = int.TryParse(inputList[0], out productID);
            bool resultQuantity = int.TryParse(inputList[1], out quantity);

            if ((resultProductID == true) && (resultQuantity == true))
            {
                return Tuple.Create(productID, quantity);
            }
            else
            {
                return Tuple.Create(-1, -1); // error in return command
            }
        }


        public static void run(List<Product> products)
        {
            var recepit = new Recepit();
            while (true)
            {
                int productID = -1;
                Product product;


                Console.WriteLine("\n**Tillåtna kommandon**");

                Console.WriteLine("PAY");
                Console.WriteLine("RETURN <PRODUKTID>");
                Console.WriteLine("<Produktid>  <Antal>");


                string input = Console.ReadLine();

                var inputType = checkInputType(input);

                if (inputType == "ERROR")
                {
                    Console.WriteLine("Fel kommando");
                }
                else if (inputType == "PAY")
                {
                    if (recepit.RecepitRows.Count() != 0)
                    {
                        recepit.save();
                    }
                    else
                    {
                        Console.WriteLine("Inget kvitto har sparats. Välkomen Åter :)");
                    }
                    break;
                }
                else if (inputType == "RETURN")
                {
                    if (recepit.RecepitRows.Count() != 0)
                    {
                        productID = checkReturnInput(input);

                        if (productID == -1)
                        {
                            Console.WriteLine("Fel kommando");
                        }
                        else
                        {
                            productID = checkReturnInput(input);
                            product = products.Find(p => p.ID == productID);
                            if (product != null)
                            {
                                recepit.decreaseQuantity(productID);
                                recepit.print();
                            }
                            else
                            {
                                Console.WriteLine("Produkten finns ej!");
                            }
                        }
                    }
                }
                else
                {
                    // CONTINUE
                    var validatedInput = checkContinueInput(input);

                    if (validatedInput.Item1 == -1)
                    {
                        Console.WriteLine("Fel kommando");
                    }
                    else
                    {
                        product = products.Find(p => p.ID == validatedInput.Item1);
                        if (product != null)
                        {
                            var receiptRow = new RecepitRow(product.Name, validatedInput.Item2, product.Price, product.ID);
                            recepit.addRecepitRow(receiptRow);
                            recepit.print();
                        }
                        else
                        {
                            Console.WriteLine("Produkten finns ej!");
                        }
                    }

                }

            }
        }




        static void Main(string[] args)
        {
            var products = readProducts();
            while (true)
            {
                Console.WriteLine("KASSA");
                Console.WriteLine("1. Ny kund");
                Console.WriteLine("2. Adminverktyg");
                Console.WriteLine("0. Avsluta");
                var input = (Console.ReadLine());
                if (input == "1")
                {
                    run(products);
                }
                else if (input == "2")
                {
                    //edit(products);
                }

                else if (input == "0")
                {
                    break;
                }

                else
                {
                    Console.WriteLine("FEL INMATNING FÖRSÖK IGEN!!");
                }
            }
        }
    }
}


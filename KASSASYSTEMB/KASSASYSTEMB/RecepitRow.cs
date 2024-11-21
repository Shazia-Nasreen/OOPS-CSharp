using System;
using System.Collections.Generic;

namespace KASSASYSTEMB
{
    public class RecepitRow
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int Sum { get; set; }
        public int Total { get; set; }
        public int ProductId { get; set; }

        public RecepitRow(string productname, int quantity, int price, int prodcutId)
        {
            ProductName = productname;
            Quantity = quantity;
            Price = price;
            Sum = price * quantity;
            ProductId = prodcutId;
        }
        public void printRow()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{ProductName} {Quantity} * {Price}kr = {Sum}kr ");
            Console.ResetColor();

        }
        public void updateSum()
        {
            Sum = Price * Quantity;
        }

    }
}
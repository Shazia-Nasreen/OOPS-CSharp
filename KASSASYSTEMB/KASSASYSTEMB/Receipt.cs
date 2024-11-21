using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using KASSASYSTEMB;


namespace KASSASYSTEMB
{
    public class Recepit
    {
        private double Total { get; set; }
        private DateTime ReceiptDateTime { get; set; }
        public List<RecepitRow> RecepitRows { get; set; }
        private double Discount { get; set; }
        private double ItemsTotal { get; set; }


        public Recepit()
        {
            ReceiptDateTime = DateTime.Now;
            Total = 0;
            RecepitRows = new List<RecepitRow>();
            Discount = 0;
            ItemsTotal = 0;
        }

        public void addRecepitRow(RecepitRow recepitRow)
        {
            int index = receiptRowExists(recepitRow.ProductId);
            if (index == -1)
            {
                RecepitRows.Add(recepitRow);
            }
            else
            {
                RecepitRows[index].Quantity += recepitRow.Quantity;
                RecepitRows[index].updateSum();
            }

            ItemsTotal = RecepitRows.Sum(item => item.Sum);
        }

        public void decreaseQuantity(int productID)
        {
            int index = receiptRowExists(productID);
            if (index != -1)
            {
                RecepitRows[index].Quantity -= 1;
                RecepitRows[index].updateSum();
                ItemsTotal = RecepitRows.Sum(item => item.Sum);
            }

        }

        public void print()
        {
            printDate();
            printRecepitRows();
            printDiscount();
            printTotal();
        }

        public void save()
        {
            string timestamp = DateTime.Today.ToString("yyyy_MMdd");
            var outFile = $"../../../receipts/{timestamp}.txt";
            using (StreamWriter sw = new StreamWriter(outFile, true))
            {
                sw.WriteLine(ReceiptDateTime.ToString());
                sw.WriteLine(Total);
                sw.WriteLine(ItemsTotal);
                sw.WriteLine(Discount);
                foreach (var receiptRow in RecepitRows)
                {
                    string receiptRowLineline = $"{receiptRow.ProductId}, {receiptRow.ProductName}, {receiptRow.Price}, {receiptRow.Quantity}, {receiptRow.Sum}";
                    sw.WriteLine(receiptRowLineline);
                }
                sw.WriteLine("*****");
            }
        }

        public void printRecepitRows()
        {
            foreach (var recepitRow in RecepitRows)
            {
                recepitRow.printRow();
            }
        }

        private int receiptRowExists(int productID)
        {
            return RecepitRows.FindIndex(item => item.ProductId == productID);
        }

        private void cheackDiscount()
        {
            if (ItemsTotal >= 1001 && ItemsTotal <= 2000)
            {
                Discount = Math.Round((ItemsTotal * 0.02), 2);
                Total = ItemsTotal - Discount;
            }
            else if (ItemsTotal > 2000)
            {
                Discount = Math.Round((ItemsTotal * 0.03), 2);
                Total = ItemsTotal - Discount;
            }
        }

        private void printDiscount()
        {
            cheackDiscount();
            if (Discount > 0)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Items Total:  {ItemsTotal} kr");
                Console.WriteLine($"Rabbat:  -{Discount} kr");
                Console.ResetColor();
            }
            else
            {
                Total = ItemsTotal;
            }
        }

        private void printDate()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"KVITOO  {ReceiptDateTime}");
            Console.ResetColor();
        }

        private void printTotal()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Total:  {Total} kr");
            Console.ResetColor();

        }

    }
}
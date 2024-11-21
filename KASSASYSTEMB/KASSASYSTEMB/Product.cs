using System;
using System.Collections.Generic;

namespace KASSASYSTEMB
{
    public class Product
    {
        public string Name { get; private set; }
        public int Price { get; private set; }
        public int ID { get; private set; }

        public Product(string name, int price, int id)
        {
            Name = name;
            Price = price;
            ID = id;
        }

        public Product(string line)
        {
            string[] words = line.Split(',');
            ID = Convert.ToInt32(words[0]);
            Name = words[1];
            Price = Convert.ToInt32(words[2]);
        }

        public string getProductLine()
        {
            return $"{ID}, {Name}, {Price}";
        }
    }
}
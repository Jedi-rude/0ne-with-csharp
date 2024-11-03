using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_Appliance_System_SQL
{
    class Product
    {
        private int id;
        private string brand;
        private string category;
        private string model;
        private string power;
        private string color;
        private string size;
        private int price;

        public int ID { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string Model { get; set; }
        public string Power { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int Price { get; set; }
    }
}

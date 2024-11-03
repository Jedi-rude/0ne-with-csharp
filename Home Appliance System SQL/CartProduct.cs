using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home_Appliance_System_SQL
{
    class CartProduct
    {
        private int id;
        private string brand;
        private string model;
        private int price;
        private int quantity;
        private int duration;

        public int ID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int Duration { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingE_Commerce.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }
        public Game Game { get; set; }
        public int NoOfItems { get; set; }
        public string ShoppingCartId { get; set; }
    }
}

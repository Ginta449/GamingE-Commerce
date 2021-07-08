using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingE_Commerce.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; }
        public DateTime OrdrDate { get; set;}
        public string PaymentType { get; set; }
        public string Status { get; set; }
        public string FirstName { get; set; }
        public string LastName{ get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string County { get; set; }
        public string Town { get; set; }
        public string Eircode{ get; set; }
        
    }
}

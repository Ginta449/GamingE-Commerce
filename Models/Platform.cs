using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingE_Commerce.Models
{
    public class Platform
    {
        public int Id { get; set; }
        public string PlatformName { get; set; }
        public string PlatformDescription { get; set; }
        public string PlatformImageName { get; set; }
        public string aspRouteIdName { get; set; }
        public string ViewLinkName { get; set; }
    }
}

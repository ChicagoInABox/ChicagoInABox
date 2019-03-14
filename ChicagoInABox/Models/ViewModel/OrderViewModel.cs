using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicagoInABox.Models.ViewModel
{
    public class OrderViewModel : Order
    {
        public bool AlternateShipAddress { get; set; }
    }
}
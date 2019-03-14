using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicagoInABox.Models.ViewModel
{
    public class OrderDetailViewModel
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public bool AlternateShipAddress { get; set; }
        public int ShipToID { get; set; }
        public int BillToID { get; set; }
        public int OrderQty { get; set; }
        public int LineID { get; set; }
        public decimal LineQty { get; set; }
        public int ItemID { get; set; }
    }
}
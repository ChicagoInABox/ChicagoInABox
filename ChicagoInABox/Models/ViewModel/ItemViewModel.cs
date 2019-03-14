using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace ChicagoInABox.Models.ViewModel
{
    public class ItemViewModel
    {
        public string StripePublishableKey { get; set; }
        public string StripeToken { get; set; }
        public string StripeEmail { get; set; }
        public bool PaymentFormHidden { get; set; }
        public string PaymentFormHiddenCss
        {
            get
            {
                return PaymentFormHidden ? "hidden" : "";
            }
        }
        public List<SelectableItem> SelectableItems { get; set; }
        public ItemViewModel()
        {
            this.SelectableItems = new List<SelectableItem>();
        }
        public ICollection<Item> Items { get; set; }
        public Address Address { get; set; }
        public Address Address1 { get; set; }
        public Order Order { get; set; }
        public User User { get; set; }
        public PaymentDetail PaymentDetails { get; set; }
        public Pricing Prices { get; set; }
        [DisplayFormat(DataFormatString="{0:C}")]
        public decimal OrderTotal { get; set; }
        [Required]
        public decimal OrderQty { get; set; }
        [Required]
        public decimal OrderSize { get; set; }
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public IEnumerable<int> getSelectedIds()
        {
            var selectedIds = (from item in SelectableItems where item.Active select item.ItemID).ToList();
            return (selectedIds);
        }
        public string Message { get; set; }
        public Email Email { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ChicagoInABox.Models
{
    public class OrderMetadata
    {
        public int OrderID;
        public int UserID;
        public int ShipToID;
        public int BillToID;
        [Required]
        [Display(Name = "Order Quantity")]
        public int OrderQty;
        [Required]
        [Display(Name = "Order Size")]
        public int OrderSize;
        public System.DateTime OrderDate;
    }
    public class AddressMetadata
    {
        public int AddressID;
        [Required]
        [Display(Name = "Street")]
        public string Address1;
        [Required]
        [Display(Name = "City")]
        public string City;
        [Required]
        [Display(Name = "State")]
        public string State;
        [Required]
        [Display(Name = "Zip Code")]
        public int Zip;
        [Required]
        [Display(Name = "Country")]
        public string Country;
    }
    public class ItemMetadata
    {
        public int ItemID;
        [Required]
        [Display(Name = "Name")]
        public string ItemName;
        [Required]
        [Display(Name = "Description")]
        public string ItemDescription;
        public string ProductImage;
        public Nullable<decimal> Price;
        public Nullable<int> CatalogID;
        public string Caption;
    }
    public class OrderDetailMetadata
    {
        public int LineID;
        public int OrderID;
        public decimal Quantity;
        public int ItemID;
    }
    public class PaymentDetailMetadata
    {
        public int PaymentID;
        [Required]
        [Display(Name = "Payment Method")]
        public string PaymentMethod;
        [Required]
        [Display(Name = "Credit Card Number")]
        public int CardNumber;
        [Required]
        [Display(Name = "Security Code")]
        public int SecurityCode;
        public decimal PaymentAmt;
        [Required]
        [Display(Name = "Exipration Date")]
        public System.DateTime ExpirationDate;
        public int OrderID;
    }
    public class UserMetadata
    {
        public int UserID;
        [Required]
        [Display(Name = "First Name")]
        public string FirstName;
        [Required]
        [Display(Name = "Last Name")]
        public string LastName;
        [Required]
        [Display(Name = "Email Address")]
        public string Email;
        public string Phone;
        [Required]
        [Display(Name = "User Name")]
        public string UserName;
    }
    public class QuestionMetadata
    {
        public int Id;
        [Required]
        [Display(Name = "Email Address")]
        public string Email;
        [Required]
        [Display(Name = "Subject")]
        public string Subject;
        [Required]
        [Display(Name = "Message")]
        public string Message;
        public string Name;
    }
}
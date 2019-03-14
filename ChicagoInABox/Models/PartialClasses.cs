using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ChicagoInABox.Models
{
    [MetadataType(typeof(OrderMetadata))]
    public partial class Order
    {
    }
    [MetadataType(typeof(AddressMetadata))]
    public partial class Address
    {
    }
    [MetadataType(typeof(ItemMetadata))]
    public partial class Item
    {
    }
    [MetadataType(typeof(OrderDetailMetadata))]
    public partial class OrderDetail
    {
    }
    [MetadataType(typeof(PaymentDetailMetadata))]
    public partial class PaymentDetail
    {
    }
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
    }
    [MetadataType(typeof(QuestionMetadata))]
    public partial class Question
    {
    }
}
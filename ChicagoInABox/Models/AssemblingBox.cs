using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ChicagoInABox.Filters;

namespace ChicagoInABox.Models
{
    public partial class AssemblingBox
    {
        ChicagoInABoxEntities db = new ChicagoInABoxEntities();

        string AssemblingBoxId { get; set; }

        public const string CartSessionKey = "CartId";

        public void EmptyCart()
        {
            var cartItems = db.Carts.Where(cart => cart.CartId == AssemblingBoxId);
            foreach (var cartItem in cartItems)
            {
                db.Carts.Remove(cartItem);
            }
            // Save changes
            db.SaveChanges();
        }

        // We're using HttpContextBase to allow access to cookies.
        public static string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    int profileID = WebMatrix.WebData.WebSecurity.GetUserId(context.User.Identity.Name);
                    context.Session[CartSessionKey] = profileID.ToString();
                    //User userInfo = db.Users.Find(profileID);
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();

                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }

            return context.Session[CartSessionKey].ToString();
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var AssemblingBox = db.Carts.Where(c => c.CartId == AssemblingBoxId);

            

            foreach (Cart item in AssemblingBox)
            {
                item.CartId = userName;
            }
            db.SaveChanges();
        }
    }
}
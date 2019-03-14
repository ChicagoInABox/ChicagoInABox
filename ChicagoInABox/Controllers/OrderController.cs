using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChicagoInABox.Models;
using ChicagoInABox.Models.ViewModel;
using System.Web.Security;
using ChicagoInABox.Filters;
using System.Configuration;
using System.Diagnostics;
using Stripe;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ChicagoInABox.Controllers
{
    public class OrderController : Controller
    {
        private ChicagoInABoxEntities db = new ChicagoInABoxEntities();

        private ItemViewModel itemViewModel = new ItemViewModel();

        public const string CartSessionKey = "CartId";
        //
        // GET: /Order/

        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Address).Include(o => o.Address1).Include(o => o.User);
            return View(orders.ToList());
        }

        //
        // GET: /Order/Details/5

        public ActionResult Details(int id = 0)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // GET: /Order/Create
        [InitializeSimpleMembership]
        //[Authorize]
        public ActionResult Create()
        {
            if (TempData["model"] != null)
            {
                TempData.Keep("model");
                itemViewModel = (ItemViewModel)TempData["model"];
                string stripePublishableKey = ConfigurationManager.AppSettings["stripePublishableKey"];
                itemViewModel.StripePublishableKey = stripePublishableKey;
                itemViewModel.PaymentFormHidden = true;
                TempData["model"] = itemViewModel;
                return View(itemViewModel);
            }
            return RedirectToAction("AssembleBox", "Home");
        }

        //
        // POST: /Order/Create

        [HttpPost]
        [InitializeSimpleMembership]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(ItemViewModel model)
        {
            if (TempData["model"] != null)
            {
                TempData.Keep("model");
                itemViewModel = (ItemViewModel)TempData["model"];
                model.Items = itemViewModel.Items;
                model.OrderQty = itemViewModel.OrderQty;
                model.OrderSize = itemViewModel.OrderSize;
                model.OrderTotal = itemViewModel.OrderTotal;
                model.Message = itemViewModel.Message;
                itemViewModel.User = model.User;
                if (model.User.UserName == null)
                {
                    model.User = db.Users.Find(4);
                    model.User.FirstName = itemViewModel.User.FirstName;
                    model.User.LastName = itemViewModel.User.LastName;
                    model.User.Email = itemViewModel.User.Email;
                }
                else
                {
                    model.User = db.Users.Find(WebMatrix.WebData.WebSecurity.GetUserId(itemViewModel.User.UserName));
                }
            }

            this.ViewData.ModelState.Remove("CartId");
            this.ViewData.ModelState.Remove("User.UserID");
            this.ViewData.ModelState.Remove("User.UserName");
            if (ModelState.IsValid)
            {
                Address billingAddress = new Address()
                {

                    Address1 = model.Address1.Address1,
                    City = model.Address1.City,
                    State = model.Address1.State,
                    Zip = model.Address1.Zip,
                    Country = model.Address1.Country,
                };
                Address existingAddress = db.Addresses.FirstOrDefault(a => a.Address1.Equals(billingAddress.Address1) && a.City.Equals(billingAddress.City) && a.State.Equals(billingAddress.State) && a.Zip.Equals(billingAddress.Zip) && a.Country.Equals(billingAddress.Country));
                if (existingAddress != null)
                {
                    billingAddress = db.Addresses.Find(existingAddress.AddressID);
                }
                else
                {
                    db.Addresses.Add(billingAddress);
                }
                db.SaveChanges();
                Address shippingAddress = new Address()
                {
                    Address1 = model.Address.Address1,
                    City = model.Address.City,
                    State = model.Address.State,
                    Zip = model.Address.Zip,
                    Country = model.Address.Country,
                };
                existingAddress = null;
                existingAddress = db.Addresses.FirstOrDefault(a => a.Address1.Equals(shippingAddress.Address1) && a.City.Equals(shippingAddress.City) && a.State.Equals(shippingAddress.State) && a.Zip.Equals(shippingAddress.Zip) && a.Country.Equals(shippingAddress.Country));
                if (existingAddress != null)
                {
                    shippingAddress = db.Addresses.Find(existingAddress.AddressID);
                }
                else
                {
                    db.Addresses.Add(shippingAddress);
                }
                db.SaveChanges();

                Order order = new Order()
                {
                    BillToID = billingAddress.AddressID,
                    ShipToID = shippingAddress.AddressID,
                    UserID = model.User.UserID,
                    EmailAddress = model.StripeEmail,
                    OrderQty = Convert.ToInt32(model.OrderQty),
                    OrderSize = Convert.ToInt32(model.OrderSize),
                    OrderDate = DateTime.Now,
                    Notes = itemViewModel.Message
                };
                db.Orders.Add(order);
                db.SaveChanges();

                //var AssemblingBox = db.Carts.Where(c => c.CartId == model.CartId);

                foreach (var item in model.Items)
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        OrderID = order.OrderID,
                        ItemID = item.ItemID,
                        Quantity = 1,
                    };
                    db.OrderDetails.Add(orderDetail);
                }
                //PaymentDetail payment = new PaymentDetail()
                //{
                //    PaymentMethod = model.PaymentDetails.PaymentMethod,
                //    CardNumber = model.PaymentDetails.CardNumber,
                //    SecurityCode = model.PaymentDetails.SecurityCode,
                //    PaymentAmt = model.OrderTotal,
                //    ExpirationDate = model.PaymentDetails.ExpirationDate,
                //    OrderID = order.OrderID,
                //};
                //db.PaymentDetails.Add(payment);
                db.SaveChanges();

                model.PaymentFormHidden = false;
                var chargeOptions = new StripeChargeCreateOptions()
                {
                    Amount = (Convert.ToInt32(model.OrderTotal) * 100),
                    Currency = "usd",
                    Source = new StripeSourceOptions()
                    {
                        TokenId = model.StripeToken,
                        //Object = "card",
                        //Number = "4242424242424242",
                        //ExpirationYear = "2022",
                        //ExpirationMonth = "10",
                        //AddressCountry = "US",                // optional
                        //AddressLine1 = "24 Beef Flank St",    // optional
                        //AddressLine2 = "Apt 24",              // optional
                        //AddressCity = "Biggie Smalls",        // optional
                        //AddressState = "NC",                  // optional
                        //AddressZip = "27617",                 // optional
                        //Name = "Joe Meatballs",               // optional
                        //Cvc = "1223"                          // optional
                    },
                    Description = "Chicago In A Box!",
                    ReceiptEmail = model.StripeEmail,
                };

                var chargeService = new StripeChargeService();
                try
                {
                    var stripeCharge = chargeService.Create(chargeOptions);
                    //create db call to store the chargeID
                    //stripeCharge.Id
                }
                catch (StripeException stripeException)
                {
                    Debug.WriteLine(model.StripeEmail);
                    Debug.WriteLine(model.StripeToken);
                    ModelState.AddModelError(string.Empty, stripeException.Message);
                    return View(model);
                }
                TempData["model"] = model;
                return RedirectToAction("OrderConfirmation", "Order");
            }
            return View(model);
        }


        //
        //GET: /Order/OrderConfirmation
        public ActionResult OrderConfirmation(ItemViewModel itemViewModel)
        {
            if (TempData["model"] != null)
            {
                TempData.Keep("model");
                itemViewModel = (ItemViewModel)TempData["model"];
                //Need to loop through items and create a list.

                //Need to find the last order that was inserted into the db and set as orderid.
                //foreach (var item in itemViewModel.Items)
                //{

                //}
                //this.ViewData.ModelState.Remove("CartId");
                //this.ViewData.ModelState.Remove("User.UserID");
                //this.ViewData.ModelState.Remove("ItemViewModel");
                //this.ViewData.ModelState.Remove("User.UserName");
                //if (ModelState.IsValid)
                //{
                //Email orderConfirmationEmail = new Email()
                //{
                //    CreatedDate = DateTime.Now,
                //    EmailAddress = itemViewModel.User.Email,
                //    FirstName = itemViewModel.User.FirstName,
                //    LastName = itemViewModel.User.LastName,
                //    //OrderID = itemViewModel.Order.OrderID,
                //};
                //db.Emails.Add(itemViewModel.Email);
                //db.SaveChanges();

                //}

                ////var body = "<h3>Thank you for supporting Chicago In A Box!</h3><p>{0}, <br/>We appreciate your business. Someone will be reviewing your order shortly and get it out the door as soon as possible. <br/><br/>Please see the message your order details below: <br/>First Name: {1} <br/>Last Name: {2} <br/>Email: {3} <br/>Phone: {4} <br/>Message: {5}</p><Thank you for supporting Chicago In A Box. Please contact us at info@chicagoinabox with any questions!";
                //MailMessage message = new MailMessage();
                //message.To.Add(new MailAddress("bharney@chicagoinabox.com"));
                //// message.To.Add(new MailAddress(itemViewModel.User.Email));
                //message.From = new MailAddress("info@chicagoinabox.com");
                //message.Subject = "Thank you for your Order!";
                //message.Body = "testing123 test.";
                ////message.Body = string.Format(body, itemViewModel.User.LastName, itemViewModel.User.FirstName, itemViewModel.User.Email, itemViewModel.Order.OrderID, itemViewModel.Order.Notes);
                //message.IsBodyHtml = true;

                //using (var smtp = new SmtpClient())
                //{
                //    var credential = new NetworkCredential
                //    {
                //        UserName = "postmaster@chicagoinabox.com",  // replace with valid value
                //        Password = "************"  // replace with valid value
                //    };
                //    smtp.Credentials = credential;
                //    smtp.Host = "mail.chicagoinabox.com";
                //    smtp.Port = 587;
                //    smtp.EnableSsl = true;
                //    await smtp.SendMailAsync(message);
                //    return View(itemViewModel);
                //}
                //return RedirectToAction("Create", "Order");
                return View(itemViewModel);
            }
            return RedirectToAction("Create", "Order");
        }


        //
        // GET: /Order/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.BillToID = new SelectList(db.Addresses, "AddressID", "Address1", order.BillToID);
            ViewBag.ShipToID = new SelectList(db.Addresses, "AddressID", "Address1", order.ShipToID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", order.UserID);
            return View(order);
        }

        //
        // POST: /Order/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BillToID = new SelectList(db.Addresses, "AddressID", "Address1", order.BillToID);
            ViewBag.ShipToID = new SelectList(db.Addresses, "AddressID", "Address1", order.ShipToID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "FirstName", order.UserID);
            return View(order);
        }

        //
        // GET: /Order/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // POST: /Order/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
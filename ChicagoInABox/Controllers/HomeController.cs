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

namespace ChicagoInABox.Controllers
{
    public class HomeController : Controller
    {
        private ChicagoInABoxEntities db = new ChicagoInABoxEntities();
        
        private ItemViewModel itemViewModel = new ItemViewModel();

        public ActionResult Index()
        {
            ViewBag.Message = "Select your Chicago In A Box goodies, then we take care of the rest!";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Learn more about the people who make this all happen!";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Ask us anything!";

            return View();
        }

        public ActionResult FAQ()
        {
            ViewBag.Message = "Frequently Asked Questions";

            return View();
        }


        //
        // GET: /AssembleBox/
        //[InitializeSimpleMembership]

        public ActionResult AssembleBox()
        {
            ViewBag.Message = "Assemble a Chicago Box.";
            //ViewBag.OrderSize = 0;
            var model = new ItemViewModel();
            foreach (var item in db.Items)
            {
                var viewModelSelectionItem = new SelectableItem()
                {
                    ItemID = item.ItemID,
                    ItemName = item.ItemName,
                    ItemDescription = item.ItemDescription,
                    ProductImage = "N/A",
                    Price = Convert.ToDecimal(item.Price),
                    CatalogID = 1,
                    Active = false
                };
                model.SelectableItems.Add(viewModelSelectionItem);
            }
            //var cartId = AssemblingBox.GetCartId(this.HttpContext);
            //model.CartId = cartId;
            //var cartItems = db.Carts.Where(cart => cart.CartId == cartId);
            //foreach (var cartItem in cartItems)
            //{
            //    db.Carts.Remove(cartItem);
            //}
            model.OrderQty = 1;
            //// Save changes
            //db.SaveChanges();

            return View(model);
        }

        //
        // POST: Getting Selected Items and Creating Order Details

        [HttpPost]
        public ActionResult Create(ItemViewModel model)
        {
            if (TempData["model"] != null)
            {
                TempData.Keep("model");
                itemViewModel = (ItemViewModel)TempData["model"];
                model.OrderQty = itemViewModel.OrderQty;
                model.OrderSize = itemViewModel.OrderSize;
                model.OrderTotal = itemViewModel.OrderTotal;

            }
            if (ModelState.IsValid)
            {


                //Guid userGuid = (Guid)Membership.GetUser().ProviderUserKey;
                //OrderViewModel order = new OrderViewModel() 
                //{
                //    UserID = Convert.ToInt32(userGuid.ToString())
                //    ,ShipToID = 0
                //    ,BillToID = 0 
                //    ,OrderQty = model.Order.OrderQty 
                //    ,OrderSize = model.Order.OrderSize

                //};

                var selectedItem = new SelectableItem();
                foreach (var item in model.SelectableItems.ToList())
                {
                    if (!item.Active)
                    {
                        var selectionItem = new SelectableItem()
                        {
                            ItemID = item.ItemID,
                            ItemName = item.ItemName,
                            ItemDescription = item.ItemDescription,
                            ProductImage = "N/A",
                            Price = Convert.ToDecimal(item.Price),
                            CatalogID = 1,
                            Active = item.Active
                        };
                        model.SelectableItems.Remove(selectionItem); 
                    }
                }
                
                // get the ids of the items selected:
                var selectedIds = model.getSelectedIds();


                // Use the ids to retrieve the records for the selected
                // from the database:
                model.Items = (from selected in db.Items
                                    where selectedIds.Contains(selected.ItemID)
                                    select selected).ToList();
                //int profileID = WebMatrix.WebData.WebSecurity.GetUserId(User.Identity.Name);
                //User userInfo = db.Users.Find(profileID);
                //model.User = userInfo;

                //model.OrderViewModels.OrderQty = Convert.ToInt32(model.OrderQty);
                //model.OrderViewModels.OrderSize = Convert.ToInt32(model.OrderSize);
                //model.OrderSize = model.OrderSize;
/*
                var boxPrice = (from price in db.Pricings
                                where price.OrderSize == model.OrderSize
                                select price.Price).FirstOrDefault();

                var orderTotal = model.OrderQty * boxPrice;

                model.OrderTotal = orderTotal;

                //model.OrderViewModels.OrderID = model.Order.OrderID + 1;
                var cartId = AssemblingBox.GetCartId(this.HttpContext);
                model.CartId = cartId;
                // Set up our ViewModel

                foreach (var item in selectedItems)
                {
                    Cart shoppingCart = new Cart()
                    {
                        CartId = cartId,
                        OrderQty = model.Order.OrderQty,
                        OrderSize = model.Order.OrderSize,
                        ItemID = item.ItemID,
                        FirstName = "Anonymous shopper",
                        LastName = "Anonymous shopper",
                        DateCreated = DateTime.Now
                    };
                    db.Carts.Add(shoppingCart);
                }
                db.SaveChanges();
                */

                // Redirect somewhere meaningful (probably to somewhere showing 
                // the results of your processing):
                TempData["model"] = model;
                return RedirectToAction("Create", "Order");

            }
            /* 
                
                
                
             };
             db.Orders.Add();
             db.Orders.Add(model.Order)
             var viewModelOrder = new OrderViewModel()
                 ( model.OrderViewModels.OrderQty
                
             foreach (var item in db.Items)
             {
                 var viewModelSelectionItem = new SelectableItem()
                 {
                     ItemID = item.ItemID,
                     ItemName = item.ItemName,
                     ItemDescription = item.ItemDescription,
                     ProductImage = "N/A",
                     Price = Convert.ToDecimal(item.Price),
                     CatalogID = 1,
                     Active = false
                 };
                 model.SelectableItems.Add(viewModelSelectionItem);
             }
                
             db.Users.Add(model.User);
             db.Addresses.Add(model.Address);
                 if(model.Address1 != null)
                 {
                     db.Addresses.Add(model.Address1);
                 }
             db.PaymentDetails.Add(model.PaymentDetails);
             db.SaveChanges();
             return RedirectToAction("Index");
         }

             */

            return View(model);
        }
        /*
              itemViewModel.SelectableItems = (from i in db.Items select i).ToList();
            

               var items = new ItemViewModel {
                     Items = new List<Item> {
                         ItemID = (from i in db. select i),
                         ItemName = (from i in Items select id),
                         ItemDescription = (from i in Items select id),
                         ProductImage = (from i in Items select id),
                         Price = (from i in Items select id),
                         CatalogID = (from i in Items select id),
                         Caption = (from i in Items select id)
                     },


                     Active = "ActiveItemViewModel",
                 }

                  public ICollection<Item> Items { get; set; }
             public bool Active { get; set; }
             public Address Address { get; set; }
             public Address Address1 { get; set; }
             public Order Order { get; set; }
             public virtual ICollection<OrderDetail> OrderDetails { get; set; }
             public User User { get; set; }
             public virtual ICollection<PaymentDetail> PaymentDetails { get; set; }

                 foreach (Item itemObject in itemViewModel.Items)
                 {
                     var itemObj = from o in itemViewModel.Items select o;
                     foreach (var itemProperty in itemObj)
                     {
                         var itemProp = from p in itemObj select p;
                         return View(itemProp);
                     }
          
                     //
                     // GET: /PaymentDetail/Create

                     public ActionResult Create()
                     {
                         ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID");
                         return View();
                     }

                     //
                     // POST: /PaymentDetail/Create

                     [HttpPost]
                     [ValidateAntiForgeryToken]
                     public ActionResult Create(PaymentDetail paymentdetail)
                     {
                         if (ModelState.IsValid)
                         {
                             db.PaymentDetails.Add(paymentdetail);
                             db.SaveChanges();
                             return RedirectToAction("Index");
                         }

                         ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", paymentdetail.OrderID);
                         return View(paymentdetail);
                     }
             */
       [HttpPost]
        public ActionResult OrderTotals(ItemViewModel model)
        {
            model.OrderQty = 1;
            var orderSize = 1;
            var numberOfItems = model.OrderSize;
            var boxPrice = (from price in db.Pricings
                            where price.OrderSize == numberOfItems
                            select price.Price).FirstOrDefault();

            var orderTotal = orderSize * boxPrice;

            model.OrderTotal = orderTotal;
            TempData["model"] = model;
            //return PartialView(model);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        //
        // POST: /FAQ/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FAQ(Question question)
        {
            question.CreatedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("FAQ");
            }

            return View(question);
        }

        public ActionResult Catalog()
        {
            ViewBag.Message = "What's in the Box?";

            return View();
        }

        //
        // POST: /Catalog/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Catalog(Question question)
        {
            question.CreatedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Catalog");
            }

            return View(question);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}

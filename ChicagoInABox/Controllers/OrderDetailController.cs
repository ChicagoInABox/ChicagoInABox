using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChicagoInABox.Models;

namespace ChicagoInABox.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class OrderDetailController : Controller
    {
        private ChicagoInABoxEntities db = new ChicagoInABoxEntities();

        //
        // GET: /OrderDetail/

        public ActionResult Index()
        {
            var orderdetails = db.OrderDetails.Include(o => o.Item).Include(o => o.Order);
            return View(orderdetails.ToList());
        }

        //
        // GET: /OrderDetail/Details/5

        public ActionResult Details(int id = 0)
        {
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            if (orderdetail == null)
            {
                return HttpNotFound();
            }
            return View(orderdetail);
        }

        //
        // GET: /OrderDetail/Create

        public ActionResult Create()
        {
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName");
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID");
            return View();
        }

        //
        // POST: /OrderDetail/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderDetail orderdetail)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetails.Add(orderdetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName", orderdetail.ItemID);
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", orderdetail.OrderID);
            return View(orderdetail);
        }

        //
        // GET: /OrderDetail/Edit/5

        public ActionResult Edit(int id = 0)
        {
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            if (orderdetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName", orderdetail.ItemID);
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", orderdetail.OrderID);
            return View(orderdetail);
        }

        //
        // POST: /OrderDetail/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderDetail orderdetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderdetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemID = new SelectList(db.Items, "ItemID", "ItemName", orderdetail.ItemID);
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", orderdetail.OrderID);
            return View(orderdetail);
        }

        //
        // GET: /OrderDetail/Delete/5

        public ActionResult Delete(int id = 0)
        {
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            if (orderdetail == null)
            {
                return HttpNotFound();
            }
            return View(orderdetail);
        }

        //
        // POST: /OrderDetail/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(orderdetail);
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
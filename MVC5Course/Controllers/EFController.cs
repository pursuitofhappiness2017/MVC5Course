using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using System.Data.Entity.Validation;

namespace MVC5Course.Controllers
{
    public class EFController : Controller
    {
        FabricsEntities db = new FabricsEntities();

        // GET: EF
        public ActionResult Index()
        {
            var all = db.Product.AsQueryable();

            var data = all.Where(p => p.IsDeleted == false &&
                        p.Active == true &&
                        p.ProductName.Contains("Black"))
                        .OrderByDescending(p => p.ProductId);

            //要注意回傳的型別不同
            //var data1 = all.Where(p => p.ProductId == 1);
            //var data2 = all.FirstOrDefault(p => p.ProductId == 1);
            //var data3 = db.Product.Find(1);

            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                //以物件的方式來新增, 之後由ORM轉成SQL去做實際的操作
                db.Product.Add(product);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Edit(int id)
        {
            var item = db.Product.Find(id);

            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(int id, Product product)
        {
            if (ModelState.IsValid)
            {
                var item = db.Product.Find(id);
                item.ProductName = product.ProductName;
                item.Price = product.Price;
                item.Stock = product.Stock;
                item.Active = product.Active;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(product);
        }

        public ActionResult Delete(int id)
        {
            var product = db.Product.Find(id);

            //1.利用導覽屬性去移除有關聯的OrderLine
            //foreach (var item in product.OrderLine)
            //{
            //    db.OrderLine.Remove(item);
            //    //不可在此db.SaveChanges(), 
            //    //因為每次都算一筆交易, 之後會造成無法Roll Back;
            //}

            //2.利用導覽屬性去移除有關聯的OrderLine
            //db.OrderLine.RemoveRange(product.OrderLine);

            //db.Product.Remove(product);
            product.IsDeleted = true;

            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                throw ex;
            }

            

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var data = db.Product.Find(id);

            return View(data);
        }
    }
}
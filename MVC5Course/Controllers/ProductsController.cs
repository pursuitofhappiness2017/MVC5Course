using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using MVC5Course.Models.ViewModels;

namespace MVC5Course.Controllers
{
    public class ProductsController : BaseController
    {
        ProductRepository repo = RepositoryHelper.GetProductRepository();

        // GET: Products
        [OutputCache(Duration = 5, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult Index(bool Active = true)
        {
            var data = repo.GetProduct列表頁所有資料(Active, showAll: false);

            return View(data);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = repo.Get單筆資料ByProductId(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                repo.Add(product);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = repo.Get單筆資料ByProductId(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection form)
        {
            //[Bind(Include = "ProductId,ProductName,Price,Active,Stock")]
            //Product product

            var product = repo.Get單筆資料ByProductId(id);

            //延遲驗證
            if (TryUpdateModel(product,
                new string[] { "ProductId", "ProductName", "Price", "Active", "Stock" }))
            {
                repo.Update(product);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = repo.Get單筆資料ByProductId(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = repo.Get單筆資料ByProductId(id);

            repo.Delete(product);
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ListProducts(ProductListSearchVM searchModel)
        {
            var data = repo.All();

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(searchModel.q))
                {
                    data = data.Where(x => x.ProductName.Contains(searchModel.q));
                }
            }

            data = data.Where(p => p.Stock > searchModel.stock_min && p.Stock < searchModel.stock_max);

            ViewData.Model = data.Where(p => p.Active == true)
                                 .Select(p => new ProductLiteVM
                                 {
                                     ProductId = p.ProductId,
                                     ProductName = p.ProductName,
                                     Price = p.Price,
                                     Stock = p.Stock,
                                 })
                                 .Take(10);

            return View();
        }

        public ActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateProduct(ProductLiteVM data)
        {
            if (ModelState.IsValid)
            {
                TempData["CreatProductResult"] = "新增商品成功";

                //TODO: 儲存資料進資料庫
                return RedirectToAction("ListProducts");
            }

            //驗證失敗, 繼續顯示原本的表單
            return View();
        }

        [HttpPost]
        public ActionResult ListProducts(ProductListSearchVM searchCondition, ProductBatchUpdateVM[] items)
        {
            // TryUpdateModel(searchCondition, "searchCondition")
            if (ModelState.IsValid)
            {
                foreach (var item in items)
                {
                    var prod = db.Product.Find(item.ProductId);
                    prod.Price = item.Price;
                    prod.Stock = item.Stock;
                }

                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

                return RedirectToAction("ListProducts", searchCondition);
            }

            GetProductListBySearch(searchCondition);

            return View("ListProducts");
        }

        private void GetProductListBySearch(ProductListSearchVM searchCondition)
        {
            var data = repo.GetProduct列表頁所有資料(true);

            if (!String.IsNullOrEmpty(searchCondition.q))
            {
                data = data.Where(p => p.ProductName.Contains(searchCondition.q));
            }

            data = data.Where(p => p.Stock > searchCondition.stock_min && p.Stock < searchCondition.stock_max);

            ViewData.Model = data
                .Select(p => new ProductLiteVM()
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Stock = p.Stock
                });

            ViewBag.searchCondition = searchCondition;
        }
    }
}

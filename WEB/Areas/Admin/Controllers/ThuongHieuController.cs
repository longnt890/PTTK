using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB.Models;
using PagedList;
using PagedList.Mvc;
namespace WEB.Areas.Admin.Controllers
{
    public class ThuongHieuController : Controller
    {
        QLCH1Entities1 db = new QLCH1Entities1();
        // GET: Admin/ThuongHieu
        public ActionResult Index(int? page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            int size = 7;
            int pageNum = (page ?? 1);
            return View(db.THUONGHIEUx.OrderBy(n => n.MaTH).ToPagedList(pageNum, size));
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(THUONGHIEU dm, FormCollection f)
        {
            if (ModelState.IsValid)
            {
                dm.MoTa = f["mota"];
                dm.TenTH = f["thuonghieu"];
                db.THUONGHIEUx.Add(dm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var dm = db.THUONGHIEUx.SingleOrDefault(n => n.MaTH == id);
            if (dm == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dm);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id, FormCollection f)
        {
            var dm = db.THUONGHIEUx.SingleOrDefault(n => n.MaTH == id);
            if (dm == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var sp = db.SANPHAMs.Where(ct => ct.MaTH == id);
            if (sp.Count() > 0)
            {
                ViewBag.ThongBao = "Thương hiệu này có sản phẩm <br>" + "Nếu muốn xóa phải xóa hết sản phẩm trong thương hiệu";
                return View(dm);
            }

            db.THUONGHIEUx.Remove(dm);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var dm = db.THUONGHIEUx.SingleOrDefault(n => n.MaTH == id);
            if (dm == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dm);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            int idDM = int.Parse(f["id"]);
            var dm = db.THUONGHIEUx.SingleOrDefault(n => n.MaTH == idDM);

            if (ModelState.IsValid)
            {
                dm.TenTH = f["thuonghieu"];
                dm.MoTa = f["mota"];
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dm);
        }
    }
}
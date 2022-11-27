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
    public class DanhMucController : Controller
    {
        QLCH1Entities1 db = new QLCH1Entities1();
        // GET: Admin/DanhMuc
        public ActionResult Index(int? page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            int size = 7;
            int pageNum = (page ?? 1);
            return View(db.DANHMUCs.OrderBy(n => n.MaDM).ToPagedList(pageNum, size));
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
        public ActionResult Create(DANHMUC dm, FormCollection f)
        {
            if (ModelState.IsValid)
            {

                dm.TenDanhMuc = f["danhmuc"];
                db.DANHMUCs.Add(dm);
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
            var dm = db.DANHMUCs.SingleOrDefault(n => n.MaDM == id);
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
            var dm = db.DANHMUCs.SingleOrDefault(n => n.MaDM == id);
            if (dm == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var sp = db.SANPHAMs.Where(ct => ct.MaDM == id);
            if (sp.Count() > 0)
            {
                ViewBag.ThongBao = "Danh mục này có sản phẩm <br>" + "Nếu muốn xóa phải xóa hết sản phẩm trong danh mục";
                return View(dm);
            }

            db.DANHMUCs.Remove(dm);
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
            var dm = db.DANHMUCs.SingleOrDefault(n => n.MaDM == id);
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
            var dm = db.DANHMUCs.SingleOrDefault(n => n.MaDM == idDM);            

            if (ModelState.IsValid)
            {
                dm.TenDanhMuc = f["danhmuc"];
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dm);
        }
    }
}
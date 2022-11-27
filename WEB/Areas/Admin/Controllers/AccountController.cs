using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB.Models;
namespace WEB.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        QLCH1Entities1 db = new QLCH1Entities1();
        // GET: Admin/Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var sTenDN = f["UserName"];
            var sMatKhau = f["Password"];
            ADMIN ad = db.ADMINs.SingleOrDefault(n => n.TenDNAdmin == sTenDN && n.MatKhauAdmin == sMatKhau);
            if (ad != null)
            {
                Session["Admin"] = ad;
                return RedirectToAction("Profile", "Account");
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        public ActionResult Logout()
        {
            if(Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return PartialView("Logout");
        }
        public ActionResult DangXuat()
        {
            Session["Admin"] = null;
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public ActionResult Profile()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var ad = Session["Admin"];
            return View(ad);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Profile(FormCollection f)
        {
            ADMIN temp = (ADMIN)Session["Admin"];
            ADMIN ad = db.ADMINs.SingleOrDefault(n => n.MaAdmin == temp.MaAdmin);
            ad.DiaChiAdmin = f["DiaChi"];
            ad.DienThoaiAdmin = f["DT"];
            ad.EmailAdmin = f["email"];
            ad.HoTenAdmin = f["HoTen"];
            ad.MatKhauAdmin = f["pass"];
            ad.TenDNAdmin = f["TenDN"];

            db.SaveChanges();
            Session["Admin"] = null;
            return RedirectToAction("Login", "Account");
        }
    }
}
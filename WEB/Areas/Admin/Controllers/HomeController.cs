using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WEB.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        QLCH1Entities1 db = new QLCH1Entities1();
        // GET: Admin/Home
        public ActionResult Index(int ? page)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            int size = 7;
            int pageNum = (page ?? 1);
            return View(db.SANPHAMs.OrderBy(n => n.MaSP).ToPagedList(pageNum, size));
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.MaDM = new SelectList(db.DANHMUCs.ToList().OrderBy(n => n.TenDanhMuc), "MaDM", "TenDanhMuc");
            ViewBag.MaTH = new SelectList(db.THUONGHIEUx.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SANPHAM sp, FormCollection f, HttpPostedFileBase fFile)
        {
            ViewBag.MaDM = new SelectList(db.DANHMUCs.ToList().OrderBy(n => n.TenDanhMuc), "MaDM", "TenDanhMuc");
            ViewBag.MaTH = new SelectList(db.THUONGHIEUx.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH");

            if (fFile == null)
            {
                ViewBag.ThongBao = "Hay chon anh bia";
                ViewBag.TenSP = f["sTenSach"];
                ViewBag.Mota = f["sMoTa"];
                ViewBag.SoLuong = int.Parse(f["iSoLuong"]);
                ViewBag.GiaBan = decimal.Parse(f["mGiaBan"]);
                ViewBag.MaDM = new SelectList(db.DANHMUCs.ToList().OrderBy(n => n.TenDanhMuc), "MaDM", "TenDanhMuc", int.Parse(f["MaDM"]));
                ViewBag.MaTH = new SelectList(db.THUONGHIEUx.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH", int.Parse(f["MaTH"]));
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var sFileName = Path.GetFileName(fFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/img"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFile.SaveAs(path);
                    }
                    sp.TenSP = f["sTenSach"];
                    sp.MoTa = f["sMoTa"].Replace("<p>", "").Replace("</p>", "\n");
                    sp.HinhMinhHoa = sFileName;
                    sp.NGAYCAPNHAT = Convert.ToDateTime(f["dNgayCapNhat"]);
                    sp.SOLUONGBAN = int.Parse(f["iSoLuong"]);
                    sp.DonGia = decimal.Parse(f["mGiaBan"]);
                    sp.MaDM = int.Parse(f["MaDM"]);
                    sp.MaTH = int.Parse(f["MaTH"]);
                    db.SANPHAMs.Add(sp);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
            
        }
        public ActionResult Details(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id, FormCollection f)
        {
            var sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = db.CTDATHANGs.Where(ct => ct.MaSP == id);
            if (ctdh.Count() > 0)
            {
                ViewBag.ThongBao = "Sach nay dang co trong bang chi tiet dat hang <br>" + "Neu muon xoa thi phai xoa het sp nay trong chi tiet dat hang";
                return View(sp);
            }
            
            db.SANPHAMs.Remove(sp);
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
            var sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaDM = new SelectList(db.DANHMUCs.ToList().OrderBy(n => n.TenDanhMuc), "MaDM", "TenDanhMuc",sp.MaDM);
            ViewBag.MaTH = new SelectList(db.THUONGHIEUx.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH",sp.MaTH);
            return View(sp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            int idSP = int.Parse(f["iMaSP"]);
            var sp = db.SANPHAMs.SingleOrDefault(n => n.MaSP == idSP);
            ViewBag.MaDM = new SelectList(db.DANHMUCs.ToList().OrderBy(n => n.TenDanhMuc), "MaDM", "TenDanhMuc", sp.MaDM);
            ViewBag.MaTH = new SelectList(db.THUONGHIEUx.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH", sp.MaTH);

            if (ModelState.IsValid)
            {
                if (fFileUpload != null)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/img"), sFileName);

                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    sp.HinhMinhHoa = sFileName;
                }
                sp.TenSP = f["sTenSach"];
                sp.MoTa = f["sMoTa"].Replace("<p>", "").Replace("</p>", "\n");
                sp.NGAYCAPNHAT = Convert.ToDateTime(f["dNgayCapNhat"]);
                sp.SOLUONGBAN = int.Parse(f["iSoLuong"]);
                sp.DonGia = decimal.Parse(f["mGiaBan"]);
                sp.MaDM = int.Parse(f["MaDM"]);
                sp.MaTH = int.Parse(f["MaTH"]);
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sp);
        }
    }
}
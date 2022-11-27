using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB.Models;
namespace WEB.Controllers
{
    public class CartController : Controller
    {
        QLCH1Entities1 db = new QLCH1Entities1();
        // GET: Cart
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                //Khởi tạo giỏ hàng (giỏ hàng chưa tồn tại)
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int ms, string url)
        {
            //lấy giỏ hàng hiện tại
            List<GioHang> lstGioHang = LayGioHang();
            //kiểm tra nếu sản phẩm chưa có trong giỏ hàng thì thêm vào, nếu đã có thi tăng số lượng
            GioHang sp = lstGioHang.Find(n => n.MaSP == ms);
            if (sp == null)
            {
                sp = new GioHang(ms);
                lstGioHang.Add(sp);
            }
            else
            {
                sp.SoLuong++;
            }
            return Redirect(url);
        }
        //tính tổng số lượng
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.SoLuong);
            }
            return iTongSoLuong;
        }
        //Tính tổng tiền
        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.dThanhTien);
            }
            return dTongTien;
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        public ActionResult XoaSP(int MaSP)
        {
            List<GioHang> ls = LayGioHang();
            GioHang sp = ls.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp != null)
            {
                ls.RemoveAll(n => n.MaSP == MaSP);
                if (ls.Count == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang(int MaSP, FormCollection f)
        {
            List<GioHang> ls = LayGioHang();
            GioHang sp = ls.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp != null)
            {
                sp.SoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult LoginLogout()
        {
            return PartialView("LoginLogout");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return Redirect("~/User/Login");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<GioHang> ls = LayGioHang();
            ViewBag.TenKH = kh.HoTenKH;
            ViewBag.Email = kh.Email;
            ViewBag.DiaChi = kh.DiaChiKH;
            ViewBag.SDT = kh.DienThoaiKH;
            ViewBag.NgayDat = DateTime.Now;
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = (decimal)TongTien();
            return View(ls);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<GioHang> ls = LayGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDH = DateTime.Now;                     
            ddh.DaGiao = false;           
            db.DONDATHANGs.Add(ddh);
            db.SaveChanges();
            double tong = 0;
            foreach (var item in ls)
            {
                CTDATHANG ctdh = new CTDATHANG();
                ctdh.SoDH = ddh.SoDH;
                ctdh.MaSP = item.MaSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = (decimal)item.DonGia;
                db.CTDATHANGs.Add(ctdh);
                tong += (double)item.DonGia* item.SoLuong;
            }
            ddh.TriGia = (decimal)tong;
            db.DONDATHANGs.Add(ddh);
            db.SaveChanges();
            db.SaveChanges();
            Session["GioHang"] = null;

            return RedirectToAction("XacNhanDonHang", "Cart");
        }
        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }
}
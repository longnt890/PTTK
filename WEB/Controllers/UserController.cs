using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB.Models;
namespace WEB.Controllers
{
    public class UserController : Controller
    {
        QLCH1Entities1 db = new QLCH1Entities1();
        // GET: User
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {

            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TenDN == sTenDN && n.MatKhau == sMatKhau);
                if (kh != null)
                {

                    //ViewBag.ThongBao = "Chúc mừng bạn đã đăng nhập thành công";
                    Session["TaiKhoan"] = kh;                  
                    return RedirectToAction("Index", "Home");
                    
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection collection, KHACHHANG kh)
        {
            //gán giá trị người dùng nhập dữ liệu cho các biến
            var sHoTen = collection["HoTen"];
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["Matkhau"];
            var sMatKhauNhapLai = collection["MatkhauNL"];
            var sDiaChi = collection["Diachi"];
            var sEmail = collection["Email"];
            var sDienThoai = collection["DienThoai"];
            var dNgaySinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);
            if (db.KHACHHANGs.SingleOrDefault(n => n.TenDN == sTenDN) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
            }
            else if (db.KHACHHANGs.SingleOrDefault(n => n.Email == sEmail) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else
            {
                //gán giá trị cho đối tượng được tạo mới (kh)
                kh.HoTenKH = sHoTen;
                kh.TenDN = sTenDN;
                kh.MatKhau = sMatKhau;
                kh.Email = sEmail;
                kh.DiaChiKH = sDiaChi;
                kh.DienThoaiKH = sDienThoai;
                kh.NgaySinh = DateTime.Parse(dNgaySinh);
                db.KHACHHANGs.Add(kh);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return this.Register();
        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ThongTin()
        {
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB.Models;
using PagedList;
using PagedList.Mvc;

namespace WEB.Controllers
{
    public class ShopController : Controller
    {
        QLCH1Entities1 db = new QLCH1Entities1();
        // GET: Shop
        private List<SANPHAM> LaySP()
        {
            return db.SANPHAMs.OrderByDescending(a => a.NGAYCAPNHAT).ToList();
        }
        public ActionResult Shop(int ? page)
        {
            int pageNum = (page ?? 1);
            int size = 9;
            var sp = LaySP();
            return View(sp.ToPagedList(pageNum,size));
        }
        public ActionResult DanhMuc()
        {
            var danhmuc = from cd in db.DANHMUCs select cd;
            return PartialView(danhmuc);
        }
        public ActionResult SPTheoDM(int id, int ? page)
        {
            ViewBag.MaDM = id;
            int size = 6;
            int pageNum = (page ?? 1);
            var sp = from s in db.SANPHAMs where s.MaDM == id orderby s.MaDM ascending select s;

            var tem =  from s in db.DANHMUCs where s.MaDM == id select s;
            ViewBag.Ten = tem.Take(1);
            return View(sp.ToPagedList(pageNum,size));
        }
        public ActionResult ThuongHieu()
        {
            var c = from cd in db.THUONGHIEUx select cd;
            return PartialView(c);
        }
        public ActionResult SPTheoTH(int id, int? page)
        {
            ViewBag.MaTH = id;
            int size = 6;
            int pageNum = (page ?? 1);
            var sp = from s in db.SANPHAMs where s.MaTH == id orderby s.MaTH ascending select s;

            var tem = from s in db.THUONGHIEUx where s.MaTH == id select s;
            ViewBag.Ten = tem.Take(1);
            return View(sp.ToPagedList(pageNum, size));
        }
        public ActionResult CTSP(int id)
        {
            var sp = from s in db.SANPHAMs 
                     join b in db.DANHMUCs
                     on s.MaDM equals b.MaDM
                     join c in db.THUONGHIEUx                    
                     on s.MaTH equals c.MaTH
                     where s.MaSP == id
                     select new ProductDetail()
                     {
                         DonGia = s.DonGia,
                         HinhMinhHoa = s.HinhMinhHoa,
                         MaSP = s.MaSP,
                         MoTa = s.MoTa,
                         NGAYCAPNHAT = s.NGAYCAPNHAT,
                         SOLUONGBAN = s.SOLUONGBAN,
                         TenDanhMuc = b.TenDanhMuc,
                         TenTH = c.TenTH,
                         TenSP = s.TenSP,
                         MaDM = s.MaDM,
                         MaTH = s.MaTH
                     };
            
            return View(sp.Single());
        }
        public ActionResult RelatedProduct()
        {
            var sp = from s in db.SANPHAMs.OrderByDescending(a => a.NGAYCAPNHAT).Take(4) select s;
            
            return View(sp);
        }

    }
}
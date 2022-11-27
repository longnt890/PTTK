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
    public class DonHangController : Controller
    {
        QLCH1Entities1 db = new QLCH1Entities1();
        // GET: Admin/DonHang
        public ActionResult Index(int? page)
        {
            int size = 7;
            int pageNum = (page ?? 1);

            return View(db.DONDATHANGs.OrderBy(n => n.SoDH).ToPagedList(pageNum, size));
        }
    }
}
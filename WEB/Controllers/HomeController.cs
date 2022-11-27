using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB.Models;
namespace WEB.Controllers
{
    public class HomeController : Controller
    {
        QLCH1Entities1 db = new QLCH1Entities1();
        private List<SANPHAM> LaySP(int count)
        {
            return db.SANPHAMs.OrderByDescending(a => a.SOLUONGBAN).Take(count).ToList();
        }
        public ActionResult Index()
        {
            var sp = LaySP(8);
            return View(sp);
            
        }
          
    }
}
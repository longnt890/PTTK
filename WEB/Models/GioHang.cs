using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WEB.Models;
namespace WEB.Models
{
    public class GioHang
    {
        QLCH1Entities1 db = new QLCH1Entities1();
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public double DonGia { get; set; }
        
        public int SoLuong { get; set; }
        public string HinhMinhHoa { get; set; }
        public double dThanhTien
        {
            get { return SoLuong * DonGia; }
        }
        public GioHang(int msp)
        {
            MaSP = msp;
            SANPHAM s = db.SANPHAMs.Single(n => n.MaSP == MaSP);
            TenSP = s.TenSP;
            DonGia = double.Parse(s.DonGia.ToString());
            HinhMinhHoa = s.HinhMinhHoa;
            SoLuong = 1;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB.Models
{
    public class ProductDetail
    {
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public Nullable<decimal> DonGia { get; set; }
        public string MoTa { get; set; }
        public string HinhMinhHoa { get; set; }
        public Nullable<int> MaTH { get; set; }
        public string TenTH { get; set; }
        public Nullable<int> MaDM { get; set; }
        public string TenDanhMuc { get; set; }
        public Nullable<int> MaKM { get; set; }
        public Nullable<System.DateTime> NGAYCAPNHAT { get; set; }
        public Nullable<int> SOLUONGBAN { get; set; }
    }
}
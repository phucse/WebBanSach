using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanSach.Models
{
    public class Cart
    {
        dbBookDataContext db = new dbBookDataContext();

        public int idSach { set; get; }
        public string sTenSach  { set; get; }
        public string sAnhBia { set; get; }
        public Double dDonGia { set; get; }
        public int iSoLuong { set; get; }
        public Double dThanhTien
        {
            get { return iSoLuong * dDonGia; }  
        }

        public Cart(int MaSach)
        {
            idSach = MaSach;
            SACH sach = db.SACHes.SingleOrDefault(n => n.MaSach == idSach);
            sTenSach = sach.TenSach;
            sAnhBia = sach.AnhBia;
            dDonGia = double.Parse(sach.GiaBan.ToString());
            iSoLuong = 1;
        }
    }
}
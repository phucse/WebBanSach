using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanSach.Models;

namespace WebBanSach.ViewModels
{
    public class DonHangNguoiBanViewModel
    {
        public int idNguoiBan { get; set; }
        public DONHANG donhang { get; set; }
        public SACH sach { get; set; }
    }
}
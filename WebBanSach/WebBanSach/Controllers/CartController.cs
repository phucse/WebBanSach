using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;
using WebBanSach.ViewModels;

namespace WebBanSach.Controllers
{
    public class CartController : Controller
    {
        dbBookDataContext db = new dbBookDataContext();
        
        public List<Cart> GetCart()
        {
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if(lstCart == null)
            {
                lstCart = new List<Cart>();
                Session["Cart"] = lstCart;
            }
            return lstCart;
        }

        public ActionResult AddCart(int iMaSach, string strURL)
        {
            List<Cart> lstCart = GetCart();

            Cart lstsach = lstCart.Find(n => n.idSach == iMaSach);

            if(lstsach == null)
            {
                lstsach = new Cart(iMaSach);
                lstCart.Add(lstsach);
                return Redirect(strURL);
            }
            else
            {
                lstsach.iSoLuong++;
                return Redirect(strURL);
            }
        }

        private int TotalAmount()
        {
            int amount = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if(lstCart!=null)
            {
                amount = lstCart.Sum(n => n.iSoLuong);
            }
            return amount;
        }

        private double TotalPrice()
        {
            double price = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if(lstCart!=null)
            {
                price = lstCart.Sum(n => n.dThanhTien);
            }
            return price;
        }

        public ActionResult ChiTietGioHang()
        {
            List<Cart> lstCart = GetCart();
            if(lstCart.Count==0)
            {
                return RedirectToAction("Index","Book");
            }
            ViewBag.TotalAmount = TotalAmount();
            ViewBag.TotalPrice = TotalPrice();
            return View(lstCart);
        }

        public ActionResult Cart()
        {
            ViewBag.TotalAmount = TotalAmount();
            return PartialView();
        }

        public ActionResult DeleteCart(int iMaSach)
        {
            List<Cart> lstCart = GetCart();

            Cart sach = lstCart.SingleOrDefault(n => n.idSach == iMaSach);

            if(sach!=null)
            {
                lstCart.RemoveAll(n => n.idSach == iMaSach);
                return RedirectToAction("ChiTietGioHang");
            }
            if(lstCart.Count==0)
            {
                return RedirectToAction("Index","Book");
            }
            return RedirectToAction("ChiTietGioHang");
        }

        public ActionResult UpdateCart(int iMaSach,FormCollection collection)
        {
            List<Cart> lstCart = GetCart();

            Cart sach = lstCart.SingleOrDefault(n => n.idSach == iMaSach);
            SACH book = db.SACHes.SingleOrDefault(s => s.MaSach==iMaSach);
            if (sach != null)
            {
                int sl = int.Parse(collection["txtSoluong"].ToString());
                if(book.Soluong > 0 && book.Soluong >= sl)
                {
                    sach.iSoLuong = sl;
                }               
            }
            return RedirectToAction("ChiTietGioHang");
        }

        public ActionResult DeleteAll()
        {
            List<Cart> lstCart = GetCart();
            lstCart.Clear();
            return RedirectToAction("Index","Book");
        }

        [HttpGet]
        public ActionResult Order()
        {
            if(Session["taikhoan"] == null)
            {
                return RedirectToAction("DangNhap","User");
            }
            if(Session["Cart"] == null)
            {
                return RedirectToAction("Index","Book");
            }
            List<Cart> lstcart = GetCart();
            ViewBag.TotalAmount = TotalAmount();
            ViewBag.TotalPrice = TotalPrice();

            return View(lstcart);
        }

        [HttpPost]
        public ActionResult Order(FormCollection collection)
        {
            //int masach;
            //DONHANG dh = new DONHANG();
            //DIACHIGIAO dcg = new DIACHIGIAO();
            List<DonHangNguoiBanViewModel> donhangs = new List<DonHangNguoiBanViewModel>();
            KHACHHANG kh = (KHACHHANG)Session["acc"];
            List<Cart> cart = GetCart();

            //dh.MaKH = kh.MaKH;
            //dh.NgayDat = DateTime.Now;
            //var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["ngaygiao"]);
            //dh.NgayGiao = DateTime.Parse(ngaygiao);
            //dh.GiaoHang = false;
            //dh.ThanhToan = false;
            //db.DONHANGs.InsertOnSubmit(dh);
            ////db.SubmitChanges();

            //dcg.MaDH = dh.MaDH;
            //dcg.HoTen = collection["hoten"];
            //dcg.DiaChi = collection["diachi"];
            //dcg.DienThoai = collection["dienthoai"];
            //dcg.GhiChu = collection["ghichu"];
            //db.DIACHIGIAOs.InsertOnSubmit(dcg);
            //db.SubmitChanges();

            //foreach (var item in cart)
            //{                            
            //    CTDH ctdh = new CTDH();
            //    ctdh.MaDH = dh.MaDH;
            //    ctdh.MaSach = item.idSach;
            //    ctdh.SoLuong = item.iSoLuong;
            //    ctdh.DonGia = (decimal)item.dDonGia;
            //    db.CTDHs.InsertOnSubmit(ctdh);
            //    SACH book = db.SACHes.SingleOrDefault(s => s.MaSach == item.idSach);
            //    book.Soluong = book.Soluong - item.iSoLuong;
            //}
            //db.SubmitChanges();

            foreach(var item in cart)
            {
                //DonHangNguoiBanViewModel dh_nb = new DonHangNguoiBanViewModel();
                //dh_nb.idNguoiBan = db.SACHes.Single(sach => sach.MaSach == item.idSach).NGUOIBAN.MaNB;
                //dh_nb.donhang = new DONHANG();

                //dh_nb.donhang.CTDHs.Add(new CTDH
                //{
                //    MaSach = item.idSach,
                //    DonGia = (decimal)item.dDonGia,
                //    SoLuong = item.iSoLuong
                //});
                bool isAdded = false;
                int maNB = db.SACHes.Single(s => s.MaSach == item.idSach).NGUOIBAN.MaNB;

                foreach (var donhang in donhangs)
                {
                    if(donhang.idNguoiBan == maNB)
                    {
                        isAdded = true;
                        donhang.donhang.CTDHs.Add(new CTDH
                        {
                            MaSach = item.idSach,
                            DonGia = (decimal)item.dDonGia,
                            SoLuong = item.iSoLuong
                        });
                    }
                }
                if(!isAdded)
                {
                    DonHangNguoiBanViewModel dh_nb = new DonHangNguoiBanViewModel();
                    dh_nb.idNguoiBan = maNB;
                    dh_nb.donhang = new DONHANG();
                    dh_nb.donhang.CTDHs.Add(new CTDH
                    {
                        MaSach = item.idSach,
                        DonGia = (decimal)item.dDonGia,
                        SoLuong = item.iSoLuong
                    });                    

                    var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["ngaygiao"]);
                    dh_nb.donhang = new DONHANG
                    {
                        MaKH = kh.MaKH,
                        NgayDat = DateTime.Now,
                        NgayGiao = DateTime.Parse(ngaygiao),
                        GiaoHang = false,
                        ThanhToan = false
                    };

                    dh_nb.donhang.DIACHIGIAO = new DIACHIGIAO
                    {
                        MaDH = dh_nb.donhang.MaDH,
                        HoTen = collection["hoten"],
                        DiaChi = collection["diachi"],
                        DienThoai = collection["dienthoai"],
                        GhiChu = collection["ghichu"],
                    };
                    donhangs.Add(dh_nb);
                }                

                db.DONHANGs.InsertAllOnSubmit(donhangs.Select(c => c.donhang));
            }
            db.SubmitChanges();

            
            db.SubmitChanges();
            Session["cart"] = null;
            return RedirectToAction("ConfirmOrder", "Cart");


        }

        public ActionResult ConfirmOrder()
        {
            return View();
        }
    }
}
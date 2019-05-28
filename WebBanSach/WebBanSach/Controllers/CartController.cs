using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;

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

            if (sach != null)
            {
                sach.iSoLuong = int.Parse(collection["txtSoluong"].ToString());
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
            DONHANG dh = new DONHANG();
            KHACHHANG kh = (KHACHHANG)Session["acc"];
            List<Cart> cart = GetCart();
            dh.MaKH = kh.MaKH;
            dh.NgayDat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["ngaygiao"]);
            dh.NgayGiao = DateTime.Parse(ngaygiao);
            dh.GiaoHang = false;
            dh.ThanhToan = false;
            db.DONHANGs.InsertOnSubmit(dh);
            db.SubmitChanges();

            foreach(var item in cart)
            {
                CTDH ctdh = new CTDH();
                ctdh.MaDH = dh.MaDH;
                ctdh.MaSach = item.idSach;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dDonGia;
                db.CTDHs.InsertOnSubmit(ctdh);
            }
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class UserController : Controller
    {
        dbBookDataContext db = new dbBookDataContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["hoten"];
            var taikhoan = collection["taikhoan"];
            var matkhau = collection["matkhau"];
            var nhaplai = collection["nhaplaimatkhau"];
            var email = collection["email"];
            var diachi = collection["diachi"];
            var dienthoai = collection["dienthoai"];
            var ngaysinh = String.Format("{0:MM-dd-yyyy}", collection["ngaysinh"]);
            DateTime ns = DateTime.Parse(ngaysinh);
            DateTime nowdate = DateTime.Now;

            List<KHACHHANG> tk = db.KHACHHANGs.Where(t => t.TaiKhoan.Contains(taikhoan)).ToList();
            List<KHACHHANG> em = db.KHACHHANGs.Where(e => e.Email.Contains(email)).ToList();

            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên không được để trống";
            }
            else if (String.IsNullOrWhiteSpace(taikhoan))
            {
                ViewData["Loi2"] = "Tài khoản không được để trống và không có khoản trống";
            }
            else if (tk.Count != 0)
            {
                ViewData["loi2"] = "Tài khoản đã tồn tại";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Mật khẩu không được để trống";
            }
            else if (matkhau.Length < 6)
            {
                ViewData["Loi3"] = "Mật khẩu phải lớn hơn 5 kí tự";
            }
            else if (String.IsNullOrEmpty(nhaplai))
            {
                ViewData["Loi4"] = "Hãy nhập lại mật khẩu";
            }
            else if (String.Compare(matkhau, nhaplai, false) != 0)
            {
                ViewData["Loi4"] = "Mật khẩu nhập lại không đúng";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được để trống";
            }
            else if (email.IndexOf("@") == -1)
            {
                ViewData["Loi5"] = "Không đúng định dạng mail";
            }
            else if (em.Count != 0)
            {
                ViewData["loi5"] = "Email đã tồn tại";
            }
            else if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loidc"] = "Địa chỉ không được để trống";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Điện thoại không được để trống";
            }
            else if (dienthoai.Length < 9 || dienthoai.Length > 11)
            {
                ViewData["Loi6"] = "Sai định dạng";
            }
            else if (ns > nowdate)
            {
                ViewData["Loi7"] = "Ngày không phù hợp";
            }
            else
            {
                kh.HoTen = hoten;
                kh.TaiKhoan = taikhoan;
                kh.MatKhau = matkhau;
                kh.Email = email;
                kh.DienThoai = dienthoai;
                kh.DiaChi = diachi;
                kh.Email = email;
                kh.DienThoai = dienthoai;
                kh.NgaySinh = ns;

                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("DangNhap");

            }

            return this.DangKy();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            if (Session["acc"] != null)
            {
                return RedirectToAction("Index", "Book");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            
                var taikhoan = collection["taikhoan"];
                var matkhau = collection["matkhau"];

                if (String.IsNullOrEmpty(taikhoan))
                {
                    ViewData["Loi1"] = "Tài khoản không được để trống";
                }
                else if (String.IsNullOrEmpty(matkhau))
                {
                    ViewData["Loi2"] = "Mật khẩu không được để trống";
                }
                else
                {
                    KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == taikhoan && n.MatKhau == matkhau);
                    if (kh != null)
                    {
                        Session["acc"] = kh;
                        Session["taikhoan"] = kh.TaiKhoan;
                        Session["id"] = kh.MaKH;
                        return RedirectToAction("Index", "Book");
                    }
                    else
                    {
                        ViewBag.Thongbao = "Sai tài khoản hoặc mật khẩu";
                    }
                }

                return View();
            
            
        }

        public ActionResult NguoiDung()
        {
            return PartialView();
        }

        public ActionResult DangXuat()
        {
            Session["acc"] = null;
            Session["taikhoan"] = null;
            Session["id"] = null;
            return RedirectToAction("Index", "Book");
        }

        [HttpGet]
        public ActionResult TTCN(int id)
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("DangNhap", "User");
            }
            else
            {
                var us = from u in db.KHACHHANGs
                         where u.MaKH == id
                         select u;
                return View(us.Single());
            }
        }

        [HttpPost]
        public ActionResult TTCN(int id, FormCollection collection)
        {

            int idkh = (int)Session["id"];
            var hoten = collection["hoten"];
            var email = collection["email"];
            var diachi = collection["diachi"];
            var dienthoai = collection["dienthoai"];
            var ngaysinh = String.Format("{0:MM-dd-yyyy}", collection["ngaysinh"]);

            List<KHACHHANG> em = db.KHACHHANGs.Where(e => e.Email.Contains(email)).ToList();

            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["loiHT"] = "Họ tên không được để trống";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["loiE"] = "Email không được để trống";
            }
            else if (em.Count != 0)
            {
                ViewData["loiE"] = "Email đã tồn tại";
            }
            else if (String.IsNullOrEmpty(diachi))
            {
                ViewData["loiDC"] = "Địa chỉ không được để trống";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["loiDT"] = "Điện thoại không được để trống";
            }
            else if (dienthoai.Length < 9 || dienthoai.Length > 11)
            {
                ViewData["loiDT"] = "Sai định dạng";
            }
            else
            {
                try
                {
                    DateTime ns = DateTime.Parse(ngaysinh);
                    DateTime nowdate = DateTime.Now;

                    if (ns > nowdate)
                    {
                        ViewData["loiNS"] = "Ngày không phù hợp";
                    }
                    var kh = db.KHACHHANGs.SingleOrDefault(k => k.MaKH == idkh);
                    kh.HoTen = hoten;
                    kh.Email = email;
                    kh.DienThoai = dienthoai;
                    kh.DiaChi = diachi;
                    kh.Email = email;
                    kh.DienThoai = dienthoai;
                    kh.NgaySinh = ns;
                    db.SubmitChanges();
                    ViewData["mess"] = "Cập nhật thành công";
                }
                catch (Exception ex)
                {
                    ViewData["mess"] = "Cập nhật thất bại";
                    return this.TTCN(idkh);
                }
            }
            
            return this.TTCN(idkh);

        }

        public ActionResult XemDonHang()
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("DangNhap", "User");
            }
            else
            {
                int idUser = (int)Session["id"];
                List<DONHANG> donhang = db.DONHANGs.OrderByDescending(d => d.NgayDat).Where(d => d.MaKH == idUser).ToList();
                return View(donhang);
            }
        }

        public ActionResult CTDH(int id)
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("DangNhap", "User");
            }
            else
            {
                List<CTDH> ctdh = db.CTDHs.Where(c => c.MaDH == id).ToList();
                return View(ctdh);
            }
        }

    }
}
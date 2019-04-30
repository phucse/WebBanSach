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
            else if (String.Compare(matkhau,nhaplai,false)!=0)
            {
                ViewData["Loi4"] = "Mật khẩu nhập lại không đúng";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được để trống";
            }
            else if (email.IndexOf("@")==-1)
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
            else if (ns>nowdate)
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
            return View();
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
                if(kh != null)
                {
                    Session["taikhoan"] = kh.TaiKhoan;
                    Session["id"] = kh.MaKH;
                    return RedirectToAction("Index","Book");
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
            Session["taikhoan"] = null;
            return RedirectToAction("Index","Book");
        }

        public ActionResult TTCN(int id)
        {
            var us = from u in db.KHACHHANGs
                     where u.MaKH == id
                     select u;
            return View(us.Single());
        }
    }
}
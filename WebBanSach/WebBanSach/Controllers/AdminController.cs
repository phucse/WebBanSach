using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class AdminController : Controller
    {
        dbBookDataContext db = new dbBookDataContext();

        // GET: Admin
        public ActionResult IndexAD()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
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
        public ActionResult Login(FormCollection collection)
        {
            var un = collection["username"];
            var pass = collection["pass"];
            if (String.IsNullOrEmpty(un))
            {
                ViewData["Er1"] = "Hãy nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(pass))
            {
                ViewData["Er2"] = "Hãy nhập mật khẩu";
            }
            else
            {
                ADMIN ad = db.ADMINs.SingleOrDefault(n => n.MaAdmin == un && n.Password == pass);
                if (ad != null)
                {
                    Session["admin"] = ad.TenAd;
                    return RedirectToAction("IndexAD", "Admin");
                }
                else
                    ViewBag.Mess = "Username hoặc Password không đúng";
            }
            return View();
        }

        public ActionResult Sach()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                return View(db.SACHes.ToList());
            }
        }

        public ActionResult BookDetail(int id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                SACH sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
                if (sach == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(sach);
            }
        }

        [HttpGet]
        public ActionResult DeleteBook(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpPost, ActionName("DeleteBook")]
        public ActionResult Confirm(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            db.SACHes.DeleteOnSubmit(sach);
            db.SubmitChanges();
            return RedirectToAction("Sach");
        }

        public ActionResult TheLoaiSach()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                return View(db.THELOAIs.ToList());
            }
        }
        [HttpGet]
        public ActionResult CreateTL()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateTL(FormCollection collection, THELOAI tl)
        {
            var theloai = collection["TenTL"];
            List<THELOAI> t = db.THELOAIs.Where(n => n.TenTL.Contains(theloai)).ToList();
            if(String.IsNullOrEmpty(theloai))
            {
                ViewData["ErTL"] = "Hãy nhập tên thể loại";
            }
            else if(t.Count!=0)
            {
                ViewData["ErTL"] = "Tên thể loại đã tồn tại";
            }
            else
            {
                tl.TenTL = theloai;
                db.THELOAIs.InsertOnSubmit(tl);
                db.SubmitChanges();
                return RedirectToAction("TheLoaiSach");
            }
            return this.CreateTL();
        }
        [HttpGet]
        public ActionResult DeleteTL(int id)
        {
            THELOAI tl = db.THELOAIs.SingleOrDefault(n => n.MaTL == id);
            return View(tl);
        }
        [HttpPost,ActionName("DeleteTL")]
        public ActionResult XoaTL(int id)
        {
            THELOAI tl = db.THELOAIs.SingleOrDefault(n => n.MaTL == id);
            db.THELOAIs.DeleteOnSubmit(tl);
            db.SubmitChanges();
            
            return RedirectToAction("TheLoaiSach");
        }

        public ActionResult NXBSach()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                return View(db.NXBs.ToList());
            }
        }
        [HttpGet]
        public ActionResult CreateNXB()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateNXB(FormCollection collection, NXB nxb)
        {
            var name = collection["TenNXB"];
            List<NXB> n = db.NXBs.Where(t => t.TenNXB.Contains(name)).ToList();
            if (String.IsNullOrEmpty(name))
            {
                ViewData["ErNXB"] = "Hãy nhập tên nhà xuất bản";
            }
            else if (n.Count != 0)
            {
                ViewData["ErNXB"] = "Tên nhà xuất bản đã tồn tại";
            }
            else
            {
                nxb.TenNXB = name;
                db.NXBs.InsertOnSubmit(nxb);
                db.SubmitChanges();
                return RedirectToAction("NXBSach");
            }
            return this.CreateNXB();
        }
        [HttpGet]
        public ActionResult DeleteNXB(int id)
        {
            NXB nxb = db.NXBs.SingleOrDefault(n => n.MaNXB == id);
            return View(nxb);
        }
        [HttpPost,ActionName("DeleteNXB")]
        public ActionResult XoaNXB(int id)
        {
            NXB nxb = db.NXBs.SingleOrDefault(n => n.MaNXB == id);
            db.NXBs.DeleteOnSubmit(nxb);
            db.SubmitChanges();

            return RedirectToAction("NXBSach");
        }

        public ActionResult DSDonHang()
        {
            List<DONHANG> dh = db.DONHANGs.OrderBy(d => d.NgayGiao).ToList();
            return View(dh);
        }

        public ActionResult OrderDetail(int id)
        {
            List<CTDH> ctdh = db.CTDHs.Where(c => c.MaDH == id).ToList();
            return View(ctdh);
        }

        public ActionResult QLNguoiDung()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                List<KHACHHANG> kh = db.KHACHHANGs.OrderBy(k => k.MaKH).ToList();
                return View(kh);
            }
            
        }

        public ActionResult CTKH(int id)
        {
            var kh = from k in db.KHACHHANGs where k.MaKH == id select k;
            return View(kh.Single());
        }
    }
}
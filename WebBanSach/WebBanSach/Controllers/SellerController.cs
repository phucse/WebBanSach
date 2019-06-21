using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class SellerController : Controller
    {
        // GET: Seller
        dbBookDataContext db = new dbBookDataContext();

        // GET: Admin
        public ActionResult IndexS()
        {
            if(Session["seller"]==null)
            {
                return RedirectToAction("LoginS","Seller");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult LoginS()
        {
            if (Session["seller"] != null)
            {
                return RedirectToAction("IndexS", "Seller");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult LoginS(FormCollection collection)
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
                    NGUOIBAN nb = db.NGUOIBANs.SingleOrDefault(n => n.TKNB == un && n.MKNB == pass);
                    if (nb != null)
                    {
                        Session["seller"] = nb.MaNB;
                        return RedirectToAction("IndexS", "Seller");
                    }
                    else
                        ViewBag.Mess = "Username hoặc Password không đúng";
                }
                return View();
            
        }

        public ActionResult QLSach()
        {
            if (Session["seller"] == null)
            {
                return RedirectToAction("LoginS", "Seller");
            }
            else
            {
                int id = (int)Session["seller"];
                List<SACH> s = db.SACHes.Where(n => n.MaNB == id).ToList();
                return View(s);
            }
                
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["seller"] == null)
            {
                return RedirectToAction("LoginS", "Seller");
            }
            else
            {
                ViewBag.MaNXB = new SelectList(db.NXBs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
                ViewBag.MaTL = new SelectList(db.THELOAIs.ToList().OrderBy(n => n.TenTL), "MaTL", "TenTL");
                return View();
            }
                
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SACH sach, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaNXB = new SelectList(db.NXBs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            ViewBag.MaTL = new SelectList(db.THELOAIs.ToList().OrderBy(n => n.TenTL), "MaTL", "TenTL");

            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if(ModelState.IsValid)
                {
                    var filename = DateTime.Now.ToString("ddMMyyyy_hhmmss")+Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images"), filename);

                    //if (System.IO.File.Exists(path))
                    //{
                    //    ViewBag.ThongBao = "File đã tồn tại";
                    //}
                    //else
                    //{

                    //}
                    fileUpload.SaveAs(path);

                    sach.AnhBia = filename;
                    sach.NgayUpdate = DateTime.Now;
                    sach.MaNB = (int)Session["seller"];

                    db.SACHes.InsertOnSubmit(sach);
                    db.SubmitChanges();
                }
                return RedirectToAction("QLSach");
            }                
            
        }

        public ActionResult BookDetail(int id)
        {
            if (Session["seller"] == null)
            {
                return RedirectToAction("LoginS", "Seller");
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
        public ActionResult Delete(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpPost,ActionName("Delete")]
        public ActionResult Xacnhan(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
                        
            db.SACHes.DeleteOnSubmit(sach);
            db.SubmitChanges();
            return RedirectToAction("QLSach");
        }

        [HttpGet]
        public ActionResult EditBook(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);

            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            ViewBag.MaNXB = new SelectList(db.NXBs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            ViewBag.MaTL = new SelectList(db.THELOAIs.ToList().OrderBy(n => n.TenTL), "MaTL", "TenTL");

            return View(sach);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditBook(SACH sach, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaNXB = new SelectList(db.NXBs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            ViewBag.MaTL = new SelectList(db.THELOAIs.ToList().OrderBy(n => n.TenTL), "MaTL", "TenTL");

            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var filename = DateTime.Now.ToString("ddMMyyyy_hhmmss") + Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images"), filename);

                    //if (System.IO.File.Exists(path))
                    //{
                    //    ViewBag.ThongBao = "File đã tồn tại";
                    //}
                    //else
                    //{

                    //}
                    fileUpload.SaveAs(path);

                    var s = db.SACHes.SingleOrDefault(sa => sa.MaSach == sach.MaSach);

                    s.AnhBia = filename;
                    s.NgayUpdate = DateTime.Now;
                    s.MaNB = (int)Session["seller"];
                    s.GiaBan = sach.GiaBan;
                    s.MaNXB = sach.MaNXB;
                    s.MaTL = sach.MaTL;
                    s.MoTa = sach.MoTa;
                    s.ShipNoiThanh = sach.ShipNoiThanh;
                    s.ShipTinh = sach.ShipTinh;
                    s.Soluong = sach.Soluong;
                    s.TacGia = sach.TacGia;
                    s.TenSach = sach.TenSach;

                    //UpdateModel(sach);
                    //db.SACHes.Attach(sach);
                    db.SubmitChanges();
                }
                return RedirectToAction("QLSach");
            }
        }

        public ActionResult DangXuat()
        {
            Session["seller"]=null;
            return RedirectToAction("LoginS", "Seller");
        }

        public ActionResult InfoSeller()
        {
            if (Session["seller"] == null)
            {
                return RedirectToAction("LoginS", "Seller");
            }
            else
            {
                int idS = (int)Session["seller"];
                var info = from i in db.NGUOIBANs where i.MaNB == idS select i;
                return View(info.Single());
            }
        }
    }
}
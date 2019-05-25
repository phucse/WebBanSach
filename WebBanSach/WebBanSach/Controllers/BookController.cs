using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class BookController : Controller
    {
        dbBookDataContext db = new dbBookDataContext();

        // GET: Book
        public ActionResult Index()
        {
            var book = GetBook();           
            return View(book);
        }

        public List<SACH> GetBook()
        {
            return db.SACHes.OrderByDescending(a => a.NgayUpdate).ToList();
        }

        public ActionResult TheLoai()
        {
            var type = from tl in db.THELOAIs select tl;
            return PartialView(type);
        }

        public ActionResult SachTheoTL(int id)
        {
            var book = from b in db.SACHes where b.MaTL == id select b;
            return View(book);
        }

        public ActionResult NXB()
        {
            var nxb = from n in db.NXBs select n;
            return PartialView(nxb);
        }

        public ActionResult SachTheoNXB(int id)
        {
            var book = from b in db.SACHes where b.MaNXB == id select b;
            return View(book);
        }

        public ActionResult Detail(int id)
        {
            var book = from b in db.SACHes
                       where b.MaSach == id
                       select b;
            return View(book.Single());
        }

        public ActionResult Search(String searchCode)
        {
            List<SACH> sach;
            if (!String.IsNullOrEmpty(searchCode))
            {
                sach = db.SACHes.Where(n => n.TenSach.Contains(searchCode)).ToList();
                if (sach.Count == 0)
                {
                    ViewData["titleSearch"] = "Không tìm thấy cuốn sách này";
                    return View(sach);
                }
                else
                {
                    ViewData["titleSearch"] = "Có " + sach.Count() + " kết quả tìm kiếm: ";
                    return View(sach);
                }                    
            }
            else
            {
                return RedirectToAction("Index");
            }                 
        }
    }

}
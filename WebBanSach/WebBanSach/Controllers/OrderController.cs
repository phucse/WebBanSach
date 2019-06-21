using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach.Models;

namespace WebBanSach.Controllers
{
    public class OrderController : Controller
    {
        dbBookDataContext db = new dbBookDataContext();

        public ActionResult DuyetDH()
        {
            
            if (Session["seller"] == null)
            {
                return RedirectToAction("LoginS", "Seller");
            }
            else
            {
                int idSeller = (int)Session["seller"];
                //List<SACH> sach = db.SACHes.Where(s => s.MaNXB == idSeller).ToList();
                //foreach (var item in sach)
                //{
                //    List<CTDH> ctdh = db.CTDHs.Where(c => c.MaSach == item.MaSach).ToList();
                //    foreach (var ct in ctdh)
                //    {
                //        List<DONHANG> dh = db.DONHANGs.Where(d => d.MaDH == ct.MaDH && d.GiaoHang == false && d.ThanhToan == false).ToList();
                //        return View(dh);
                //    }
                //}
                var idSaches = db.NGUOIBANs.Single(s => s.MaNB == idSeller).SACHes.Select(s => s.MaSach);
                IEnumerable<CTDH> cthdes = db.CTDHs.Where(cthd => idSaches.Contains(cthd.MaSach));
                List<DONHANG> donhangs = new List<DONHANG>();
                foreach(var cthd in cthdes)
                {
                    donhangs.Add(cthd.DONHANG);
                }
                var donhanges = donhangs.Where(d => d.ThanhToan == false || d.GiaoHang == false).Distinct().ToList();
                return View(donhanges);
            }
        }

        public ActionResult DSDH ()
        {            
            if (Session["seller"] == null)
            {
                return RedirectToAction("LoginS", "Seller");
            }
            else
            {
                int idSeller = (int)Session["seller"];
                var idSaches = db.NGUOIBANs.Single(s => s.MaNB == idSeller).SACHes.Select(s => s.MaSach);
                IEnumerable<CTDH> ctdhes = db.CTDHs.Where(cthd => idSaches.Contains(cthd.MaSach));
                List<DONHANG> donhangs = new List<DONHANG>();
                foreach (var cthd in ctdhes)
                {
                    donhangs.Add(cthd.DONHANG);
                }
                var donhanges = donhangs.Where(d => d.ThanhToan == true && d.GiaoHang == true).Distinct().ToList();
                return View(donhanges);
            }
        }

        public ActionResult ChiTietDH(int id)
        {            
            if (Session["seller"] == null)
            {
                return RedirectToAction("LoginS", "Seller");
            }
            else
            {
                List<CTDH> ctdh = db.CTDHs.Where(c => c.MaDH == id).ToList();
                return View(ctdh);
            }
        }

        public ActionResult UpdateDH(int id,FormCollection collection)
        {
            int tt = int.Parse(collection["thanhtoan"]);
            int gh = int.Parse(collection["giaohang"]);

            var dh = db.DONHANGs.SingleOrDefault(d => d.MaDH == id);

            if(tt == 1)
            {
                dh.ThanhToan = true;
            }
            if (gh == 1)
            {
                dh.GiaoHang = true;
            }

            db.SubmitChanges();
            return RedirectToAction("DuyetDH");
            
        }

        public ActionResult GiaoHang(int id)
        {
            if (Session["seller"] == null)
            {
                return RedirectToAction("LoginS", "Seller");
            }
            else
            {
                var gh = from g in db.DIACHIGIAOs
                           where g.MaDH == id
                           select g;
                return View(gh.Single());
            }
        }
    }
}
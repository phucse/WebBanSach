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
            return View();
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
    }
}
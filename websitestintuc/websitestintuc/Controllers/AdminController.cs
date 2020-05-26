using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using websitestintuc.Models;
using System.IO;
using System.Text;

namespace websitestintuc.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        DataWebTinTucDataContext db = new DataWebTinTucDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult News()
        {
            return View(db.TinTucs.ToList());
        }
        public ActionResult Categories()
        {
            return View(db.categories.ToList());
        }
        public ActionResult LoGo()
        {
            return View(db.Logos.ToList());
        }
        public ActionResult Accounts()
        {
            return View(db.Accounts.ToList());
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = " Bạn phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Bạn phải nhập mật khẩu";
            }
            else
            {
                Account ad = db.Accounts.SingleOrDefault(n => n.userName == tendn && n.password == matkhau);
                if (ad != null)
                {
                    Session["tai khoan admin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.ThongBao = " ten dang nhap hoac mat khau khong dung ";
                }

            }
            return View();
        }
        [HttpGet]
        public ActionResult Themtinmoi()
        {
            ViewBag.IDlogo = new SelectList(db.Logos.ToList().OrderBy(n => n.SourceName), "ID", "SourceName");
            ViewBag.IDcategory = new SelectList(db.categories.ToList().OrderBy(n => n.CategoryName), "ID", "CategoryName");
            return View();
        }
        [HttpPost]
        public ActionResult Themtinmoi(TinTuc model, HttpPostedFileBase fileupload)
        {
            var fileName = Path.GetFileName(fileupload.FileName);
            var path = Path.Combine(Server.MapPath("~/ImageUpload"), fileName);
            if (System.IO.File.Exists(path))
            {
                ViewBag.ThongBao = "Hình Ảnh Đã Tồn Tại";
            }
            else
            {
                fileupload.SaveAs(path);
            }
            ViewBag.IDlogo = new SelectList(db.Logos.ToList().OrderBy(n => n.SourceName), "ID", "SourceName");
            ViewBag.IDcategory = new SelectList(db.categories.ToList().OrderBy(n => n.CategoryName), "ID", "CategoryName");
            return RedirectToAction("News");
        }

    }
}

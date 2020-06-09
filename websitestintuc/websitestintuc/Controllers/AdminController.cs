using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using websitestintuc.Models;
using System.IO;
using PagedList;
using PagedList.Mvc;
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
        public ActionResult News(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            return View(db.TinTucs.ToList().OrderBy(n => n.description).ToPagedList(pageNumber, pageSize));
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
                    ViewBag.ThongBao = " Tên Đăng Nhập hoặc Mật Khẩu Không Đúng ";
                }

            }
            return View();
        }
        
        [HttpGet]
        public ActionResult Themtaikhoan()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Themtaikhoan(Account account)
        {
            if (account == null)
            {
                ViewBag.ThongBao = "Vui Lòng Nhập Account";
            }


            db.Accounts.InsertOnSubmit(account);
            db.SubmitChanges();
            return RedirectToAction("Accounts");
        }
        public ActionResult DeleteAccounts(int id)
        {
            Account account = db.Accounts.SingleOrDefault(n => n.ID == id);

            return View(account);
        }
        [HttpPost, ActionName("DeleteAccounts")]
        public ActionResult confirmdelete3(int id)
        {
            Account account = db.Accounts.SingleOrDefault(n => n.ID == id);
            ViewBag.ID = account.ID;
            if (account == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Accounts.DeleteOnSubmit(account);
            db.SubmitChanges();
            return RedirectToAction("Accounts");
        }
        public ActionResult DetailsAccounts(int id)
        {
            Account account = db.Accounts.SingleOrDefault(n => n.ID == id);
            if (account == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(account);
        }
        [HttpGet]
        public ActionResult Themlogo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Themlogo(Logo logo,HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.ThongBao = "Vui Lòng Chọn LoGo";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Imagelogo"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình Ảnh Đã Tồn Tại";
                    }
                    else
                    {
                        file.SaveAs(path);
                    }
                    logo.Image = fileName;
                    db.Logos.InsertOnSubmit(logo);
                    db.SubmitChanges();
                }
                return RedirectToAction("LoGo");
            }

        }
        public ActionResult DeleteLoGo(int id)
        {
            Logo logo = db.Logos.SingleOrDefault(n => n.ID == id);

            return View(logo);
        }
        [HttpPost, ActionName("DeleteLoGo")]
        public ActionResult confirmdelete2(int id)
        {
            Logo logo = db.Logos.SingleOrDefault(n => n.ID == id);
            ViewBag.ID = logo.ID;
            if (logo == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Logos.DeleteOnSubmit(logo);
            db.SubmitChanges();
            return RedirectToAction("LoGo");
        }
        public ActionResult DetailsLoGo(int id)
        {
            Logo logo = db.Logos.SingleOrDefault(n => n.ID == id);
            if (logo == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(logo);
        }
        public ActionResult EditCategories(int id)
        {
            return View();
        }
        public ActionResult DeleteCategories(int id)
        {
           category category= db.categories.SingleOrDefault(n => n.ID == id);

            return View(category);
        }
        [HttpPost, ActionName("DeleteCategories")]
        public ActionResult confirmdelete1(int id)
        {
            category category= db.categories.SingleOrDefault(n => n.ID == id);
            ViewBag.ID = category.ID;
            if (category == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.categories.DeleteOnSubmit(category);
            db.SubmitChanges();
            return RedirectToAction("Categories");
        }
        public ActionResult DetailsCategories(int id)
        {
            category category = db.categories.SingleOrDefault(n => n.ID == id);
            if (category == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(category);
        }
        [HttpGet]
        public ActionResult Themloaimoi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Themloaimoi(category category)
        {
            if(category==null)
            {
                ViewBag.ThongBao = "Vui Lòng Nhập Tên Loại";
            }
            
                
            db.categories.InsertOnSubmit(category);
            db.SubmitChanges();
            return RedirectToAction("Categories");
        }
       
        [HttpGet]

        public ActionResult Themtinmoi()
        {
            ViewBag.IDlogo = new SelectList(db.Logos.ToList().OrderBy(n => n.SourceName), "ID", "SourceName");
            ViewBag.IDcategory = new SelectList(db.categories.ToList().OrderBy(n => n.CategoryName), "ID", "CategoryName");
            return View();
        }
        [HttpPost]
        public ActionResult Themtinmoi(TinTuc tin ,HttpPostedFileBase fileupload)
        {
            ViewBag.IDlogo = new SelectList(db.Logos.ToList().OrderBy(n => n.SourceName), "ID", "SourceName");
            ViewBag.IDcategory = new SelectList(db.categories.ToList().OrderBy(n => n.CategoryName), "ID", "CategoryName");
            if (fileupload == null)
            {
                ViewBag.ThongBao = "Vui Lòng Chọn Ảnh";
                return View();
            }
            else
            {
               if(ModelState.IsValid)
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
                    tin.Imagedemo = fileName;
                    db.TinTucs.InsertOnSubmit(tin);
                    db.SubmitChanges();
                }
                return RedirectToAction("News");
            }
            
        }

        public ActionResult Detailsnews(int id)
        {
            TinTuc tin = db.TinTucs.SingleOrDefault(n => n.ID == id);
            if (tin == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tin);
        }
        [HttpGet]
        public ActionResult Deletenews(int id)
        {
            TinTuc tin = db.TinTucs.SingleOrDefault(n => n.ID == id);

            return View(tin);
        }
        [HttpPost, ActionName("Deletenews")]
        public ActionResult confirmdelete(int id)
        {
            TinTuc tin = db.TinTucs.SingleOrDefault(n => n.ID == id);
            ViewBag.ID = tin.ID;
            if (tin == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.TinTucs.DeleteOnSubmit(tin);
            db.SubmitChanges();
            return RedirectToAction("News");
        }
        [HttpGet]
        public ActionResult Editnews(int id)
        {
            TinTuc tin = db.TinTucs.SingleOrDefault(n => n.ID == id);
            if (tin == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.IDlogo = new SelectList(db.Logos.ToList().OrderBy(n => n.SourceName), "ID", "SourceName", tin.IDlogo);
            ViewBag.IDcategory = new SelectList(db.categories.ToList().OrderBy(n => n.CategoryName), "ID", "CategoryName", tin.IDcategory);
            return View(tin);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Editnews(TinTuc tin ,HttpPostedFileBase fileupload)
        {
            ViewBag.IDlogo = new SelectList(db.Logos.ToList().OrderBy(n => n.SourceName), "ID", "SourceName");
            ViewBag.IDcategory = new SelectList(db.categories.ToList().OrderBy(n => n.CategoryName), "ID", "CategoryName");
            if(fileupload==null)
            {
                ViewBag.ThongBao = "Vui Lòng Chọn Ảnh";
                return View();
            }
            else
            {
                if(ModelState.IsValid)
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
                    tin.Imagedemo = fileName;
                    UpdateModel(tin);
                    db.SubmitChanges();
                }
               
                return RedirectToAction("News");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;
using doan.Models;
using System.Web.Security;

namespace doan.Controllers
{
    public class AdminController : Controller
    {
        QLBanxeDataContext db = new QLBanxeDataContext();
        // GET: /Admin/
        public ActionResult Index()
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            return View();
        }
        public ActionResult Xe(int ?page)
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.XEs.ToList().OrderBy(n => n.Maxe).ToPagedList(pageNumber,pageSize));
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            var checkuseradmin = db.Admins.Any(k => k.UserAdmin == admin.UserAdmin);
            var checkpassadmin = db.Admins.Any(k => k.PassAdmin == admin.PassAdmin);

            if (!checkuseradmin)
            {
                ModelState.AddModelError("", "Tài khoản không đúng");
                if (!checkpassadmin)
                    ModelState.AddModelError("", "Mật khẩu không đúng");
                return View("Login");
            }
            if (!checkpassadmin)
            {
                ModelState.AddModelError("", "Mật khẩu không đúng");
                if (!checkuseradmin)
                    ModelState.AddModelError("", "Tài khoản không đúng");
                return View("Login");
            }
            var tenUser = db.Admins.FirstOrDefault(a => a.UserAdmin == admin.UserAdmin).Hoten;
            Session["UserAdmin"] = tenUser;
            return RedirectToAction("Xe", "Admin");
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Admin");
        }
        //public ActionResult Login()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Login(FormCollection collection)
        //{
        //    // Gán các giá trị người dùng nhập liệu cho các biến 
        //    var tendn = collection["username"];
        //    var matkhau = collection["password"];
        //    if (String.IsNullOrEmpty(tendn))
        //    {
        //        ViewData["Loi1"] = "Phải nhập tên đăng nhập";
        //    }
        //    else if (String.IsNullOrEmpty(matkhau))
        //    {
        //        ViewData["Loi2"] = "Phải nhập mật khẩu";
        //    }
        //    else
        //    {
        //        //Gán giá trị cho đối tượng được tạo mới (ad)        

        //        Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
        //        if (ad != null)
        //        {
        //            // ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
        //            Session["Taikhoanadmin"] = ad;
        //            return RedirectToAction("Xe", "Admin");
        //        }
        //        else
        //            ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
        //    }
        //    return View();
        //} 
        [HttpGet]
        public ActionResult ThemmoiXe()
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            ViewBag.MaLX = new SelectList(db.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaixe");
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiXe(XE xe, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaLX = new SelectList(db.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaixe");
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Hinh"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    xe.Anhbia = fileName;
                    //Luu vao CSDL
                    db.XEs.InsertOnSubmit(xe);
                    db.SubmitChanges();
                }
                return RedirectToAction("Xe");
            }
        }
        public ActionResult Chitietxe(int id)
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            //Lay ra doi tuong sach theo ma
            XE xe = db.XEs.SingleOrDefault(n => n.Maxe == id);
            ViewBag.Masach = xe.Maxe;
            if (xe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(xe);
        }
        [HttpGet]
        public ActionResult Xoaxe(int id)
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            //Lay ra doi tuong sach can xoa theo ma
            XE xe = db.XEs.SingleOrDefault(n => n.Maxe == id);
            ViewBag.Maxe = xe.Maxe;
            if (xe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(xe);
        }
        [HttpPost, ActionName("Xoaxe")]
        public ActionResult Xacnhanxoa(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            XE xe = db.XEs.SingleOrDefault(n => n.Maxe == id);
            ViewBag.Maxe = xe.Maxe;
            if (xe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.XEs.DeleteOnSubmit(xe);
            db.SubmitChanges();
            return RedirectToAction("Xe");
        }
        [HttpGet]
        public ActionResult Suaxe(int id)
        {
            if (Session["UserAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            //Lay ra doi tuong sach theo ma
            XE xe = db.XEs.SingleOrDefault(n => n.Maxe == id);
            ViewBag.Maxe = xe.Maxe;
            if (xe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Dua du lieu vao dropdownList
            //Lay ds tu tabke chu de, sắp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude
            ViewBag.MaLX = new SelectList(db.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaixe", xe.MaLX);
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX", xe.MaNSX);
            return View(xe);
        }
        [HttpPost]
        [ValidateInput(false)]
        //public ActionResult Suaxe(XE xe, HttpPostedFileBase fileUpload)
        //{
        //    //Dua du lieu vao dropdownload
        //    ViewBag.MaLX = new SelectList(db.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaixe");
        //    ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            
        //    //Kiem tra duong dan file
        //    if (fileUpload == null)
        //    {
        //        ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
        //        return View();
        //    }
        //    //Them vao CSDL
        //    else
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            //Luu ten fie, luu y bo sung thu vien using System.IO;
        //            var fileName = Path.GetFileName(fileUpload.FileName);
        //            //Luu duong dan cua file
        //            var path = Path.Combine(Server.MapPath("~/Hinh"), fileName);
        //            //Kiem tra hình anh ton tai chua?
        //            if (System.IO.File.Exists(path))
        //                ViewBag.Thongbao = "Hình ảnh đã tồn tại";
        //            else
        //            {
        //                //Luu hinh anh vao duong dan
        //                fileUpload.SaveAs(path);
        //            }
        //            var sp = db.XEs.FirstOrDefault(s => s.Maxe == xe.Maxe);
        //            xe.Anhbia = fileName;
        //            //Luu vao CSDL   
        //            //UpdateModel(xe);
        //            sp.Tenxe = xe.Tenxe;
        //            sp.Giaban = xe.Giaban;
        //            sp.Mota = xe.Mota;
        //            sp.Ngaycapnhat = xe.Ngaycapnhat;
        //            sp.Soluongton = xe.Soluongton;
        //            sp.MaNSX = xe.MaNSX;
        //            sp.MaLX = xe.MaLX;
        //            db.SubmitChanges();

        //        }
        //        return RedirectToAction("Xe");
        //    }
        //}
         public ActionResult Suaxe(XE xe, HttpPostedFileBase fileUpload, string Anhbia)
        {
            //if (Session["UserAdmin"] == null)
            //    return RedirectToAction("Login", "Admin");
            ViewBag.MaLX = new SelectList(db.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaixe");
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            if (ModelState.IsValid)
                {

                    var sp = db.XEs.FirstOrDefault(s => s.Maxe == xe.Maxe);
                    if (fileUpload != null)
                    {
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        var path = Path.Combine(Server.MapPath("~/Hinh"), fileName);
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            fileUpload.SaveAs(path);
                        }
                        sp.Anhbia = fileName;
                    }
                    sp.Tenxe = xe.Tenxe;
                    sp.Giaban = xe.Giaban;
                    sp.Mota = xe.Mota;
                    sp.Ngaycapnhat = xe.Ngaycapnhat;
                    sp.Soluongton = xe.Soluongton;
                    sp.MaNSX = xe.MaNSX;
                    sp.MaLX = xe.MaLX;
                    db.SubmitChanges();
                }
                return RedirectToAction("Xe");
        }
    }
}
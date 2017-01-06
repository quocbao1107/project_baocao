using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doan.Models;
using PagedList;
using PagedList.Mvc;

namespace doan.Controllers
{
    public class otoController : Controller
    {
        QLBanxeDataContext data = new QLBanxeDataContext();
        // GET: oto
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult XeMoiPartial()
        {
            var lstXemoi = data.XEs.Take(3).ToList();
            return PartialView(lstXemoi);
        }

        public PartialViewResult XeCungLoaiPartial()
        {
            var lstXecungloai = data.XEs.Take(10).ToList();
            return PartialView(lstXecungloai);
        }
        private List<XE> Layxemoi(int count)
        {
            //Sắp xếp sách theo ngày cập nhật, sau đó lấy top @count 
            return data.XEs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Sanpham(int ? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
          
            var xemoi = Layxemoi(15);

            return View(xemoi.ToPagedList(pageNum,pageSize));
        }
        public ActionResult SPTheoloaixe(int id)
        {
            var xe = from s in data.XEs where s.MaLX == id select s;
            return View(xe);
        }
        public ActionResult Chitiet(int id)
        {
            var xe = from s in data.XEs
                       where s.Maxe == id
                       select s;
            return View(xe.Single());
        }
        public ActionResult Bando()
        {
            return View();
        }
        
    }
}
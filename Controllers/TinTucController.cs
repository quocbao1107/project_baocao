using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doan.Models;
using PagedList.Mvc;
using PagedList;


namespace doan.Controllers
{
    public class TinTucController : Controller
    {
        //
        // GET: /TinTuc/
        QLBanxeDataContext data = new QLBanxeDataContext();
        //public ActionResult Index(int n = 5)
        //{
        //    var item = data.TINTUCs.Take(n).ToList();
        //    return View(item);
        //}
        //public ActionResult ChiTietTin(int id)
        //{
        //    var tintuc = from t in data.TINTUCs
        //                 where t.MaTin == id
        //                 select t;
        //    return View(tintuc.Single());
        //}
        private List<TINTUC> Laytintucmoi(int count)
        {
            //s?p x?p gi?m d?n theo ngaycapnhat, lay count dong dau
            return data.TINTUCs.OrderByDescending(a => a.NgayViet).Take(count).ToList();
        }
        public ActionResult Index(int? page)
        {
            // t?o bi?n quy d?nh s? ?n tin t?c trên môi tru?ng
            int pageSize = 3;
            //t?o bi?n s? trang
            int pageNum = (page ?? 1);

            // l?y top 5 tin t?c bán m?i nh?t
            var sanphammoi = Laytintucmoi(10);
            return View(sanphammoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult ChiTietTin(int id)
        {
            var tintuc = from s in data.TINTUCs
                         where s.MaTin == id
                         select s;
            return View(tintuc.Single());
        }
    }
}
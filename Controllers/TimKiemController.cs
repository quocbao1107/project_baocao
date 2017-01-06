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
    public class TimKiemController : Controller
    {
        QLBanxeDataContext db = new QLBanxeDataContext();
        // GET: TimKiem

        public ActionResult Search()
        {
            var model = new List<XE>();
            return View(model);
        }

        [HttpPost]
        public ActionResult Search(String Keywords = "")
        {
            var model = db.XEs
                .Where(p => p.Tenxe.Contains(Keywords));
            return View(model);
        }
    }
}
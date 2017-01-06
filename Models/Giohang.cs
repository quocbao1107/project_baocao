using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doan.Models;

namespace doan.Models
{
    public class Giohang
    {
        //Tao doi tuong data chua dữ liệu từ model dbBansach đã tạo. 
        QLBanxeDataContext data = new QLBanxeDataContext();
        public int iMaxe { set; get; }
        public string sTenxe { set; get; }
        public string sAnhbia { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }

        }
        //Khoi tao gio hàng theo Masach duoc truyen vao voi Soluong mac dinh la 1
        public Giohang(int Maxe)
        {
            iMaxe = Maxe;
            XE xe = data.XEs.Single(n => n.Maxe == iMaxe);
            sTenxe = xe.Tenxe;
            sAnhbia = xe.Anhbia;
            dDongia = double.Parse(xe.Giaban.ToString());
            iSoluong = 1;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SEP_Team1.Models;

namespace SEP_Team1.Controllers
{
    public class HomeController : Controller
    {
        SEPdataEntities db = new SEPdataEntities();
        public ActionResult Index()
        {
            string maGV = Session["MaGV"] as string;
            var monhoc = db.MonHoc.Where(mh => mh.GiangVien.Count(gv => gv.maGV == maGV) > 0).ToList();
            var maMH = monhoc.Select(mh=>mh.maMH).ToList();
            var khoahoc = db.KhoaHoc.Where(kh=>maMH.Contains(kh.maMH)).ToList();
            var maKH = khoahoc.Select(kh => kh.maKH);
            var buoihoc = db.BangBuoiHoc.Where(bh=>maKH.Contains(bh.maKH)).ToList();
            ViewBag.Days = buoihoc.Count();
            ViewBag.nKhoaHoc = khoahoc.Count();
            ViewBag.Lesson = khoahoc;
            ViewBag.Subject = monhoc;
            return View();
        }
        public ActionResult Course(string id)
        {
            string maGV = Session["MaGV"] as string;
            var monhoc = db.MonHoc.Where(mh => mh.GiangVien.Count(gv => gv.maGV == maGV) > 0).ToList();
            ViewBag.Subject = monhoc;          
            var ikhoahoc = db.KhoaHoc.Where(kh => kh.maMH == id).ToList();
            var maKH = ikhoahoc.Select(kh => kh.maKH);
            ViewBag.iLesson = ikhoahoc;
            var buoihoc = db.BangBuoiHoc.Where(bh => maKH.Contains(bh.maKH)).ToList();
            ViewBag.Classes = buoihoc.Count();
            var sinhvien = db.SinhVien.ToList();
            ViewBag.Student = sinhvien.Count();
            return View();
        }
        public ActionResult Attendance(string id)
        {
            string maGV = Session["MaGV"] as string;
            var monhoc = db.MonHoc.Where(mh => mh.GiangVien.Count(gv => gv.maGV == maGV) > 0).ToList();
            var sinhvien = db.SinhVien.Where(sv => sv.KhoaHoc.Count(kh => kh.maKH == id) > 0).ToList();
            var buoihoc = db.BangBuoiHoc.Where(bh =>bh.maKH == id).ToList();

            var mabuoihoc = buoihoc.Select(mh => mh.maBH).ToArray();
            string mbh = mabuoihoc[0];

            var diemdanh = db.DiemDanh.Where(dd => dd.maBH == mbh);
            var model = new DAO();

            if (ModelState.IsValid)
            {
             
                foreach (var at in diemdanh)
                {

                    if (at.diemDanh1 == true)
                    {
                        db.DiemDanh.Add(new DiemDanh
                        {
                            MSSV = (string)at.MSSV

                        });
                    }
                }

            }



                ViewBag.Student = sinhvien;
            ViewBag.Diemdanh = diemdanh.ToList();
            ViewBag.CountStudent = sinhvien.Count();
            ViewBag.Buoihoc = buoihoc;
            ViewBag.Subject = monhoc;
            return View();

        }

        [HttpPost]
        public ActionResult Check(string bhoc)
        {
            var model = new DAO();
     
            string maGV = Session["MaGV"] as string;

            var Fch = Request.Form.AllKeys.Where(k => k != "bhoc");
            db.DiemDanh.RemoveRange(db.DiemDanh.Where(d => d.maBH == bhoc));
            db.SaveChanges();
             
            foreach (var fea in Fch )
            {
                {
                    db.DiemDanh.Add(new DiemDanh
                    {
                        maBH = bhoc,
                        MSSV = fea,
                        diemDanh1 = Request.Form[fea] != "false"
                        
                    });
                }
            }
            db.SaveChanges();
            var monhoc = db.MonHoc.Where(mh => mh.GiangVien.Count(gv => gv.maGV == maGV) > 0).ToList();
          
            var diemdanh = db.DiemDanh.Include("SinhVien").Include("BangBuoiHoc").Where(dd => dd.maBH == bhoc);
       



            ViewBag.Diemdanh = diemdanh.ToList();
 
            ViewBag.Subject = monhoc;
            return PartialView(diemdanh.ToList());

        }
        public ActionResult Login()
        {
            return View();
        } 
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = db.GiangVien.FirstOrDefault(x => x.tenTK == username);
            if (user != null)
            {
                if (user.password.Equals(password))
                {
                    Session["hoten"] = user.hoten;
                    Session["MaGV"] = user.maGV;
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.mgs = "tai khoang khong ton tai";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session["hoten"] = null;
            Session["MaGV"] = null;
            return RedirectToAction("Login", "Home");
        }
       public ActionResult StudentProfile(string id )
        {
            var studentInfo = db.SinhVien.FirstOrDefault(x=>x.MSSV == id);
            return View(studentInfo);
        }
        public ActionResult Enrollment()
        {
            string maGV = Session["MaGV"] as string;
            var monhoc = db.MonHoc.Where(mh => mh.GiangVien.Count(gv => gv.maGV == maGV) > 0).ToList();
            return View();
        }


      
    }
}
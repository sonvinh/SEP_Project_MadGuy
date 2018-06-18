using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SEP_Team1.Models;
using SEP_Team1.API;

namespace SEP_Team1.Controllers
{
    public class HomeController : Controller
    {
        sep21t21Entities db = new sep21t21Entities();
        APIConnect connect = new APIConnect();
        public ActionResult Index()
        {
            string maGV = Session["MaGV"] as string;
            var monhoc = db.MonHocs.Where(mh => mh.GiangViens.Count(gv => gv.maGV == maGV) > 0).ToList();
            var maMH = monhoc.Select(mh=>mh.maMH).ToList();
            var khoahoc = db.KhoaHocs.Where(kh=>maMH.Contains(kh.maMH)).ToList();
            var maKH = khoahoc.Select(kh => kh.maKH);
            var buoihoc = db.BangBuoiHocs.Where(bh=>maKH.Contains(bh.maKH)).ToList();
            ViewBag.Days = buoihoc.Count();
            ViewBag.nKhoaHoc = khoahoc.Count();
            ViewBag.Lesson = khoahoc;
            ViewBag.Subject = monhoc;
            return View();
        }
        public ActionResult Course(string id)
        {
            string maGV = Session["MaGV"] as string;
            var monhoc = db.MonHocs.Where(mh => mh.GiangViens.Count(gv => gv.maGV == maGV) > 0).ToList();
            ViewBag.Subject = monhoc;          
            var ikhoahoc = db.KhoaHocs.Where(kh => kh.maMH == id).ToList();
            var maKH = ikhoahoc.Select(kh => kh.maKH);
            ViewBag.iLesson = ikhoahoc;
            var buoihoc = db.BangBuoiHocs.Where(bh => maKH.Contains(bh.maKH)).ToList();
            ViewBag.Classes = buoihoc.Count();
            var sinhvien = db.SinhViens.ToList();
            ViewBag.Student = sinhvien.Count();
            return View();
        }
        public ActionResult Attendance(string id)
        {
            var item = connect.GetStudent(id);
            
            // xử lí lưu dsach svien API
            if (db.SinhViens.Where(s => s.maKH == id).Count() == 0)
            {
                foreach (var svien in item)
                {
                    SinhVien stu = new SinhVien();
                    stu.fullname = svien.fullname;
                    stu.lastname = svien.lastname;
                    stu.firstname = svien.firstname;
                    stu.birthday = Convert.ToDateTime(svien.birthday);
                    stu.MSSV = svien.id;
                    stu.maKH = id;
                    db.SinhViens.Add(stu);
                    db.SaveChanges();
                }
            }
            // xử lí lưu vào bang điểm danh
            foreach (var at in item)
            {
                DiemDanh atten = new DiemDanh();
                if (id == "MH1")
                {
                    atten.maBH = "BHMH1001";
                }
                if (id == "MH2")
                {
                    atten.maBH = "BHMH2001";
                }
                if (id == "MH3")
                {
                    atten.maBH = "BHMH3001";
                }
                atten.MaKH = id;
                atten.MSSV = at.id;
                atten.Date = DateTime.Now.Date;
                atten.Time = DateTime.Now.TimeOfDay;
                atten.diemDanh1 = false;
                db.DiemDanhs.Add(atten);
                db.SaveChanges();
            }


            string maGV = Session["MaGV"] as string;
            var monhoc = db.MonHocs.Where(mh => mh.GiangViens.Count(gv => gv.maGV == maGV) > 0).ToList();
            var sinhvien = db.SinhViens.Where(sv => sv.KhoaHocs.Count(kh => kh.maKH == id) > 0).ToList();
            var buoihoc = db.BangBuoiHocs.Where(bh =>bh.maKH == id).ToList();

            var mabuoihoc = buoihoc.Select(mh => mh.maBH).ToArray();
            string mbh = mabuoihoc[0];

            var diemdanh = db.DiemDanhs.Where(dd => dd.maBH == mbh);
          

            if (ModelState.IsValid)
            {       
                foreach (var at in diemdanh)
                {
                    if (at.diemDanh1 == true)
                    {
                        db.DiemDanhs.Add(new DiemDanh
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
            string maGV = Session["MaGV"] as string;

            var Fch = Request.Form.AllKeys.Where(k => k != "bhoc");
            db.DiemDanhs.RemoveRange(db.DiemDanhs.Where(d => d.maBH == bhoc));
            db.SaveChanges();
            
            foreach (var fea in Fch )
            {
                {
                    db.DiemDanhs.Add(new DiemDanh
                    {
                        maBH = bhoc,
                        MSSV = fea,
                        diemDanh1 = Request.Form[fea] != "false"
                    });
                }
            }
            db.SaveChanges();
            var monhoc = db.MonHocs.Where(mh => mh.GiangViens.Count(gv => gv.maGV == maGV) > 0).ToList();
          
            var diemdanh = db.DiemDanhs.Include("SinhVien").Include("BangBuoiHoc").Where(dd => dd.maBH == bhoc);

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
            var user = db.GiangViens.FirstOrDefault(x => x.tenTK == username);
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
            var studentInfo = db.SinhViens.FirstOrDefault(x=>x.MSSV == id);
            return View(studentInfo);
        }
        public ActionResult Enrollment()
        {
            string maGV = Session["MaGV"] as string;
            var monhoc = db.MonHocs.Where(mh => mh.GiangViens.Count(gv => gv.maGV == maGV) > 0).ToList();
            return View();
        }
    }
}
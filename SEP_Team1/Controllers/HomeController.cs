﻿using System;
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

        public ActionResult ListStudent(string id)
        {
            var item = connect.GetStudent(id);
            //xử lí lưu dsach svien API
            if (db.SinhViens.Where(s => s.maKH == id).Count() == 0)
            {
                foreach (var svien in item)
                {
                    //if(db.SinhViens.Where(x => x.MSSV == svien.id && x.maKH == id).Count() != 0)
                    //{
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
            
            string maGV = Session["MaGV"] as string;
            var monhoc = db.MonHocs.Where(mh => mh.GiangViens.Count(gv => gv.maGV == maGV) > 0).ToList();
            var sinhvien = db.SinhViens.Where(v => v.maKH == id).ToList();
            ViewBag.Student = sinhvien;
            ViewBag.Subject = monhoc;

            return View();
        }
        public ActionResult diemDanh( string id)
        {
            string maGV = Session["MaGV"] as string;
            Session["maKH"] = id;
            var monhoc = db.MonHocs.Where(mh => mh.GiangViens.Count(gv => gv.maGV == maGV) > 0).ToList();
            var sinhvien = db.SinhViens.Where(sv => sv.maKH == id).ToList();
            var buoihoc = db.BangBuoiHocs.Where(bh => bh.maKH == id).ToList();
            var buoihocnew = buoihoc.OrderByDescending(x => x.Id).ToArray();
            string mbh = Convert.ToString(buoihocnew[0]);
            var diemdanh = db.DiemDanhs.Where(dd => dd.maBH == mbh);


            ViewBag.Student = sinhvien;
            ViewBag.Diemdanh = diemdanh.ToList();
            ViewBag.CountStudent = sinhvien.Count();
            ViewBag.Buoihoc = buoihoc;
            ViewBag.Subject = monhoc;
            return View();
        }
        [HttpPost]
        public ActionResult Ed(string bhoc)
        {
            string maGV = Session["MaGV"] as string;
            string maKh = Session["maKH"] as string;
            int ssion = Convert.ToInt32(Session["ss"]);
            var Fch = Request.Form.AllKeys.Where(k => k != "bhoc");
            db.DiemDanhs.RemoveRange(db.DiemDanhs.Where(d => d.maBH == bhoc));
            db.SaveChanges();
            foreach (var fea in Fch)
            {
                {
                    db.DiemDanhs.Add(new DiemDanh
                    {
                        sessionID = ssion,
                        MaKH = maKh,
                        Date = DateTime.Now.Date,
                        Time = TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss")),
                        maBH = bhoc,
                        MSSV = fea.Trim(),
                        diemDanh1 = Request.Form[fea.Trim()] != "false"

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
        public ActionResult Attendance(string id)
        {
           Session["maKH"] = id;
           var bhoc = db.BangBuoiHocs.Where(b => b.maKH == id).ToList();
           var svien = db.SinhViens.Where(v => v.maKH == id).ToList();
           var max = db.DiemDanhs.Where(m => m.MaKH == id).Max(x => x.sessionID) + 1;
            Session["ss"] = max;
            // xử lí lưu vào bang điểm danh
            foreach (var bh in bhoc)
            {
                foreach (var sv in svien)
                {
                    DiemDanh atten = new DiemDanh();
                    atten.maBH = bh.maBH;
                    atten.MaKH = id;
                    atten.MSSV = sv.MSSV;

                    if (max == 0 || max == null)
                    {
                        atten.sessionID = 1;
                    }
                    else
                    {
                        atten.sessionID = max;
                    }

                    atten.Date = DateTime.Now.Date;
                    atten.Time = TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));
                    atten.diemDanh1 = false;
                    db.DiemDanhs.Add(atten);
                    db.SaveChanges();
                }
            }

            string maGV = Session["MaGV"] as string;
            var monhoc = db.MonHocs.Where(mh => mh.GiangViens.Count(gv => gv.maGV == maGV) > 0).ToList();
            var sinhvien = db.SinhViens.Where(sv => sv.maKH == id).ToList();
            var buoihoc = db.BangBuoiHocs.Where(bh =>bh.maKH == id).ToList();

            var mabuoihoc = buoihoc.Select(mh => mh.maBH).ToArray();
            string mbh = mabuoihoc[0];

            var diemdanh = db.DiemDanhs.Where(dd => dd.maBH == mbh);
         
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
            string maKh = Session["maKH"] as string;
            int ssion = Convert.ToInt32(Session["ss"]);
            var Fch = Request.Form.AllKeys.Where(k => k != "bhoc");
            db.DiemDanhs.RemoveRange(db.DiemDanhs.Where(d => d.maBH == bhoc));
            db.SaveChanges();        
                foreach (var fea in Fch)
                {
                    {
                    db.DiemDanhs.Add(new DiemDanh
                    {
                        sessionID = ssion,
                            MaKH = maKh,
                            Date = DateTime.Now.Date,
                            Time= TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss")),
                            maBH = bhoc,
                            MSSV = fea.Trim(),
                            diemDanh1 = Request.Form[fea.Trim()] != "false"
                            
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
            //string id = connect.Login(username, password);
            //if (id != "")
            //{
            //    Session["id"] = id;
            //    Session["username"] = username;
            //    return RedirectToAction("Index", "Home");
            //}
            //else
            //{
            //    ViewBag.mgs = "username hoặc password không đúng, Xin thử lại!!!";
            //    return View();
            //}
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session["hoten"] = null;
            Session["MaGV"] = null;
            return RedirectToAction("Login", "Home");
        }
    }
}
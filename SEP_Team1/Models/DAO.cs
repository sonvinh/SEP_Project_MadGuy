using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEP_Team1.Models
{
    public class DAO
    {
        sep21t21Entities db = null;
        public DAO()
        {
            db = new sep21t21Entities();
        }
        public string attendanceSV(SinhVien entity)
        {
            db.SinhViens.Add(entity);
            db.SaveChanges();
            return entity.MSSV;
        }
    }
}
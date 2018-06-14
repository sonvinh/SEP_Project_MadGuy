using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEP_Team1.Models
{
    public class DAO
    {
        SEPdataEntities db = null;
        public DAO()
        {
            db = new SEPdataEntities();
        }
        public string attendanceSV(SinhVien entity)
        {
            db.SinhVien.Add(entity);
            db.SaveChanges();
            return entity.MSSV;
        }
    }
}
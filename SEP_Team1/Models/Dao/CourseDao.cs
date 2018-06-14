using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SEP_Team1.Models;
using SEP_Team1.Controllers;

namespace SEP_Team1.Models.Dao
{
    public class CourseDao
    {
        sep21t21Entities db = null;
        public CourseDao()
        {
            db = new sep21t21Entities();
        }

       
    }
}
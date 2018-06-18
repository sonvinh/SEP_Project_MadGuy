using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEP_Team1.Models
{
    public class StudentAPI
    {
        public int code { get; set; }
        public List<Info> data { get; set; }
        public string message { get; set; }
    }
    public class Info
    {
        public string id { get; set; }
        public string fullname { get; set; }
        public string birthday { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
    }
}
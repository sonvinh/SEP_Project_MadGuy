using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEP_Team1.Models
{
    public class GiangVienAPI
    {
        public int code { get; set; }
        public List<GiangVienInfo> data { get; set; }
        public string message { get; set; }
    }
    public class GiangVienInfo
    {
        public string id { get; set; }
        public string secret { get; set; }
    }
}
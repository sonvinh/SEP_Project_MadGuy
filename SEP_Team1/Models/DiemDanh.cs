//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SEP_Team1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DiemDanh
    {
        public int Id { get; set; }
        public string maBH { get; set; }
        public string MaKH { get; set; }
        public string MSSV { get; set; }
        public System.DateTime Date { get; set; }
        public System.TimeSpan Time { get; set; }
        public bool diemDanh1 { get; set; }
    
        public virtual BangBuoiHoc BangBuoiHoc { get; set; }
        public virtual SinhVien SinhVien { get; set; }
    }
}

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
    
    public partial class BangBuoiHoc
    {
        public string maBH { get; set; }
        public string maKH { get; set; }
        public string day { get; set; }
        public Nullable<System.TimeSpan> time { get; set; }
        public string room { get; set; }
        public int Id { get; set; }
    
        public virtual KhoaHoc KhoaHoc { get; set; }
    }
}

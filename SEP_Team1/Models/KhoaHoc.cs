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
    
    public partial class KhoaHoc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhoaHoc()
        {
            this.SinhVien = new HashSet<SinhVien>();
            this.BangBuoiHoc = new HashSet<BangBuoiHoc>();
        }
    
        public string maKH { get; set; }
        public string nienkhoa { get; set; }
        public System.DateTime ngaybatdau { get; set; }
        public System.DateTime ngayketthuc { get; set; }
        public string maMH { get; set; }
        public Nullable<int> sobuoihoc { get; set; }
    
        public virtual MonHoc MonHoc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SinhVien> SinhVien { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BangBuoiHoc> BangBuoiHoc { get; set; }
    }
}
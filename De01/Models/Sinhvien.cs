namespace De01.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sinhvien")]
    public partial class Sinhvien
    {
        [Key]
        [StringLength(6)]
        public string MaSV { get; set; }

        [Required]
        [StringLength(40)]
        public string HotenSV { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgaySinh { get; set; }

        [StringLength(3)]
        public string MaLop { get; set; }

        public virtual Lop Lop { get; set; }
    }
}

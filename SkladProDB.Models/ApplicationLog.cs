namespace SkladProDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ApplicationLog")]
    public partial class ApplicationLog
    {
        public int ID { get; set; }

        [StringLength(255)]
        public string Message { get; set; }

        public int? UserID { get; set; }

        public DateTime? UserRealtime { get; set; }

        [StringLength(50)]
        public string MessageSource { get; set; }
    }
}

namespace SkladProDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OperationType")]
    public partial class OperationType
    {
        public int ID { get; set; }

        [StringLength(255)]
        public string BG { get; set; }

        [StringLength(255)]
        public string EN { get; set; }

        [StringLength(255)]
        public string DE { get; set; }

        [StringLength(255)]
        public string RU { get; set; }

        [StringLength(255)]
        public string TR { get; set; }

        [StringLength(255)]
        public string SQ { get; set; }

        [StringLength(255)]
        public string SR { get; set; }

        [StringLength(255)]
        public string RO { get; set; }

        [StringLength(255)]
        public string GR { get; set; }
    }
}

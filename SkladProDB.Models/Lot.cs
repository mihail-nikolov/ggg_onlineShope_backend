namespace SkladProDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Lot
    {
        public int ID { get; set; }

        [StringLength(255)]
        public string SerialNo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? EndDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ProductionDate { get; set; }

        [StringLength(255)]
        public string Location { get; set; }
    }
}

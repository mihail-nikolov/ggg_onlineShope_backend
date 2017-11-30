namespace SkladProDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PriceRule
    {
        public int ID { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Formula { get; set; }

        public int? Enabled { get; set; }

        public int? Priority { get; set; }
    }
}

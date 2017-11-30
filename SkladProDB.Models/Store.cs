namespace SkladProDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Store")]
    public partial class Store
    {
        public int ID { get; set; }

        public int? ObjectID { get; set; }

        public int? GoodID { get; set; }

        public double? Qtty { get; set; }

        public double? Price { get; set; }

        [StringLength(250)]
        public string Lot { get; set; }

        public int? LotID { get; set; }

        public int? LotOrder { get; set; }
    }
}

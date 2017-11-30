namespace SkladProDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Transformation
    {
        public int ID { get; set; }

        public int RootOperType { get; set; }

        public int RootAcct { get; set; }

        public int FromOperType { get; set; }

        public int FromAcct { get; set; }

        public int ToOperType { get; set; }

        public int ToAcct { get; set; }

        public int UserID { get; set; }

        public DateTime? UserRealTime { get; set; }
    }
}

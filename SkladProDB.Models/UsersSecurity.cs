namespace SkladProDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UsersSecurity")]
    public partial class UsersSecurity
    {
        public int ID { get; set; }

        public int? UserID { get; set; }

        [StringLength(100)]
        public string ControlName { get; set; }

        public int? State { get; set; }
    }
}

namespace SkladProDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InternalLog")]
    public partial class InternalLog
    {
        public int ID { get; set; }

        [StringLength(3000)]
        public string Message { get; set; }
    }
}

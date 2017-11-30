namespace SkladProDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Network")]
    public partial class Network
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Num { get; set; }

        public int Counter { get; set; }
    }
}

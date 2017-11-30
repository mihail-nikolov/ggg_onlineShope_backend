namespace SkladProDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CurrenciesHistory")]
    public partial class CurrenciesHistory
    {
        public int ID { get; set; }

        public int? CurrencyID { get; set; }

        public double? ExchangeRate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Date { get; set; }

        public int? UserID { get; set; }
    }
}

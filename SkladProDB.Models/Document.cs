namespace SkladProDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Document
    {
        public int ID { get; set; }

        public int? Acct { get; set; }

        [StringLength(255)]
        public string InvoiceNumber { get; set; }

        public short? OperType { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InvoiceDate { get; set; }

        public short? DocumentType { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ExternalInvoiceDate { get; set; }

        [StringLength(255)]
        public string ExternalInvoiceNumber { get; set; }

        public short? PaymentType { get; set; }

        [StringLength(255)]
        public string Recipient { get; set; }

        [StringLength(50)]
        public string EGN { get; set; }

        [StringLength(255)]
        public string Provider { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TaxDate { get; set; }

        [StringLength(255)]
        public string Reason { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(255)]
        public string Place { get; set; }
    }
}

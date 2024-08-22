using System;
using System.Collections.Generic;

namespace IMS.Models
{
    public partial class Invoice
    {
        public int Id { get; set; }  // Primary key

        public int? Cid { get; set; }

        public DateOnly? Date { get; set; }

        public decimal? TotalDiscount { get; set; }

        public decimal? GrandTotal { get; set; }

        public virtual Customer? CidNavigation { get; set; }

        public virtual ICollection<InvoiceList> InvoiceLists { get; set; } = new List<InvoiceList>();
    }
}

using System;

namespace IMS.Models
{
    public partial class InvoiceList
    {
        public int Id { get; set; }

        public int? InvoiceId { get; set; }

        public string? Name { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }

        public decimal? Total { get; set; }

        public decimal? Discount { get; set; }

        public virtual Invoice? Invoice { get; set; }
    }
}

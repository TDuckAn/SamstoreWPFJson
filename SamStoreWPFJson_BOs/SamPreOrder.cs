using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamStoreWPFJson_BOs
{
    public class SamPreOrder
    {
        public int PreOrderID { get; set; }

        public string PreOrderNo { get; set; } = null!;

        public decimal DepositAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public string? CustomerName { get; set; }

        public string? CustomerAddress { get; set; }

        public string? CustomerPhone { get; set; }

        public string? Status { get; set; }

        public DateOnly? CreatedAt { get; set; }

        public DateOnly? UpdatedAt { get; set; }

        public int ProductID { get; set; }

        public SamProduct Product { get; set; } = null!;
    }
}

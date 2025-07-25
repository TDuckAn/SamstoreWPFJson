using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamStoreWPFJson_BOs
{
    public class SamProduct
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; } = null!;

        public int Ram { get; set; }

        public int Storage { get; set; }

        public decimal Price { get; set; }

        public DateOnly? CreatedAt { get; set; }

        public DateOnly? UpdatedAt { get; set; }

        public ICollection<SamPreOrder> SamPreOrders { get; set; } = new List<SamPreOrder>();
    }
}

using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ItemPurchaseOrderDTO
    {
        public int OrderId { get; set; }
        public string PurchaseOrderDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public string PurchaseOrderCode { get; set; }
        public string Vendor { get; set; }
        public int? ItemCategoryId { get; set; }
        public string Items { get; set; }
        public bool IsReceipt { get; set; }
        public IEnumerable<ItemPurchaseGoods> itemPurchaseGoods { get; set; }
    }
}

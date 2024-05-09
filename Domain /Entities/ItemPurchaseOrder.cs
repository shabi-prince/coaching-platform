using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ItemPurchaseOrder : GenericModel
    {
        [Required]
        public DateTime PurchaseOrderDate { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PurchasePrice { get; set; }
        public string PurchaseOrderCode { get; set; }
        public int CashDeliver { get; set; }        
        public bool IsReceipt { get; set; }
        public DateTime? DueDate { get; set; }
        public IEnumerable<ItemPurchaseGoods> ItemPurchaseGoods { get; set; }
        public Vendor? Vendor { get; set; }        
        public List<VendorReceipt>? VendorReceipts { get; set; }

    }
}

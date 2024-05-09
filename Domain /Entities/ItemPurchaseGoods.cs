using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ItemPurchaseGoods : GenericModel
    {
        public int quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }
        [ForeignKey("ItemPurchaseOrder")]
        public int? PurchaseOrderId { get; set; }
        [ForeignKey("NegatePurchaseOrder")]
        public int? NegatePurchaseOrderId { get; set; }
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public virtual Item? Item { get; set; }
        public virtual ItemPurchaseOrder? ItemPurchaseOrder { get; set; }
        public virtual NegatePurchaseOrder? NegatePurchaseOrder { get; set; }

    }
}

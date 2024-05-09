using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Item : GenericModel
    {
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        [ForeignKey("ItemCategory")]
        public int? ItemCategoryId { get; set; }
        [ForeignKey("MedicineType")]
        public int? MedicineTypeId { get; set; }
        public int? Stock { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Cost { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Price { get; set; }
        public virtual ItemCategory? ItemCategory { get; set; }
        public IEnumerable<ItemPurchaseGoods>? ItemPurchaseGoods { get; set; }
        public IEnumerable<ItemSaleGoods>? ItemSaleGoods { get; set; }

    }
}

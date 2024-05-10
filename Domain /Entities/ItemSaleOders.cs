
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ItemSaleOders : GenericModel
    {
        public DateTime SaleOrderDate { get; set; }
        public string SaleOrderCode { get; set; }
         [ForeignKey("ItemCategory")]
        public int? ItemCategoryId { get; set; }
        [ForeignKey("PlayerMemberShip")]
        public int? PlayerId { get; set; }
        public bool IsReceipt { get; set; }
        public bool IsPlayerRegistered { get; set; } = false;
        public DateTime? DueDate { get; set; }
        public List<CustomerReceipt>? CustomerReceipts { get; set; }
        public virtual PlayerMemberShip? PlayerMemberShip { get; set; }        
        public virtual ItemCategory? ItemCategory { get; set; }
    }
}


﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PaymentVoucherDetail : GenericModel
    {
        [ForeignKey("PaymentVoucher")]
        public int PaymentVoucherId { get; set; }
        public string? Description { get; set; }
        public double Amount { get; set; }
        public virtual PaymentVoucher? PaymentVoucher { get; set; }
    }
}

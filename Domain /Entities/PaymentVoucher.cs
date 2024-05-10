using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PaymentVoucher : GenericModel
    {
        public string VoucherCode { get; set; }
        public DateTime VoucherDate { get; set; }
        [ForeignKey("AccountCodes")]
        public int AccountCodeId { get; set; }
        [ForeignKey("PaymentMethods")]
        public int PaymentMethod_id { get; set; }
        public double VoucherAmount { get; set; }       
        public virtual IEnumerable<PaymentVoucherDetail> PaymentVoucherDetails { get; set; }

    }
}

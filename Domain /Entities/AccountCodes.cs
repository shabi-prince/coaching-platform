using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AccountCodes : GenericModel
    {
        [ForeignKey("AccountHead")]
        public int AccountHeadId { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public virtual AccountHead? AccountHead { get; set; }
        public virtual List<PaymentVoucher>? PaymentVouchers { get; set; }
        public virtual List<ReceiptVoucher>? ReceiptVoucher { get; set; }

    }
}

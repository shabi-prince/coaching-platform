using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AccountHead : GenericModel
    {
        public string AccountHeadName { get; set; }
        public string? AccountHeadDescription { get; set; }
        public virtual IEnumerable<AccountCodes> AccountCodes { get; set; }
    }
}

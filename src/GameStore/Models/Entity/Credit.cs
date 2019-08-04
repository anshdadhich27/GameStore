using GameStore.Models.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class Credit
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public bool IsBlocked { get; set; }
        public decimal Amount { get; set; }
        public string Trancaction_Id { get; set; }
        public string Transaction_Type { get; set; }
        public string Transaction_For { get; set; }
        public string Comments { get; set; }
        public DateTime AddedOn { get; set; }

        public Credit()
        {
            Trancaction_Id = DbHelper.GetUniqueKey();
            Transaction_Type = "Credit";
        }

        public Credit(string trancaction_type)
        {
            Trancaction_Id = DbHelper.GetUniqueKey();
            Transaction_Type = trancaction_type;
        }

    }
}

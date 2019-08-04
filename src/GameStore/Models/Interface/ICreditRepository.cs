using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface ICreditRepository
    {
        Task<int> Add_New(Credit obj);
        Task<IEnumerable<Credit>> Get_logs_By_User_Id(long UserId);
        Task<int> Unblock_By_Transaction_Id(string Trancaction_Id);
    }
}

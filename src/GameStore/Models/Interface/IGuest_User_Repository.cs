using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IGuest_User_Repository
    {
        Task<long> Add_New_Async(Guest_User obj);
        Task<bool> Check_Async(Guest_User obj);
        Task<int> Delete_Async(Guest_User obj);
    }
}

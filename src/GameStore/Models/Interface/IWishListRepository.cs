using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IWishListRepository
    {
        Task<bool> Add_New(WishList obj);
        Task<int> Delete_By_Id(long Id);
        Task<WishList_DTO> Get_All_By_User_Id(long UserId);
    }
}

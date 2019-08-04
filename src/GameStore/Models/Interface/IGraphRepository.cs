using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IGraphRepository
    {
        Task<IEnumerable<Total_Count>> Get_Selling_Count_By_Product_Type();
        Task<IEnumerable<Total_Count>> Get_Buying_Count_By_Product_Type();

        Task<IEnumerable<Total_Count>> Get_Pending_Order_Count();
        Task<IEnumerable<Total_Count>> Get_Todays_Order_Count();

        DateWiseGraph Get_Order_Count_By_Month(string cartType, int month, int year);
    }
}

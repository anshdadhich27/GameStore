using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IOrder_Repository
    {
        Task<int> Add_New_Cancelation_Request(Cancelation_Order obj);
        Task<PaginatedEntity<Cancelation_Order>> Get_Cancelation_Orders_By_Query(string query);
        Task<PaginatedEntity<Cancelation_Order>> Get_Order_Cancelation_List(long offset, int row);
    }
}

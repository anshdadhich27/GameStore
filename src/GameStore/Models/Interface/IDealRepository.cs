using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IDealRepository
    {
        Task<Deal> Get_By_Id(int Id);
        Task<int> Delete_By_Id(int Id);
        Task<Deal> Add_new(Deal obj);
        Task<Deal> Update_By_Id(Deal obj);

        Task<PaginatedEntity<Deal>> GetPaginated(int rows, long offset);
        Task<IEnumerable<Deal_Product>> Get_Product_By_Id(int Id);
        Task<PaginatedEntity<Deal>> Search_And_GetPaginated(SearchQuery obj);
    }
}

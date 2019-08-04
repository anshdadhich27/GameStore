using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IProductRepository
    {
        
        Task<Product> Get_By_Id(long Id);
        Task<bool> Delete_By_Id(long Id);
        Task<long> Add_New(Product obj);
        Task<bool> Update_By_Id(Product obj);
        Task<Product> Get_Details_By_SKU(string SKU);
        Task<PaginatedEntity<Product>> GetPaginated(SearchQuery obj);
        Task<Product> Get_By_Url_Type(string URL, string ProductType);
        Task<PaginatedEntity<Product>> GetPaginated_By_TypeId(SearchQuery obj);
        Task<PaginatedEntity<Product>> GetPaginated_By_Custom_QueryAsync(string query);
        Task<Product> Get_By_Url_Type_Condition(string URL, string ProductType, string Condition);
    }
}

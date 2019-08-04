using GameStore.Models.DataProvider;
using GameStore.Models.Entity;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface ICollectionRepository
    {
        Task<DbResult> Add_New_IgdbCollectionAsync(Collection obj);
        Task<DbResult> IsIgdbCollection_ExistsAsync(long id);
        Task<DbResult> Add_NewAsync(Collection obj);
        Task<DbResult> DeleteByIdAsync(long Id);
        Task<PaginatedEntity<Collection>> Search_And_GetPaginatedAsync(SearchQuery obj);
        Task<PaginatedEntity<Collection>> GetPaginatedAsync(long offset, int rows);
        Task<DbResult> UpdateAsync(Collection obj);
        Task<long> Get_Last_IdgbId();
    }
}

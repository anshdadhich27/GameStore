using GameStore.Models.DataProvider;
using GameStore.Models.Entity;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IPlatformRepository
    {
        Task<DbResult> IsIdgbPlatform_ExistsAsync(long id);
        Task<DbResult> Add_New_IdgbPlatformAsync(Platform obj);
        Task<DbResult> Add_NewAsync(Platform obj);
        Task<DbResult> DeleteByIdAsync(long Id);
        Task<PaginatedEntity<Platform>> Search_And_GetPaginatedAsync(SearchQuery obj);
        Task<PaginatedEntity<Platform>> GetPaginatedAsync(long offset, int rows);
        Task<DbResult> UpdateAsync(Platform obj);
        Task<long> Get_Last_IdgbId();
    }
}

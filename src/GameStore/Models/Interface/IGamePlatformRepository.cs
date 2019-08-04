using GameStore.Models.DataProvider;
using GameStore.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IGamePlatformRepository
    {
        Task<PaginatedEntity<GamePlatform>> Search_And_GetPaginatedAsync(SearchQuery obj);
        Task<PaginatedEntity<GamePlatform>> GetPaginatedAsync(long offset, int rows);
        Task<IEnumerable<GamePlatform>> Get_Supported_N_Platforms(int top, string Ids);
        Task<DbResult> DeleteByIdAsync(int Id);
        Task<DbResult> Add_NewAsync(GamePlatform obj);
        Task<DbResult> UpdateAsync(GamePlatform obj);
        Task<GamePlatform> GetByUrlAsync(string Url);
        Task<IEnumerable<GamePlatform>> GetTopNAsync(int N);
        Task<long> Get_Last_IdgbId();
        Task<DbResult> IsIdgbPlatform_ExistsAsync(long id);
        Task<DbResult> Add_New_From_IdgbAsync(GamePlatform obj);
        Task<IEnumerable<GamePlatform>> Get_ALL_ActiveAsync();
        Task<GamePlatform> GetById(int Id);
        Task<PaginatedEntity<GamePlatformExport>> GetGamePlatform_By_Custom_QueryAsync(string query);

    }
}

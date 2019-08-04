using GameStore.Models.DataProvider;
using GameStore.Models.Entity;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IGameEngineRepository
    {
        Task<DbResult> IsIdgb_GameEngine_ExistsAsync(long id);
        Task<DbResult> Add_New_IdgbGame_EngineAsync(GameEngine obj);
        Task<DbResult> Add_NewAsync(GameEngine obj);
        Task<DbResult> DeleteByIdAsync(long Id);
        Task<PaginatedEntity<GameEngine>> Search_And_GetPaginatedAsync(SearchQuery obj);
        Task<PaginatedEntity<GameEngine>> GetPaginatedAsync(long offset, int rows);
        Task<DbResult> UpdateAsync(GameEngine obj);
        Task<long> Get_Last_IdgbId();
    }
}

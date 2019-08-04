using GameStore.Models.DataProvider;
using GameStore.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IGameModeRepository
    {
        Task<DbResult> Add_NewAsync(GameMode obj);
        Task<DbResult> UpdateAsync(GameMode obj);
        Task<DbResult> DeleteByIdAsync(long ID);
        Task<DbResult> Add_New_IdgbGame_ModeAsync(GameMode obj);
        Task<DbResult> IsIdgb_Game_Mode_ExistsAsync(long id);
        Task<IEnumerable<GameMode>> Get_ALL_ActiveAsync();
        Task<PaginatedEntity<GameMode>> GetPaginatedAsync(long offset, int rows);
        Task<PaginatedEntity<GameMode>> Search_And_GetPaginatedAsync(SearchQuery obj);
        Task<long> Get_Last_IdgbId();
    }
}

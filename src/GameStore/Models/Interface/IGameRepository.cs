using GameStore.Models.DataProvider;
using GameStore.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IGameRepository
    {
        Task<DbResult> Add_NewGame_From_Idgb_ApiAsync(Game obj);
        Task<DbResult> IsIdgbGame_ExistsAsync(long id);
        Task<DbResult> Add_New_Release_Date_From_IGDbAsync(Game_Release_Date obj);

        Task<PaginatedEntity<Game>> GetPaginated_By_PlatformId(long offset, int rows, long PlatformId);
        Task<PaginatedEntity<Game>> GetPaginated_By_PlatformId_From_View(long offset, int rows, long PlatformId);
        Task<Game_DTO> Get_Details_By_Url_Platform_n_Condition(string NameUrl, string PlatformUrl, int Condition_Id);
        Task<PaginatedEntity<Game>> GetPaginated_By_PlatformId_GenreId(long offset, int rows, long PlatformId, long GenreId);        
        Task<PaginatedEntity<Game>> GetPaginated_By_PlatformId_GenreId_From_View(long offset, int rows, long PlatformId, long GenreId);
        Task<PaginatedEntity<Game>> GetPaginated_By_PlatformId_GenreId_ConditionId_From_View(long offset, int rows, long PlatformId, long GenreId, int ConditionId);
        Task<PaginatedEntity<Game>> GetPaginated_By_PlatformId_ConditionId_From_View(long offset, int rows, long PlatformId, int ConditionId);
        Task<Game_DTO> Get_Details_By_Url_Platform(string NameUrl, string PlatformUrl);
        Task<PaginatedEntity<Game>> GetPaginated_By_Custom_QueryAsync(string query);
        Task<PaginatedEntity<Game>> GetPaginated_By_Custom_master_QueryAsync(string query);
        
        Task<PaginatedEntity<GameListExport>> GetGame_By_Custom_QueryAsync(string query);
        Task<DbResult> Update_Game_DetailsAsync(Game obj);
        Task<DbResult> Add_NewGame_ManuallyAsync(Game obj);
        Task<IEnumerable<Game>> Get_ALL_Witch_Has_No_Cover_Image();
        Task<IEnumerable<Game>> Get_ALL_Which_Has_No_VideosAsync();
        Task<Game_DTO> Get_Details_By_Url(string NameUrl);

        Task<long> Get_Last_IdgbId();
        Task<Game_DTO> Get_Details_By_Id(long id);
        Task<DbResult> DeleteByIdAsync(long Id);
        Task<Game> Get_Details_By_SKU(string SKU);
    }
}

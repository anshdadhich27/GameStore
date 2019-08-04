using GameStore.Models.DataProvider;
using GameStore.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IGenereRepository
    {
        Task<PaginatedEntity<Genere>> Search_And_GetPaginatedAsync(SearchQuery obj);
        Task<PaginatedEntity<Genere>> GetPaginatedAsync(long offset, int rows);
        Task<IEnumerable<Genere>> Get_Supported_N_Genere(int top, string Ids);
        Task<DbResult> DeleteByIdAsync(int Id);
        Task<DbResult> Add_NewAsync(Genere obj);
        Task<DbResult> UpdateAsync(Genere obj);
        Task<Genere> GetByUrlAsync(string Url);
        Task<IEnumerable<Genere>> GetTopNAsync(int N);
        Task<DbResult> IsIdgb_Genere_ExistsAsync(long id);
        Task<DbResult> Add_New_From_IdgbAsync(Genere obj);
        Task<long> Get_Last_IdgbId();
        Task<IEnumerable<Genere>> Get_ALL_ActiveAsync();
        Task<PaginatedEntity<Genere_CSV>> GetGenere_By_Custom_QueryAsync(string obj);
    }
}

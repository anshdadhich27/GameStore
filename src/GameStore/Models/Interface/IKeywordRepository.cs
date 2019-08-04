using GameStore.Models.DataProvider;
using GameStore.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IKeywordRepository
    {
        Task<DbResult> Add_New_IgdbKeywordAsync(Keyword obj);
        Task<DbResult> IsIgdbKeywords_ExistsAsync(long id);
        Task<DbResult> Add_NewAsync(Keyword obj);
        Task<DbResult> DeleteByIdAsync(long Id);
        Task<PaginatedEntity<Keyword>> Search_And_GetPaginatedAsync(SearchQuery obj);
        Task<PaginatedEntity<Keyword>> GetPaginatedAsync(long offset, int rows);
        Task<IEnumerable<Keyword>> Get_ALL_ActiveAsync();
        Task<DbResult> UpdateAsync(Keyword obj);
        Task<long> Get_Last_IdgbId();
    }
}

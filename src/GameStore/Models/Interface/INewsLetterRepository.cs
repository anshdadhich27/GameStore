using GameStore.Models.DataProvider;
using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface INewsLetterRepository
    {
        Task<DbResult> NewsLetter_Subscriber_Add_New(NewsLetter obj);
        Task<DbResult> NewsLetter_Subscriber_Verify(string Security_Key);
        Task<PaginatedEntity<NewsLetter>> GetPaginatedAsync(long offset, int rows);
        Task<PaginatedEntity<NewsLetter>> Search_And_GetPaginatedAsync(SearchQuery obj);
        Task<int> Delete_By_Id(int Id);
    }
}

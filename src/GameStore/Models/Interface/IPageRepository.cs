using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IPageRepository
    {
        Task<int> Add_New(PageContent obj);
        Task<int> Update_By_Id(PageContent obj);
        Task<int> Update_Status_Id(PageContent obj);
        Task<int> Delete_By_Id(int Id);
        Task<PageContent> Get_By_Id(int Id);
        Task<PageContent> Get_By_Url(string URL);
        Task<PaginatedEntity<PageContent>> GetPaginatedAsync(long offset, int rows);
        Task<PaginatedEntity<PageContent>> Search_And_GetPaginatedAsync(SearchQuery obj);
    }
}

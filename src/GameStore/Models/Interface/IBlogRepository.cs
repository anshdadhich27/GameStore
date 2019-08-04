using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IBlogRepository
    {
        
        Task<Blog> Get_By_Id(int Id);
        Task<Blog> Get_By_Link(string Link);
        Task<bool> Delete_By_Id(int Id);
        Task<bool> Add_New(Blog obj);
        Task<bool> Update_By_Id(Blog obj);
        Task<IEnumerable<Blog>> GetTopN_News(int N);
        Task<IEnumerable<Blog>> GetTopN_EditorsPic(int N);
        Task<IEnumerable<Blog>> GetTopNAsync(int N);
        Task<PaginatedEntity<Blog>> Search_And_GetPaginatedAsync(SearchQuery obj);
        Task<PaginatedEntity<Blog>> GetPaginatedAsync(long offset, int rows);
    }
}

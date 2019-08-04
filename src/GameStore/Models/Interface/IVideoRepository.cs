using GameStore.Models.DataProvider;
using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IVideoRepository
    {
        Task<DbResult> Add_NewAsync(Video obj);
        Task<int> Remove_By_Id_Async(long Id);
        Task<int> Add_or_Update_Multiple(IEnumerable<Video> list);
        Task<DbResult> Videos_Get_By_ReferenceId_Video_Of(long ReferenceId, string Video_of);
    }
}

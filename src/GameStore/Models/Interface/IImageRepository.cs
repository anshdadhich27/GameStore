using GameStore.Models.DataProvider;
using GameStore.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IImageRepository
    {
        Task<DbResult> Add_New_ImageAsync(Images obj);
        Task<DbResult> Delete_Image_ByIdAsync(long Id);
        Task<int> Remove_By_Url_Async(string Url);
        Task<int> Add_New_ImagesAsync(IEnumerable<Images> list);
        Task<DbResult> Delete_Image_By_ReferenceIdAsync(long ReferenceId);
        Task<DbResult> Get_Image_By_ReferenceId_ImageType_ImageOfAsync(long ReferenceId, string ImageType, string ImageOf);
        Task<DbResult> Get_Image_By_ReferenceId_ImageOfAsync(long ReferenceId, string ImageOf);
        Task<PaginatedEntity<ConsoleListExport>> GetConsole_By_Custom_QueryAsync(string query);
    }
}

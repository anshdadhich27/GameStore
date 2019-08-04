using GameStore.Models.DataProvider;
using GameStore.Models.Entity;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IReviewRepository
    {
        Task<DbResult> Add_New_IgdbReviewAsync(Review obj);
        Task<DbResult> Add_New_Review(Review obj);
        Task<DbResult> Update_By_Id(Review obj);
        Task<DbResult> IsIgdbReview_ExistsAsync(long id);
        Task<PaginatedEntity<Review>> GetPaginated(int rows, long offset);
        Task<int> Delete_By_Id(long Id);
    }
}

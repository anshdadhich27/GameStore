using GameStore.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IVendorRepository
    {
        Task<PaginatedEntity<Vendor>> Search_And_GetPaginated(SearchQuery obj);
        Task<PaginatedEntity<Vendor>> GetPaginated(long offset, int rows);
        Task<int> Delete_By_Id(int Id);
        Task<Vendor> Get_By_Id(int Id);
        Task<Vendor> Add_New(Vendor obj);
        Task<Vendor> Update_By_Id(Vendor obj);
        Task<IEnumerable<Vendor>> Get_All();
    }
}

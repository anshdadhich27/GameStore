using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IBannerRepository
    {
        Task<IEnumerable<Banner>> Get_All_Active();
        Task<IEnumerable<Banner>> Get_All();
        Task<Banner> Add_new(Banner obj);
        Task<int> Update_By_Id(Banner obj);
        Task<int> Delete_By_Id(int Id);
    }
}

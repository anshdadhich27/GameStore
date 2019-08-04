using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IAdsRepository
    {
        Task<Ads> Get_By_Page_Name(string PageName);
        Task<IEnumerable<Ads>> Get_All();
        Task<int> Delete_By_Id(int Id);
        Task<int> Update_By_Id(Ads obj);
        Task<Ads> Add_New(Ads obj);
    }
}

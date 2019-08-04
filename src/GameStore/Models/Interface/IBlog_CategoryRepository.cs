using GameStore.Models.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface ICommon_Name_UrlRepository
    {
        Task<IEnumerable<Common_Name_Url>> Get_All_Async();
        Task<bool> Delete_By_Id(int Id);
        Task<bool> Add_New(Common_Name_Url obj);
        Task<bool> Update_By_Id(Common_Name_Url obj);
        Task<IEnumerable<Common_Name_Url>> Get_All_By_Type(string type);
    }
}

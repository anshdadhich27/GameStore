using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> Get_All_Async();        
        Task<bool> Add_New_Async(Category obj);
        Task<bool> Update_By_Id(Category obj);
        Task<bool> Delete_By_Id(int Id);
    }
}

using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IOperatorRepository
    {
        Task<bool> Add_New(User obj);
        Task<int> Update_By_Id(User obj);
        Task<IEnumerable<User>> Get_All();
        Task<User> Get_By_Credentials(string Email, string Password);
    }
}

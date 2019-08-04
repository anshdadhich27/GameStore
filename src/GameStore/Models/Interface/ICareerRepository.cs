using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface ICareerRepository
    {
        Task<bool> Verify_Email(long Id, int OTP);
        Task<int> Delete_By_Id(long Id);
        Task<Career> Add_New(Career obj);
    }
}

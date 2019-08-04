using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IAddressRepository
    {
        Task<int> Add_New(Address obj);
        Task<int> Add_Multiple_address(IEnumerable<Address> list);
        Task<IEnumerable<Address>> Get_By_UserId(long UserId);
        Task<Address> Get_By_Id(long Id);
        Task<int> Delete_By_Id(long Id);
    }
}

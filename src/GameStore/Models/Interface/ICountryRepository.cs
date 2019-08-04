using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> Get_All();
    }
}

using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IGamePlatformMapingRepository
    {
        Task<int> Remove_Maping_By_Sku(string sku);
        Task<int> Add_New_Maping(GamePlatformMaping obj);
        Task<int> Add_New_Mapings(IEnumerable<GamePlatformMaping> list);
        Task<IEnumerable<GamePlatformMaping>> Get_By_Game_Id(long Game_Id);
        Task<int> Remove_Maping_Async(long Game_Id, int Platform_Id, int Condition_Id);

    }
}

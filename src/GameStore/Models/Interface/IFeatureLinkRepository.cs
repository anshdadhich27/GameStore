using GameStore.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Interface
{
    public interface IFeatureLinkRepository
    {
        Task<IEnumerable<FeatureLink>> Get_All();
        Task<int> Add_New(IList<FeatureLink> list);
        Task<int> Delete_By_Reference(long Reference_Id, string Reference_Name);
    }
}

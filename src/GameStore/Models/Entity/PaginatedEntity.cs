using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class PaginatedEntity<T> where T : class
    {
        public IEnumerable<T> PagedSet { get; set; }       
        public long TotalCount { get; set; }        
        public int NumResult { get; set; }
        public decimal? MaxPrice { get; set; }
        public PaginatedEntity()
        {
            NumResult = 0;
            TotalCount = 0;
            PagedSet = new List<T>();
            MaxPrice = 0;
        }
    }
}

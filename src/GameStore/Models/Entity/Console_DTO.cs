using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class Console_DTO
    {
        public string Name { get; set; }
        public string NameUrl { get; set; }

        public IEnumerable<Product> ProductList { get; set; }

        public Console_DTO()
        {
            ProductList = new List<Product>();
        }
    }
}

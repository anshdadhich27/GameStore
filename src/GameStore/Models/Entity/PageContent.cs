using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Models.Entity
{
    public class PageContent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string Title { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string HeaderContent { get; set; }
        public string BodyContent { get; set; }
        public bool Active { get; set; } = true;
        public bool CanDelete { get; set; } = true;
        public bool ShowMenu { get; set; } = true;
        public DateTime AddedOn { get; set; } = DateTime.Now;
    }
}

using SushmaElectrical.ViewModels.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SushmaElectrical.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public class PagedCategoryViewModel
        {
            public List<CategoryViewModel> Categories { get; set; }
            public PageInfo PageInfo { get; set; }
        }
    }
}

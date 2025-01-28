using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldsBelly.DataAccess.Models
{
    public class PaginationSettings
    {
        private const int MaxPageSize = 1000;

        public int Page { get; set; } = 1;

        private int _pageSize = 50;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value < MaxPageSize) ? value : MaxPageSize;
        }
    }
}

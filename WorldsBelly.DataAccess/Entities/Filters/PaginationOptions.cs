
namespace WorldsBelly.DataAccess.Entities.Filters
{
    public class PaginationOptions
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
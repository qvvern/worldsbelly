using System.Collections.Generic;


namespace WorldsBelly.DataAccess.Entities
{
    public class CollectionResponse<T>
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int Count { get; set; }
        public int TotalCount { get; set; }
        public ICollection<T> Items { get; set; }
    }
}
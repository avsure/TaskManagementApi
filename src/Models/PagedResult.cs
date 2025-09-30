namespace TaskManagementApi.Models
{
    public class PagedResult<T> where T : class
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}

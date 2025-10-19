namespace Ovr.Domain.Responses
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalRows { get; set; }

        public PagedResult()
        {
        }

        public PagedResult(List<T> items, int totalRows)
        {
            Items = items ?? new List<T>();
            TotalRows = totalRows;
        }
    }
}

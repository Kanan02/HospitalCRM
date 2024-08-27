namespace Application.Models.Response.Base
{
    public class PagedResponseList<T>
    {
        public IReadOnlyList<T>? List { get; set; }
        public int Count { get; set; }
    }
}

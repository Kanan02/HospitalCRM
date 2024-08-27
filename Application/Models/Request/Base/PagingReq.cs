namespace Application.Models.Request.Base
{
    public class PagingReq<T> : BaseReq<T>
    {
        public PagingOptions? Pager { get; set; }
    }

    public class PagingOptions
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int Skip { get => (CurrentPage - 1) * PageSize; }
    }
    public class PagingReq
    {
        public PagingOptions? Pager { get; set; }
    }
}

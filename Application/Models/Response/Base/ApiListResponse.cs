namespace Application.Models.Response.Base
{
    public class ApiListResponse<T> : ApiResponse
    {
        public List<T> List { get; set; }
        public ApiListResponse() { }
        public ApiListResponse(List<T> list) => List = list;
        public ApiListResponse(string error) : base(error) { }
    }
}

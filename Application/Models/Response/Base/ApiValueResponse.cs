namespace Application.Models.Response.Base
{
    public class ApiValueResponse<T> : ApiResponse
    {
        public ApiValueResponse(T value) => Value = value;
        public ApiValueResponse(string error) : base(error) { }

        public T Value { get; set; }
    }
}

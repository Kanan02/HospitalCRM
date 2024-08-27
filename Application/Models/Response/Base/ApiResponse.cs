namespace Application.Models.Response.Base
{
    public class ApiResponse
    {
        public bool Success { get => Errors == null || Errors.Count() == 0; }
        public List<BrokenRule> Errors { get; set; }
        public ApiResponse() => Errors = new List<BrokenRule>();

        public ApiResponse(string error) : this() => Errors.Add(new BrokenRule { Error = error });

        public ApiResponse(List<BrokenRule> errors) => Errors = errors;
    }

    public class BrokenRule
    {
        public BrokenRule() { }
        public BrokenRule(string error) : this() => Error = error;
        public BrokenRule(string key, string error) : this()
        {
            Error = error;
            Key = key;
        }
        public string Error { get; set; }
        public string Key { get; set; }

    }
}

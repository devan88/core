namespace Core.HttpClient
{
    public class HttpResult
    {
        public bool IsSuccess { get; set; }
        public HttpError HttpError { get; set; }
    }

    public class HttpResult<T> : HttpResult
    {
        public T? Data { get; set; }
    }
}

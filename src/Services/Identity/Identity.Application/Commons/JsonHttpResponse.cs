namespace Identity.Application.Commons;
public class JsonHttpResponse<T>
{
    public int Code { get; set; }
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public object? Trace { get; set; }

    public JsonHttpResponse()
    {

    }

    public JsonHttpResponse(int code, bool isSuccess, T? data, string? message = null, object? trace = null)
    {
        Code = code;
        IsSuccess = isSuccess;
        Data = data;
        Message = message;
        Trace = trace;
    }

    public static JsonHttpResponse<T> Ok(T? data, string? message)
    {
        return new(200, true, data, message);
    }

    public static JsonHttpResponse<T> Ok(T? data)
    {
        return new(200, true, data);
    }

    public static JsonHttpResponse<T> ErrorNotFound(T? data, string? message, object? trace)
    {
        return new JsonHttpResponse<T>(404, false, data, message, trace);
    }

    public static JsonHttpResponse<T> ErrorBadRequest(T? data, string? message, object? trace)
    {
        return new JsonHttpResponse<T>(400, false, data, message, trace);
    }

    public static JsonHttpResponse<T> ErrorBadRequest(string? message, object? trace)
    {
        return new JsonHttpResponse<T>(400, false, default, message, trace);
    }

    public static JsonHttpResponse<T> ErrorBadRequest(string? message)
    {
        return new JsonHttpResponse<T>(400, false, default, message, default);
    }
}

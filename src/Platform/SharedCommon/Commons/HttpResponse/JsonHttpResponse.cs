using MediatR;

namespace SharedCommon.Commons.HttpResponse;

public class JsonHttpResponse
{
	public JsonHttpResponse()
	{
	}

	public JsonHttpResponse(int status, bool isSuccess, object? data, string? message = null, object? errors = null)
	{
		Status = status;
		IsSuccess = isSuccess;
		Data = data;
		Message = message;
		Errors = errors;
	}

	public int Status { get; set; }
	public bool IsSuccess { get; set; }
	public object? Data { get; set; }
	public string? Message { get; set; }
	public object? Errors { get; set; }

	public static JsonHttpResponse Success(object? data, string? message)
	{
		data ??= Unit.Value;
		return new JsonHttpResponse(200, true, data, message);
	}

	public static JsonHttpResponse Success(object? data = default)
	{
		data ??= Unit.Value;
		return new JsonHttpResponse(200, true, data);
	}

	public static JsonHttpResponse Fail(string? message = default)
	{
		message ??= "Cannot find anything!";
		return new JsonHttpResponse(400, false, default, message);
	}
}

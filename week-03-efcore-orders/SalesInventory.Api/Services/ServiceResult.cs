namespace SalesInventory.Api.Services;

public class ServiceResult<T>
{
    private ServiceResult(bool succeeded, T? value, string? error)
    {
        Succeeded = succeeded;
        Value = value;
        Error = error;
    }

    public bool Succeeded { get; }
    public T? Value { get; }
    public string? Error { get; }

    public static ServiceResult<T> Success(T value)
    {
        return new ServiceResult<T>(true, value, null);
    }

    public static ServiceResult<T> Failure(string error)
    {
        return new ServiceResult<T>(false, default, error);
    }
}

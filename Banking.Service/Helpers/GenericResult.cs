namespace Banking.Service.Helpers;
public class GenericResult<T>
{
    public bool IsSuccess { get; private set; }
    public T Data { get; private set; }
    public string Error { get; private set; }

    public static GenericResult<T> Success(T data)
    {
        return new GenericResult<T> { IsSuccess = true, Data = data };
    }

    public static GenericResult<T> Failure(string error)
    {
        return new GenericResult<T> { IsSuccess = false, Error = error };
    }
}


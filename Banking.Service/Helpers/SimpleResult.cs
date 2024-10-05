namespace Banking.Service.Helpers;
public class Result
{
    public bool IsSuccess { get; private set; }
    public string Error { get; private set; }

    private Result(bool isSuccess, string error = null)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    // Success result
    public static Result Success()
    {
        return new Result(true);
    }

    // Failure result with an error
    public static Result Failure(string error)
    {
        return new Result(false, error);
    }
}


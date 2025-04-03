namespace MotoLocadora.BuildingBlocks.Core;

public class OperationResult
{
    public bool IsSuccess { get; set; }
    public List<string> Errors { get; set; } = new();
    public static OperationResult Success() => new() { IsSuccess = true };
    public static OperationResult Failure(params string[] errors) => new() { IsSuccess = false, Errors = errors.ToList() };
    public static OperationResult Failure(IEnumerable<string> errors) => new() { IsSuccess = false, Errors = errors.ToList() };
}

public class OperationResult<T> : OperationResult
{
    public T? Value { get; set; }

    public static OperationResult<T> Success(T value) => new() { IsSuccess = true, Value = value };
    public new static OperationResult<T> Failure(params string[] errors) => new() { IsSuccess = false, Errors = errors.ToList() };
    public new static OperationResult<T> Failure(IEnumerable<string> errors) => new() { IsSuccess = false, Errors = errors.ToList() };
}
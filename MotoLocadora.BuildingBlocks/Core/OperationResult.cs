namespace MotoLocadora.BuildingBlocks.Core;


public class OperationResult
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = new();

    public static OperationResult Success(string? message = null) => new() { IsSuccess = true, Message = message };
    public static OperationResult Failure(params string[] errors) => new() { IsSuccess = false, Errors = errors.ToList() };
    public static OperationResult Failure(IEnumerable<string> errors) => new() { IsSuccess = false, Errors = errors.ToList() };
}

public class OperationResult<T> : OperationResult
{
    public T? Value { get; set; }

    public static OperationResult<T> Success(T value, string? message = null) => new() { IsSuccess = true, Value = value, Message = message };
    public new static OperationResult<T> Failure(params string[] errors) => new() { IsSuccess = false, Errors = errors.ToList() };
    public new static OperationResult<T> Failure(IEnumerable<string> errors) => new() { IsSuccess = false, Errors = errors.ToList() };
}